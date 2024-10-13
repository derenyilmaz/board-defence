
using System;
using TMPro;
using UnityEngine;
using static Constants;

public class PlacementTile : MonoBehaviour
{
    [SerializeField] private TextMeshPro countText;
    
    
    public DefenceItemType defenceItemType;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            countText.text = value.ToString();
            
            if (value <= 0)
            {
                Deactivate();
            }
        }
    }

    private int _count;

    private void Awake()
    {
        EventManager.OnLevelStarted += LevelStartedEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnLevelStarted -= LevelStartedEventHandler;
    }

    private void LevelStartedEventHandler(object sender, EventManager.LevelStartedEventArgs args)
    {
        foreach (var defenceItem2Count in args.LevelFormat.startingDefenceItemConfiguration)
        {
            if (defenceItem2Count.defenceItemType != defenceItemType)
            {
                continue;
            }

            Count = defenceItem2Count.count;
        }
    }

    private void Deactivate()
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!spriteRenderer)
        {
            return;
        }
        
        spriteRenderer.color = Color.grey;
    }
}
