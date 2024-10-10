using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnComponent : MonoBehaviour
{
    private PrefabLibrary _prefabLibrary;

    private Queue<Constants.EnemyType> _enemyTypeQueue;

    private void Start()
    {
        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");
    }

    public void Configure(List<Constants.EnemyType> enemyTypes)
    {
        _enemyTypeQueue = new Queue<Constants.EnemyType>(enemyTypes);
    }

    public Enemy GetNextEnemy(Transform parentTransform)
    {
        if (_enemyTypeQueue.Count == 0)
        {
            return null;
        }
        
        var nextEnemyType = _enemyTypeQueue.Dequeue();
        var enemyPrefab = _prefabLibrary.GetEnemyPrefabFromType(nextEnemyType);
        var enemy = Instantiate(enemyPrefab, parent: parentTransform);

        return enemy;
    }
}
