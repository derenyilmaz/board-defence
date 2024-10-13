using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private GameTile gameTilePrefab;
    
    
    private LevelLibrary _levelLibrary;
    private GameTile[,] _tileMatrix;
    private int _levelHeight;
    private int _levelWidth;
    private int _baseHealth;

    
    private void Start()
    {
        EventManager.OnEnemyReadyToMove += EnemyReadyToMoveEventHandler;
        EventManager.OnDefenceItemReadyToAttack += DefenceItemReadyToAttackEventHandler;
        EventManager.OnEnemyReachedBase += EnemyReachedBaseEventHandler;
    }

    private void EnemyReachedBaseEventHandler(object sender, EventArgs args)
    {
        _baseHealth--;

        if (_baseHealth <= 0)
        {
            EventManager.LevelFailed();
        }
    }

    private void OnDestroy()
    {
        EventManager.OnEnemyReadyToMove -= EnemyReadyToMoveEventHandler;
        EventManager.OnDefenceItemReadyToAttack -= DefenceItemReadyToAttackEventHandler;
        EventManager.OnEnemyReachedBase -= EnemyReachedBaseEventHandler;
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

        var levelIndex = PlayerPrefs.GetInt("levelIndex", defaultValue: 0);
        
        LoadLevelByIndex(levelIndex);
    }
    
    private void LoadLevelByIndex(int index)
    {
        if(index < 0 || index >= _levelLibrary.levelList.Count)
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

                tile.transform.localPosition = new Vector3(2 * i, 2 * j, 0);
                tile.xIndex = i;
                tile.yIndex = j;

                _tileMatrix[i, j] = tile;

                if (j == levelFormat.height - 1)
                {
                    tile.SetSpawnPoint(new List<Constants.EnemyType>{ Constants.EnemyType.Enemy1 });
                }
            }
        }

        foreach (var enemy2Count in levelFormat.enemyConfiguration)
        {
            _baseHealth += enemy2Count.count;
        }

        _baseHealth /= 2;
        
        EventManager.LevelStarted(levelFormat);
    }

    private bool IsIndexValid(int x, int y)
    {
        return 0 <= x && x < _tileMatrix.GetLength(0) && 0 <= y && y < _tileMatrix.GetLength(1);
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
        var range = defenceItem.range;
        var (x, y) = (defenceItem.xIndex, defenceItem.yIndex);

        for (int offset = 0; offset <= range; offset++)
        {
            if (!IsIndexValid(x, y + offset))
            {
                continue;
            }

            var presentEnemy = _tileMatrix[x, y + offset].presentEnemy;
            if (presentEnemy != null)
            {
                defenceItem.Attack(presentEnemy);
            }
        }
    }
}
