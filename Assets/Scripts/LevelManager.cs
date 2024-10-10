using System;
using UnityEngine;
using UnityEngine.Serialization;
using static Constants;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private GameTile gameTilePrefab;
    
    
    private LevelLibrary _levelLibrary;
    private GameTile[,] _tileMatrix;
    
    
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
        // -3 -7 -> +2 +2

        _tileMatrix = new GameTile[levelFormat.width, levelFormat.height];
        
        for (int j = 0; j < levelFormat.height; j++)
        {
            for (int i = 0; i < levelFormat.width; i++)
            {
                var tile = Instantiate(
                    gameTilePrefab, new Vector3(-3 + 2 * i, -5 + 2 * j, 0), Quaternion.identity, tileParent);

                tile.xIndex = i;
                tile.yIndex = j;

                _tileMatrix[i, j] = tile;
            }
        }
        
        EventManager.LevelStarted(levelFormat);
    }
    
    
}
