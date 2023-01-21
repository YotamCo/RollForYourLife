using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] prefabsToSpawn;
    private ValidPositionChecker validPositionChecker;
    protected bool shouldSpawnPrefab = false;
    protected List<GameObject> prefabsOnMap;
    protected bool spawningEnabled = true;

    private void Start()
    {
        validPositionChecker = new ValidPositionChecker();
        prefabsOnMap = new List<GameObject>();

        SpecificInitializations();
    }

    private void Update()
    {
        if(spawningEnabled)
        {
            if(shouldSpawnPrefab || ShouldSpawnPrefab())
            {
                Vector3 spawnPosition = ChooseSpawningPosition();
                if(IsSpawningPositionLegal(spawnPosition))
                {
                    Spawn(spawnPosition);
                    shouldSpawnPrefab = false;
                }
            }
        }
    }

    private bool IsSpawningPositionLegal(Vector3 pos)
    {
        return validPositionChecker.IsSpawningPositionLegal(pos);
    }

    protected void AddToPrefabsOnMapList(GameObject spawnedPrefab)
    {
        prefabsOnMap.Add(spawnedPrefab);
    }

    protected void RemoveFromPrefabsOnMapList(GameObject removedPrefab)
    {
        prefabsOnMap.Remove(removedPrefab);
    }

    public List<GameObject> GetPrefabsOnMap()
    {
        return prefabsOnMap;
    }

    private Vector3 ChooseSpawningPosition()
    {
        int xPos = Random.Range(ConstUtils.LEFT_MAP_X, ConstUtils.RIGHT_MAP_X + 1);
        int yPos = Random.Range(ConstUtils.BOTTOM_MAP_Y, ConstUtils.TOP_MAP_Y + 1);
        return new Vector3(xPos, yPos, 0);
    }

    protected bool ShouldSpawnPrefab()
    {
        if(SpecificShouldSpawnPrefab())
        {
            shouldSpawnPrefab = true;
            return true;
        }
        return false;
    }

    public void ClearPrefabsOnMap()
    {
        for(int i = prefabsOnMap.Count - 1; i >= 0; i--)
        {
            GameObject prefabToDestroy = prefabsOnMap[i];
            DestroyPrefab(prefabToDestroy);
        }
    }

    public void ChangeSpawningStatus(bool isSetToSpawning)
    {
        spawningEnabled = isSetToSpawning;
    }

    protected abstract void SpecificInitializations();
    protected abstract bool SpecificShouldSpawnPrefab(); 
    protected abstract int ChooseWhichToSpawn();
    protected abstract void Spawn(Vector3 spawningPosition);
    protected abstract void DestroyPrefab(GameObject prefabObject);
}
