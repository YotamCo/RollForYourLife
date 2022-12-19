using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EnemySpawnerRateCalculator;

public class EnemySpawner : AbstractSpawnManager
{
    [SerializeField] private float _initialTimeBetweenSpawns = 5f;
    [SerializeField] private GameObject _preEnemySpawnAnimationPrefab;
    private float _timeBetweenSpawns;
    [SerializeField] private float[] _spawningProbabilityBetweenMonsters; //Has to sum up to 1
    [SerializeField] private int _maxNumOfEnemiesOnMap = 5;

    private int _numOfEnemiesOnMap = 0;

    public delegate void OnEnemyDeathUpdateUI();
    public static OnEnemyDeathUpdateUI onEnemyDeathUpdateUI;

    public delegate void OnEnemyDied(int totalEnemiesKilled);
    public static OnEnemyDied onEnemyDied;

    private int _totalEnemiesKilled = 0;
    
    private float _lastSpawnTime = 0;

    //private EnemySpawnerRateCalculator _enemySpawnerRateCalculator;
    private LevelTransitionManager _levelTransitionManagerScript;
    

    public int GetTotalNumOfEnemiesKilled()
    {
        return _totalEnemiesKilled;
    }

    protected override void SpecificInitializations()
    {
        EnemyController.onEnemyDeath += DestroyPrefab;
        //LevelTransitionManager.onUpdatingTargetScoreWhenLevelUp += InitSpawnRateCalculator(int newTargetScore);
        _timeBetweenSpawns = _initialTimeBetweenSpawns;
        
        //_enemySpawnerRateCalculator = new EnemySpawnerRateCalculator();
        //_enemySpawnerRateCalculator.Init(_timeBetweenSpawns, )
    }

    private void InitSpawnRateCalculator(int newTargetScore)
    {

    }

    protected override void DestroyPrefab(GameObject enemy)
    {
        _numOfEnemiesOnMap--;
        _totalEnemiesKilled++;
        RemoveFromPrefabsOnMap(enemy);
        Destroy(enemy);
        //enemy.GetComponent<EnemyController>().KillEnemyOnMapClear();
        onEnemyDeathUpdateUI?.Invoke();
        onEnemyDied?.Invoke(GetTotalNumOfEnemiesKilled());
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
        GameObject preEnemySpawnAnimation = Instantiate(_preEnemySpawnAnimationPrefab, 
                                                    spawningPosition, Quaternion.identity);
        Destroy(preEnemySpawnAnimation, 2f);
        GameObject enemy = Instantiate(prefabsToSpawn[ChooseWhichToSpawn()],
                                         spawningPosition, Quaternion.identity);
        AddToPrefabsOnMap(enemy);
        enemy.SetActive(false);
        _numOfEnemiesOnMap++;
        StartCoroutine(SpawnInDelay(spawningPosition, enemy));
    }

    IEnumerator SpawnInDelay(Vector3 spawningPosition, GameObject enemy)
    {
        yield return new WaitForSeconds(2f);
        if(enemy != null) //Enemy might have been destroyed when passing to next level
        {
            enemy.SetActive(true);
        }
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

    private void SetTimeBetweenSpawns(float timeBetweenSpawns)
    {
        _timeBetweenSpawns = timeBetweenSpawns;
    }
}
