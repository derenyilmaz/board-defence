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

    private void Start()
    {
        EventManager.OnEnemyReadyToMove += EnemyReadyToMoveEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnEnemyReadyToMove -= EnemyReadyToMoveEventHandler;
    }

    private void EnemyReadyToMoveEventHandler(object sender, EventManager.EnemyReadyToMoveEventArgs args)
    {
        var (x, y) = (args.Enemy.xIndex, args.Enemy.yIndex);
        if (y == 0)
        {
            return;
        }

        args.Enemy.yIndex = y - 1;
        _tileMatrix[x, y].presentEnemy = null;
        _tileMatrix[x, y - 1].presentEnemy = args.Enemy;
        
        args.Enemy.MoveToTile(_tileMatrix[x, y - 1]);
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
                    tile.SetSpawnPoint(new List<Constants.EnemyType>{ Constants.EnemyType.Enemy1, Constants.EnemyType.Enemy2, Constants.EnemyType.Enemy3});
                }
            }
        }
        
        EventManager.LevelStarted(levelFormat);
    }
    
    
}
