using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private GameTile gameTilePrefab;
    
    
    private LevelLibrary _levelLibrary;
    private GameTile[,] _tileMatrix;
    private int _levelHeight;
    
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
            }
        }
        
        EventManager.LevelStarted(levelFormat);
    }
    
    
}
