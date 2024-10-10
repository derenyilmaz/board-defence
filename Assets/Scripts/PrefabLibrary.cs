using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Constants;

[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "ScriptableObjects/PrefabLibrary", order = 1)]
public class PrefabLibrary : ScriptableObject
{
    public List<DefenceItemType2Prefab> defenceItemType2PrefabList = new();
    public List<EnemyType2Prefab> enemyType2PrefabList = new();


    private readonly Dictionary<DefenceItemType, DefenceItem> _defenceItemType2PrefabDict = new();
    private readonly Dictionary<EnemyType, Enemy> _enemyType2PrefabDict = new();

    
    private void Awake()
    {
        foreach (var defenceItemType2Prefab in defenceItemType2PrefabList)
        {
            _defenceItemType2PrefabDict.Add(defenceItemType2Prefab.defenceItemType, defenceItemType2Prefab.defenceItemPrefab);
        }
        
        foreach (var enemyType2Prefab in enemyType2PrefabList)
        {
            _enemyType2PrefabDict.Add(enemyType2Prefab.enemyType, enemyType2Prefab.enemyPrefab);
        }
    }

    public DefenceItem GetDefenceItemPrefabFromType(DefenceItemType gameItemType)
    {
        if (_defenceItemType2PrefabDict.Count == 0)
        {
            Awake();
        }
        
        if (!_defenceItemType2PrefabDict.ContainsKey(gameItemType))
        {
            return null;
        }
        
        return _defenceItemType2PrefabDict[gameItemType];
    }
    
    public Enemy GetEnemyPrefabFromType(EnemyType enemyType)
    {
        if (_enemyType2PrefabDict.Count == 0)
        {
            Awake();
        }
        
        if (!_enemyType2PrefabDict.ContainsKey(enemyType))
        {
            return null;
        }
        
        return _enemyType2PrefabDict[enemyType];
    }
}

[Serializable]
public class DefenceItemType2Prefab
{
    public DefenceItemType defenceItemType;
    public DefenceItem defenceItemPrefab;
}

[Serializable]
public class EnemyType2Prefab
{
    public EnemyType enemyType;
    public Enemy enemyPrefab;
}
