using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public delegate void OnGunFires();
    public static OnGunFires onGunFires;

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
        muzzle = GameObject.Find("Player/WeaponHolder");
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
            _tempTimeBetweenAttacks = _timeBtweenAttacks;
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity);
        bullet.GetComponent<BasicBullet>().SetBulletDirection(muzzle.gameObject.transform.eulerAngles);
        onGunFires?.Invoke();
    }

    private void IncrementNumberOfShotsFired()
    {
        _numOfShotsFired++;
    }

    
}
