using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemSpawner : AbstractSpawnManager
{
    [SerializeField] private int _numOfEnemiesToKillForSpawningWeapon = 10;

    private int _totalEnemiesKilled = 0;
    private int _lastSpawnedTotalEnemiesKilled = 0;


    public int GetNumOfEnemiesToKillForSpawningWeapon()
    {
        return _numOfEnemiesToKillForSpawningWeapon;
    }

    public void SetNumOfEnemiesToKillForSpawningWeapon(int numOfEnemiesToKillForSpawningWeapon)
    {
        _numOfEnemiesToKillForSpawningWeapon = numOfEnemiesToKillForSpawningWeapon;
    }

    protected override void SpecificInitializations()
    {
        WeaponManager.onEquipedWeapon += DestroyPrefab;
        EnemySpawner.onEnemyDied += UpdateNumOfEnemiesKilled;
    }

    private void UpdateNumOfEnemiesKilled(int totalEnemiesKilled)
    {
        _totalEnemiesKilled = totalEnemiesKilled;
    }

    protected override bool SpecificShouldSpawnPrefab()
    {
        if(_totalEnemiesKilled - _lastSpawnedTotalEnemiesKilled >= _numOfEnemiesToKillForSpawningWeapon)
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
        RemoveFromPrefabsOnMap(weaponItem);
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
        AddToPrefabsOnMap(weaponItem);

        _lastSpawnedTotalEnemiesKilled = _totalEnemiesKilled;
    }
}
