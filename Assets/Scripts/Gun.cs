using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    
    [SerializeField] int _numOfShotsBeforeDrops = 3;
    private int _numOfShotsFired = 0;
    private GameObject muzzle;

    public int GetNumOfShotsBeforeDrops()
    {
        return _numOfShotsBeforeDrops;
    }

    protected override void SpecificWeaponInitializations()
    {
        muzzle = GameObject.Find("Player/WeaponHolder"); //TODO: maybe change to another game object
    }

    public override bool IsTimeToDropWeapon()
    {
        if(_numOfShotsFired >= _numOfShotsBeforeDrops)
        {
            return true;
        }
        return false;
    }

    public override string GetWeaponName()
    {
        return "Gun";
    }

    public override void Attack()
    {
        if (_tempTimeBetweenAttacks < 0)
        {
            ShootBullet();
            IncrementNumberOfShotsFired();
            //TODO: trigger event to WeaponsUI 
            _tempTimeBetweenAttacks = _timeBtweenAttacks;
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity);
        bullet.GetComponent<BasicBullet>().SetBulletDirection(muzzle.gameObject.transform.eulerAngles);

    }

    private void IncrementNumberOfShotsFired()
    {
        _numOfShotsFired++;
    }

    
}
