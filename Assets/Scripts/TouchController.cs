using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    private CursorFollowerImage _cursorFollowerImage;
    private Constants.DefenceItemType _selectedDefenceItemType = Constants.DefenceItemType.None;
    private PlacementTile _lastSelectedPlacementTile;
    private bool _touchEnabled = true;
    private int _levelHeight;

    private PrefabLibrary _prefabLibrary;
    
    
    private void Awake()
    {
        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");

        _cursorFollowerImage = GetComponentInChildren<CursorFollowerImage>();
        
        EventManager.OnLevelStarted += LevelStartedEventHandler;
        EventManager.OnLevelFailed += LevelFailedEventHandler;
        EventManager.OnLevelWon += LevelWonEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnLevelStarted -= LevelStartedEventHandler;
        EventManager.OnLevelFailed -= LevelFailedEventHandler;
        EventManager.OnLevelWon -= LevelWonEventHandler;
    }
    

    private void Update()
    {
        if (!_touchEnabled)
        {
            _cursorFollowerImage.Hide();
            return;
        }

        if (Camera.main == null)
        {
            return;
        }
        
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _cursorFollowerImage.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        if (Input.GetMouseButtonDown(0))
        {
            var raycastHit = Physics2D.Raycast(
                origin: mousePosition,
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

            var sprite = _prefabLibrary.GetDefenceItemPrefabFromType(
                    _selectedDefenceItemType).GetComponentInChildren<Image>().sprite; 
            
            _cursorFollowerImage.SetSpriteAndShow(sprite);
        }
    }
    
    private void HandleGameTileTouch(GameTile tappedGameTile)
    {
        if (tappedGameTile.presentEnemy != null || tappedGameTile.presentDefenceItem != null || 
            tappedGameTile.yIndex >= _levelHeight / 2)
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

            _cursorFollowerImage.Hide();
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
    
    private void LevelStartedEventHandler(object sender, EventManager.LevelStartedEventArgs args)
    {
        _levelHeight = args.LevelFormat.height;
    }
}

[Serializable]
public class DefenceItemType2Sprite
{
    public Constants.DefenceItemType defenceItemType;
    public Sprite sprite;
}