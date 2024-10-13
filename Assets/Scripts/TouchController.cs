using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchController : MonoBehaviour
{
    private Constants.DefenceItemType _selectedDefenceItemType = Constants.DefenceItemType.None;
    private PlacementTile _lastSelectedPlacementTile;
    private bool _touchEnabled = true;

    private PrefabLibrary _prefabLibrary;
    
    private void Start()
    {
        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");
        
        EventManager.OnLevelFailed += LevelFailedEventHandler;
        EventManager.OnLevelWon += LevelWonEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnLevelFailed -= LevelFailedEventHandler;
        EventManager.OnLevelWon -= LevelWonEventHandler;
    }
    

    private void Update()
    {
        if (!_touchEnabled)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var raycastHit = Physics2D.Raycast(
                origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
                direction: Vector3.forward,
                distance: 100f);

            if (raycastHit.collider != null)
            {
                var tappedPlacementTile = raycastHit.collider.GetComponent<PlacementTile>();

                if (tappedPlacementTile != null)
                {
                    HandlePlacementTileTouch(tappedPlacementTile);
                    return;
                }
                
                var tappedGameTile = raycastHit.collider.GetComponent<GameTile>();

                if (tappedGameTile != null)
                {
                    HandleGameTileTouch(tappedGameTile);
                    return;
                }

                _selectedDefenceItemType = Constants.DefenceItemType.None;
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void HandlePlacementTileTouch(PlacementTile tappedPlacementTile)
    {
        if (tappedPlacementTile.Count > 0)
        {
            _selectedDefenceItemType = tappedPlacementTile.defenceItemType;
            _lastSelectedPlacementTile = tappedPlacementTile;
        }
    }
    
    private void HandleGameTileTouch(GameTile tappedGameTile)
    {
        if (tappedGameTile.presentEnemy != null || tappedGameTile.presentDefenceItem != null)
        {
            return;
        }

        if (_selectedDefenceItemType != Constants.DefenceItemType.None)
        {
            var defenceItemPrefab = _prefabLibrary.GetDefenceItemPrefabFromType(_selectedDefenceItemType);
            var defenceItem = Instantiate(defenceItemPrefab, parent: tappedGameTile.transform);

            defenceItem.xIndex = tappedGameTile.xIndex;
            defenceItem.yIndex = tappedGameTile.yIndex;
            
            tappedGameTile.presentDefenceItem = defenceItem;

            _lastSelectedPlacementTile.Count--;
            _selectedDefenceItemType = Constants.DefenceItemType.None;
        }
    }

    private void LevelFailedEventHandler(object sender, EventArgs args)
    {
        _touchEnabled = false;
    }
    
    private void LevelWonEventHandler(object sender, EventArgs args)
    {
        _touchEnabled = false;
    }
}
