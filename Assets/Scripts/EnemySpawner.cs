using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : AbstractSpawnManager
{
    [SerializeField] private float _timeBetweenSpawns = 5f;
    [SerializeField] private float[] _spawningProbabilityBetweenMonsters; //Has to sum up to 1
    [SerializeField] private int _maxNumOfEnemiesOnMap = 5;

    private int _numOfEnemiesOnMap = 0;

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
        EnemyController.onEnemyDeath += DestroyPrefab;
    }

    protected override void DestroyPrefab(GameObject enemy)
    {
        _numOfEnemiesOnMap--;
        _totalEnemiesKilled++;
        RemoveFromPrefabsOnMap(enemy);
        Destroy(enemy);
        //enemy.GetComponent<EnemyController>().KillEnemyOnMapClear();
        onEnemyDeathUpdateUI?.Invoke();
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if((Time.time - _lastSpawnTime > _timeBetweenSpawns) && (_numOfEnemiesOnMap < _maxNumOfEnemiesOnMap))
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
        _numOfEnemiesOnMap++;
    }

    protected override int ChooseWhichToSpawn()
    {
        float rand = (float)Random.Range(0, 101) / 100;
        return IndexFactoryForSpawningEnemy(rand);
    }

    private int IndexFactoryForSpawningEnemy(float rand)
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
