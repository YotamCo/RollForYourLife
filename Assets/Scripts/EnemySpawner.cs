using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EnemySpawnerRateCalculator;

public class EnemySpawner : AbstractSpawner
{
    [SerializeField] private float initialTimeBetweenSpawns = 5f;
    [SerializeField] private GameObject preEnemySpawnAnimationPrefab;
    [SerializeField] private float[] spawningProbabilityBetweenMonsters; //Has to sum up to 1
    [SerializeField] private int maxNumOfEnemiesOnMap = 5;
    [SerializeField] private float preEnemySpawnEffectDuration = 2f;

    public delegate void OnEnemyDeathUpdateUI();
    public static OnEnemyDeathUpdateUI onEnemyDeathUpdateUI;

    public delegate void OnEnemyDied(int totalEnemiesKilled);
    public static OnEnemyDied onEnemyDied;

    private float timeBetweenSpawns;
    private float lastSpawnTime = 0;
    private int totalEnemiesKilled = 0;
    private int numOfEnemiesOnMap = 0;
    private LevelTransitionManager levelTransitionManagerScript;
    

    public int GetTotalNumOfEnemiesKilled()
    {
        return totalEnemiesKilled;
    }

    protected override void SpecificInitializations()
    {
        AbstractEnemy.onEnemyDeath += DestroyPrefab;
        //LevelTransitionManager.onUpdatingTargetScoreWhenLevelUp += InitSpawnRateCalculator(int newTargetScore);
        timeBetweenSpawns = initialTimeBetweenSpawns;
        
        //TODO - Refactor
        // add a calculator for rate of enemies spawned. Maybe also add an Enemy wave spawner

        //_enemySpawnerRateCalculator = new EnemySpawnerRateCalculator();
        //_enemySpawnerRateCalculator.Init(_timeBetweenSpawns, )
    }

    private void InitSpawnRateCalculator(int newTargetScore)
    {

    }

    protected override void DestroyPrefab(GameObject enemy)
    {
        numOfEnemiesOnMap--;
        totalEnemiesKilled++;
        RemoveFromPrefabsOnMapList(enemy);
        Destroy(enemy);
        onEnemyDeathUpdateUI?.Invoke();
        onEnemyDied?.Invoke(GetTotalNumOfEnemiesKilled());
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if((Time.time - lastSpawnTime > timeBetweenSpawns) 
            && (numOfEnemiesOnMap < maxNumOfEnemiesOnMap))
        {
            lastSpawnTime = Time.time;
            return true;
        }
        return false;
    }
    
    protected override void Spawn(Vector3 spawningPosition)
    {
        SpawningPreEnemySpawnEffect(spawningPosition);
        GameObject enemy = Instantiate(prefabsToSpawn[ChooseWhichToSpawn()],
                                         spawningPosition, Quaternion.identity);
        AddToPrefabsOnMapList(enemy);
        enemy.SetActive(false);
        numOfEnemiesOnMap++;
        StartCoroutine(SpawnInDelay(spawningPosition, enemy));
    }

    private void SpawningPreEnemySpawnEffect(Vector3 spawningPosition)
    {
        GameObject preEnemySpawnAnimation = Instantiate(preEnemySpawnAnimationPrefab, 
                                                    spawningPosition, Quaternion.identity);
        Destroy(preEnemySpawnAnimation, preEnemySpawnEffectDuration);
    }

    IEnumerator SpawnInDelay(Vector3 spawningPosition, GameObject enemy)
    {
        yield return new WaitForSeconds(preEnemySpawnEffectDuration);
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
        for(int i = 0; i < spawningProbabilityBetweenMonsters.Length; i++)
        {
            untilValue += spawningProbabilityBetweenMonsters[i];
            if(rand >= fromValue && rand < untilValue)
            {
                return i;
            }
            fromValue += spawningProbabilityBetweenMonsters[i];
        }
        return spawningProbabilityBetweenMonsters.Length - 1;
    }

    private void SetTimeBetweenSpawns(float timeBetweenSpawns)
    {
        this.timeBetweenSpawns = timeBetweenSpawns;
    }
}
