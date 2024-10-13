using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class GameTile : MonoBehaviour
{
    private const float EnemySpawnDelayInSeconds = 3f;
    
    
    public int xIndex;
    public int yIndex;

    public DefenceItem presentDefenceItem;
    public Enemy presentEnemy;


    private EnemySpawnComponent _enemySpawnComponent;
    private float _timeElapsedSinceLastSpawn;
    
    private void FixedUpdate()
    {
        if (!_enemySpawnComponent)
        {
            return;
        }

        _timeElapsedSinceLastSpawn += Time.fixedDeltaTime;

        if (_timeElapsedSinceLastSpawn >= EnemySpawnDelayInSeconds)
        {
            SpawnEnemy();
        }
    }
    
    public void SetSpawnPoint(List<EnemyType> enemyTypes)
    {
        _enemySpawnComponent = gameObject.AddComponent<EnemySpawnComponent>();
        _enemySpawnComponent.Configure(enemyTypes);
    }

    private void SpawnEnemy()
    {
        var nextEnemy = _enemySpawnComponent.GetNextEnemy(transform);

        if (nextEnemy == null)
        {
            return;
        }

        presentEnemy = nextEnemy;
        presentEnemy.xIndex = xIndex;
        presentEnemy.yIndex = yIndex;
    }
}
