using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemSpawner : AbstractSpawner
{
    [SerializeField] private int numOfEnemiesToKillForSpawningWeapon = 10;
    private int totalEnemiesKilled = 0;
    private int lastSpawnedTotalEnemiesKilled = 0;


    public int GetNumOfEnemiesToKillForSpawningWeapon()
    {
        return numOfEnemiesToKillForSpawningWeapon;
    }

    public void SetNumOfEnemiesToKillForSpawningWeapon(int numOfEnemiesToKillForSpawningWeapon)
    {
        this.numOfEnemiesToKillForSpawningWeapon = numOfEnemiesToKillForSpawningWeapon;
    }

    protected override void SpecificInitializations()
    {
        WeaponManager.onEquipedWeapon += DestroyPrefab;
        EnemySpawner.onEnemyDied += UpdateNumOfEnemiesKilled;
    }

    private void UpdateNumOfEnemiesKilled(int totalEnemiesKilled)
    {
        this.totalEnemiesKilled = totalEnemiesKilled;
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if(totalEnemiesKilled - lastSpawnedTotalEnemiesKilled >= numOfEnemiesToKillForSpawningWeapon)
        {
            return true;
        }
        return false;
    }

    protected override int ChooseWhichToSpawn()
    {
        //TODO: Maybe add percentage option to choose like in EnemySpawner
        float rand = (float)Random.Range(0, 101) / 100;
        return IndexFactoryForSpawningWeaponItem(rand);
    }

    protected override void DestroyPrefab(GameObject weaponItem)
    {
        //TODO: add visuals when picking weaponItem
        RemoveFromPrefabsOnMapList(weaponItem);
        Destroy(weaponItem);
    }

    private int IndexFactoryForSpawningWeaponItem(float rand)
    {
        float fromValue = 0;
        float untilValue = 0;
        for(int i = 0; i < prefabsToSpawn.Length; i++)
        {
            untilValue += (float)((float)(100 / prefabsToSpawn.Length) / 100);
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
        AddToPrefabsOnMapList(weaponItem);

        lastSpawnedTotalEnemiesKilled = totalEnemiesKilled;
    }
}
