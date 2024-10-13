using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private GameTile gameTilePrefab;
    [SerializeField] private TextMeshProUGUI healthText;
    

    private LevelLibrary _levelLibrary;
    private GameTile[,] _tileMatrix;
    private int _levelHeight;
    private int _levelWidth;
    private int _currentHealth;
    private int _maxHealth;
    private int _totalEnemyCount;

    private int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value <= 0)
            {
                value = 0;
                EventManager.LevelFailed();
            }
            
            _currentHealth = value;
            healthText.text = $"{value}/{_maxHealth}";
        }
    }
    
    private void Start()
    {
        EventManager.OnEnemyReadyToMove += EnemyReadyToMoveEventHandler;
        EventManager.OnDefenceItemReadyToAttack += DefenceItemReadyToAttackEventHandler;
        EventManager.OnEnemyReachedBase += EnemyReachedBaseEventHandler;
        EventManager.OnEnemyDied += EnemyDiedEventHandler;
    }

    

    private void OnDestroy()
    {
        EventManager.OnEnemyReadyToMove -= EnemyReadyToMoveEventHandler;
        EventManager.OnDefenceItemReadyToAttack -= DefenceItemReadyToAttackEventHandler;
        EventManager.OnEnemyReachedBase -= EnemyReachedBaseEventHandler;
        EventManager.OnEnemyDied -= EnemyDiedEventHandler;
    }

    private void EnemyReadyToMoveEventHandler(object sender, EventManager.EnemyReadyToMoveEventArgs args)
    {
        var (x, y) = (args.Enemy.xIndex, args.Enemy.yIndex);
        if (y == 0)
        {
            EventManager.EnemyReachedBase();
            Destroy(args.Enemy.gameObject);
            return;
        }

        args.Enemy.yIndex = y - 1;
        _tileMatrix[x, y].presentEnemy = null;
        _tileMatrix[x, y - 1].presentEnemy = args.Enemy;
        
        args.Enemy.MoveToTile(_tileMatrix[x, y - 1]);
        
        CheckAttackers();
    }

    public void Configure()
    {
        _levelLibrary = Resources.Load<LevelLibrary>("LevelLibrary");

        var levelIndex = PlayerPrefs.GetInt(Constants.LevelIndexKey, defaultValue: 0);
        
        LoadLevelByIndex(levelIndex);
    }
    
    private void LoadLevelByIndex(int index)
    {
        if(index < 0)
        {
            return;
        }
        
        var levelFormat = _levelLibrary.GetLevelByIndex(index);
        
        SetupLevel(levelFormat);
    }

    private void SetupLevel(LevelFormat levelFormat)
    {
        _levelHeight = levelFormat.height;
        _levelWidth = levelFormat.width;
        
        _tileMatrix = new GameTile[levelFormat.width, levelFormat.height];
        
        for (int j = 0; j < levelFormat.height; j++)
        {
            for (int i = 0; i < levelFormat.width; i++)
            {
                var tile = Instantiate(gameTilePrefab, parent: tileParent);

                // tile.transform.position = new Vector3(2 * i, 2 * j, 0);
                tile.xIndex = i;
                tile.yIndex = j;

                _tileMatrix[i, j] = tile;
            }
        }
        
        SetSpawnPoints(levelFormat.enemyConfiguration);

        foreach (var enemy2Count in levelFormat.enemyConfiguration)
        {
            _totalEnemyCount += enemy2Count.count;
        }

        _maxHealth = _totalEnemyCount / 2;
        CurrentHealth = _maxHealth;
        
        EventManager.LevelStarted(levelFormat);
    }

    private void DefenceItemReadyToAttackEventHandler(object sender, EventManager.DefenceItemReadyToAttackEventArgs args)
    {
        CheckRangeOfAttacker(args.DefenceItem);
    }
    
    private void CheckAttackers()
    {
        for (int i = 0; i < _levelWidth; i++)
        {
            for (int j = 0; j < _levelHeight; j++)
            {
                var tile = _tileMatrix[i, j];

                if (tile.presentDefenceItem != null)
                {
                    CheckRangeOfAttacker(tile.presentDefenceItem);
                }
            }
        }
    }

    private void CheckRangeOfAttacker(DefenceItem defenceItem)
    {
        if (!defenceItem.canAttack)
        {
            return;
        }

        for (int i = 0; i < _levelWidth; i++)
        {
            for (int j = 0; j < _levelHeight; j++)
            {
                if (!IndexInRangeOfDefenceItem(defenceItem, i, j))
                {
                    continue;
                }
                
                var presentEnemy = _tileMatrix[i, j].presentEnemy;
                if (presentEnemy != null)
                {
                    defenceItem.Attack(presentEnemy);
                    return;
                }
            }
        }
    }
    
    private void EnemyDiedEventHandler(object sender, EventManager.EnemyDiedEventArgs args)
    {
        _totalEnemyCount--;

        if (_totalEnemyCount <= 0)
        {
            EventManager.LevelWon();
        }
    }

    private void EnemyReachedBaseEventHandler(object sender, EventArgs args)
    {
        CurrentHealth--;
        _totalEnemyCount--;

        if (CurrentHealth > 0 && _totalEnemyCount <= 0)
        {
            EventManager.LevelWon();
        }
    }

    private void SetSpawnPoints(List<Enemy2Count> enemy2CountList)
    {
        var allEnemyTypes = new List<Constants.EnemyType>();

        foreach (var enemy2Count in enemy2CountList)
        {
            allEnemyTypes.AddRange(Enumerable.Repeat(enemy2Count.enemyType, enemy2Count.count));
        }
        
        // shuffle the list to randomize generation
        allEnemyTypes = allEnemyTypes.OrderBy(_ => Guid.NewGuid()).ToList();

        var enemyPerColumn = allEnemyTypes.Count / _levelWidth;
        var generationCounts = new List<int>();

        for (int i = 0; i < _levelWidth; i++)
        {
            generationCounts.Add(enemyPerColumn);
        }
        
        var remainder = allEnemyTypes.Count % _levelWidth;
        
        for (int i = 0; i < remainder; i++)
        {
            var randomColumn = Random.Range(0, _levelWidth);
            generationCounts[randomColumn]++;
        }

        var lastIndex = 0;
        for (int i = 0; i < _levelWidth; i++)
        {
            var countForThisColumn = generationCounts[i];
            _tileMatrix[i, _levelHeight-1].SetSpawnPoint(allEnemyTypes.GetRange(lastIndex, countForThisColumn));
            lastIndex += countForThisColumn;
        }
    }

    private bool IndexInRangeOfDefenceItem(DefenceItem defenceItem, int xIndex, int yIndex)
    {
        return xIndex >= defenceItem.xIndex - defenceItem.leftXOffset && 
               xIndex <= defenceItem.xIndex + defenceItem.rightXOffset &&
               yIndex >= defenceItem.yIndex - defenceItem.bottomYOffset &&
               yIndex <= defenceItem.yIndex + defenceItem.topYOffset;
    }
}

