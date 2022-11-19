using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : AbstractSpawnManager
{
    [SerializeField] private float _timeBetweenSpawns = 5f;
    [SerializeField] private float[] _spawningProbabilityBetweenMonsters; //Has to sum up to 1

    public delegate void OnEnemyDeathUpdateUI();
    public static OnEnemyDeathUpdateUI onEnemyDeathUpdateUI;

    private int _totalEnemiesKilled = 0;
    
    private float _lastSpawnTime = 0;
    

    public int GetTotalNumOfEnemiesKilled()
    {
        return _totalEnemiesKilled;
    }

    protected override void SpecificInitializations()
    {
        EnemyController.onEnemyDeath += EnemyDied;
    }

    private void EnemyDied(GameObject enemy)
    {
        _totalEnemiesKilled++;
        RemoveFromPrefabsOnMap(enemy);
        Destroy(enemy);
        onEnemyDeathUpdateUI?.Invoke();
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if(Time.time - _lastSpawnTime > _timeBetweenSpawns)
        {
            _lastSpawnTime = Time.time;
            return true;
        }
        return false;
    }
    
    protected override void Spawn(Vector3 spawningPosition)
    {
        GameObject enemy = Instantiate(prefabsToSpawn[ChooseWhichToSpawn()],
                                         spawningPosition, Quaternion.identity);
        AddToPrefabsOnMap(enemy);
    }

    protected override int ChooseWhichToSpawn()
    {
        float rand = (float)Random.Range(0, 101) / 100;
        return IndexFactoryForSpawningMonster(rand);
    }

    private int IndexFactoryForSpawningMonster(float rand)
    {
        float fromValue = 0;
        float untilValue = 0;
        for(int i = 0; i < _spawningProbabilityBetweenMonsters.Length; i++)
        {
            untilValue += _spawningProbabilityBetweenMonsters[i];
            if(rand >= fromValue && rand < untilValue)
            {
                return i;
            }
            fromValue += _spawningProbabilityBetweenMonsters[i];
        }
        return _spawningProbabilityBetweenMonsters.Length - 1;
    }
}
