using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    private List<AbstractSpawner> spawnerScripts;


    private void Start()
    {
        GameObject spawnersHolder = GameObject.Find("SpawnersHolder");
        
        spawnerScripts = new List<AbstractSpawner>();
        spawnerScripts.Add(spawnersHolder.GetComponent<EnemySpawner>());
        spawnerScripts.Add(spawnersHolder.GetComponent<DieSpawner>());
        spawnerScripts.Add(spawnersHolder.GetComponent<WeaponItemSpawner>());
    }

    public void ChangeSpawningStatus(bool spawningStatus)
    {
        foreach(AbstractSpawner spawnerScript in spawnerScripts)
        {
            spawnerScript.ChangeSpawningStatus(spawningStatus);
        }
    }

    public void ClearPrefabsFromMap()
    {
        foreach(AbstractSpawner spawnerScript in spawnerScripts)
        {
            spawnerScript.ClearPrefabsOnMap();
        }
    }
}
