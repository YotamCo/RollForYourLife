using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSpawner : AbstractSpawner
{
    [SerializeField] float timeBetweenPossibleSpawns = 10f;
    private float lastSpawnTrialTime = 0;
    [SerializeField] [Range(0, 1)] float spawningProbability = 0.3f;


    protected override void SpecificInitializations()
    {
        DiceController.onDiePickedUp += DestroyPrefab;
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if(Time.time - lastSpawnTrialTime > timeBetweenPossibleSpawns)
        {
            lastSpawnTrialTime = Time.time;
            return canSpawnBasedOnRate();
        }
        return false;
    }

    private bool canSpawnBasedOnRate()
    {
        float rand = (float)Random.Range(0, 101);
        if(rand / 100 <= spawningProbability)
        {
            return true;
        }
        return false;
    }
 
    protected override void Spawn(Vector3 spawningPosition)
    {
        GameObject die = Instantiate(prefabsToSpawn[ChooseWhichToSpawn()],
                                     spawningPosition, Quaternion.identity);
        AddToPrefabsOnMapList(die);
    }

    protected override int ChooseWhichToSpawn()
    {
        return 0;
    }

   protected override void DestroyPrefab(GameObject dieObject)
   {
        RemoveFromPrefabsOnMapList(dieObject);
        Destroy(dieObject);    
   }
}
