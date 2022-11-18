using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : AbstractSpawnManager
{
    [SerializeField] float _timeBetweenSpawns = 5f;
    
    private float _lastSpawnTime = 0;
    

    protected override void SpecificInitializations()
    {
        
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
        addToPrefabsOnMap(enemy);
    }

    protected override int ChooseWhichToSpawn()
    {
        int randIndex = Random.Range(0, prefabsToSpawn.Length);  //TODO: add a smarter way of choosing enemies
        return randIndex;
    }
}
