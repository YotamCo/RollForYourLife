using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSpawner : AbstractSpawnManager
{
    [SerializeField] float _timeBetweenPossibleSpawns = 10f;
    private float _lastSpawnTrialTime = 0;
    [SerializeField] [Range(0, 1)] float _spawningProbability = 0.3f;


    protected override void SpecificInitializations()
    {
        DiceController.onDiePickedUp += DestroyPrefab;
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if(Time.time - _lastSpawnTrialTime > _timeBetweenPossibleSpawns)
        {
            _lastSpawnTrialTime = Time.time;
            return canSpawnBasedOnRate();
        }
        return false;
    }

    private bool canSpawnBasedOnRate()
    {
        float rand = (float)Random.Range(0, 101);
        if(rand / 100 <= _spawningProbability)
        {
            return true;
        }
        return false;
    }

    
    protected override void Spawn(Vector3 spawningPosition)
    {
        GameObject die = Instantiate(prefabsToSpawn[ChooseWhichToSpawn()],
                                     spawningPosition, Quaternion.identity);
        AddToPrefabsOnMap(die);
    }

    protected override int ChooseWhichToSpawn()
    {
        return 0;
    }

   protected override void DestroyPrefab(GameObject dieObject)
   {
        RemoveFromPrefabsOnMap(dieObject);
        Destroy(dieObject);    
   }
}
