using System;
using UnityEngine;
using static Constants;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private Tile tilePrefab;
    
    
    private LevelLibrary _levelLibrary;

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
        // -3 -5 -> +2 +2

        for (int j = 0; j < levelFormat.height; j++)
        {
            for (int i = 0; i < levelFormat.width; i++)
            {
                var tile = Instantiate(
                    tilePrefab, new Vector3(-3 + 2 * i, -7 + 2 * j, 0), Quaternion.identity, tileParent);

                tile.xIndex = i;
                tile.yIndex = j;
            }
        }
    }
}
