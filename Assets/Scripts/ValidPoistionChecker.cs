using System.Collections.Generic;
using UnityEngine;


public class ValidPositionChecker
{
    private GameObject player;
    private List<AbstractSpawner> spawnerScripts;
    private WallSpawner wallSpawner;

    public ValidPositionChecker()
    {
        this.player = GameObject.Find("Player");
        InitializeSpawnerScripts();
    }

    private void InitializeSpawnerScripts()
    {
        GameObject spawnersHolder = GameObject.Find("SpawnersHolder");
        if(spawnersHolder == null)
        {
            Debug.LogWarning("SpawnersHolder does not exist");
        }
        spawnerScripts = new List<AbstractSpawner>();
        spawnerScripts.Add(spawnersHolder.GetComponent<EnemySpawner>());
        spawnerScripts.Add(spawnersHolder.GetComponent<DieSpawner>());
        spawnerScripts.Add(spawnersHolder.GetComponent<WeaponItemSpawner>());

        
        wallSpawner = spawnersHolder.GetComponent<WallSpawner>();
    }

    public bool IsInMapBoundaries(Vector3 position)
    {
        if(position.x < ConstUtils.LEFT_MAP_X || position.x > ConstUtils.RIGHT_MAP_X
            || position.y < ConstUtils.BOTTOM_MAP_Y || position.y > ConstUtils.TOP_MAP_Y)
            return false;
        return true;
    }
    
    public bool IsWantedPositionOutOfBoundsOrWall(Vector3 position)
    {
        if(IsInMapBoundaries(position) && !wallSpawner.DoesPositionHasWall(position))
            return true;
        return false;
    }

    public bool IsMovementPositionLegal(Vector3 position)
    {
        if(IsInMapBoundaries(position) 
            && !wallSpawner.DoesPositionHasWall(position)
            && !IsSameAsPrefabPosition(position, (int)ConstUtils.SpawnerScriptsIndex.ENEMY_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)ConstUtils.SpawnerScriptsIndex.DIE_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)ConstUtils.SpawnerScriptsIndex.WEAPON_ITEM_SPAWNER))
            return true;
        return false;
    }

    public bool IsSpawningPositionLegal(Vector3 position)
    {
        if(IsInMapBoundaries(position) 
            && !wallSpawner.DoesPositionHasWall(position)
            && !IsSameAsPlayerPosition(position)
            && !IsSameAsPrefabPosition(position, (int)ConstUtils.SpawnerScriptsIndex.ENEMY_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)ConstUtils.SpawnerScriptsIndex.DIE_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)ConstUtils.SpawnerScriptsIndex.WEAPON_ITEM_SPAWNER))
            return true;
        return false;
    }

    private bool IsSameAsPrefabPosition(Vector3 pos, int prefabScriptIndex)
    {
        if(spawnerScripts[prefabScriptIndex].enabled == false)
        {
            Debug.LogWarning("Spawner script is disabled");
            return false;
        }
        List<GameObject> prefabInMap = spawnerScripts[prefabScriptIndex].GetPrefabsOnMap();
        foreach(GameObject prefab in prefabInMap)
        {
            Debug.Assert(prefab != null);
            if(prefab == null)
                Debug.Log(prefabScriptIndex);
            if(prefab.transform.position.x == pos.x && prefab.transform.position.y == pos.y)
                return true;
        }
        return false;
    }

    private bool IsSameAsPlayerPosition(Vector3 pos)
    {
        if(this.player == null) //TODO: !! Check how to check if a script is null or inactive
        {
            Debug.LogWarning("Player object does not exist");
            return false;
        }

        Vector3 playerPosition = this.player.transform.position;
        if(playerPosition.x == pos.x && playerPosition.y == pos.y)
            return true;

        return false;
    }
}