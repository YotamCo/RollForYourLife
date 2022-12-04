using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemSpawner : AbstractSpawnManager
{
    private bool _shouldSpawnWeaponItem = false;

    protected override void SpecificInitializations()
    {
        DiceRollManager.onRollingWeaponItemSpawn += TimeToSpawnWeaponItem;
        WeaponManager.onEquipedWeapon += DestroyPrefab;
    }

    private void TimeToSpawnWeaponItem()
    {
        _shouldSpawnWeaponItem = true;
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if(_shouldSpawnWeaponItem)
        {
            return true;
        }
        return false;
    }

    protected override int ChooseWhichToSpawn()
    {
        //TODO: Maybe add percentage option to choose like in EnemySpawner
        float rand = (float)Random.Range(0, 101) / 100;
        return IndexFactoryForSpawningMonster(rand);
    }

    protected override void DestroyPrefab(GameObject weaponItem)
    {
        //TODO: add visuals when picking weaponItem
        RemoveFromPrefabsOnMap(weaponItem);
        Destroy(weaponItem);
    }

    private int IndexFactoryForSpawningMonster(float rand)
    {
        float fromValue = 0;
        float untilValue = 0;
        for(int i = 0; i < prefabsToSpawn.Length; i++)
        {
            untilValue += (float)((float)(100 / prefabsToSpawn.Length) / 100);
            Debug.Log("rand = " + rand);
            Debug.Log("fromValue = " + fromValue);
            Debug.Log("untilValue = " + untilValue);
            if(rand >= fromValue && rand < untilValue)
            {
                return i;
            }
            fromValue += (float)((float)(100 / prefabsToSpawn.Length) / 100);
        }
        return prefabsToSpawn.Length - 1;
    }

    protected override void Spawn(Vector3 spawningPosition)
    {
        GameObject weaponItem = Instantiate(prefabsToSpawn[ChooseWhichToSpawn()],
                                         spawningPosition, Quaternion.identity);
        AddToPrefabsOnMap(weaponItem);

        _shouldSpawnWeaponItem = false;
    }
}
