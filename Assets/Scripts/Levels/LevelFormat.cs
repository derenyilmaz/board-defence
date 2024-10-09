using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/New Level", order = 1)]
public class LevelFormat : ScriptableObject
{
    public int width;
    public int height;

    public List<DefenceItem2Count> startingDefenceItemConfiguration;
    public List<Enemy2Count> enemyConfiguration;
    
}

[System.Serializable]
public class DefenceItem2Count
{
    public DefenceItemType defenceItemType;
    public int count;
}

[System.Serializable]
public class Enemy2Count
{
    public EnemyType enemyType;
    public int count;
}
