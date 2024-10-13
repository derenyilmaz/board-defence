using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(TouchController))]
public class Board : MonoBehaviour
{
    private LevelManager _levelManager;
    private GridLayoutGroup _layoutGroup;

    private void Awake()
    {
        _layoutGroup = GetComponentInChildren<GridLayoutGroup>();
    }

    private void Start()
    {
        EventManager.OnLevelStarted += LevelStartedEventHandler;
    
        _levelManager = GetComponent<LevelManager>();
        
        _levelManager.Configure();
    }
    
    private void OnDestroy()
    {
        EventManager.OnLevelStarted -= LevelStartedEventHandler;
    }

    private void LevelStartedEventHandler(object sender, EventManager.LevelStartedEventArgs args)
    {
        if (!_layoutGroup)
        {
            return;
        }
        
        _layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _layoutGroup.constraintCount = args.LevelFormat.height;

        var scaleFactor = Mathf.Min(6f / args.LevelFormat.height, 4f / args.LevelFormat.width);
        
        _layoutGroup.cellSize *= scaleFactor;
        _layoutGroup.spacing *= scaleFactor;
        
    }
}                                       
