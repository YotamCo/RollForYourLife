using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    enum WeaponEnum
    {
        FIST = 0,
        SWORD = 1,
        GUN = 2
    }

    [SerializeField] private List<GameObject> _weaponPrefabs; // fist, sword, gun
    //private List<Weapon> _weaponScripts;
    private List<GameObject> _weaponsArt;
    private Transform _weaponHolder;
    private int _currentWeaponIndex;
    private GameObject _currentWeapon;


    // Start is called before the first frame update
    void Start()
    {
        WeaponItem.onWeaponItemTaken += WeaponItemPickedUp;
        //_weaponScripts = new List<Weapon>();
        _weaponHolder = GameObject.Find("Player/WeaponHolder").transform; //gameObject.transform.GetChild(1);//TODO: needs to add the weapon holder from player. GetChild or something

        _currentWeaponIndex = (int)WeaponEnum.FIST;
        InitWeaponsArt();
        InstantiateWeapon(_currentWeaponIndex);
    }

    private void InitWeaponsArt()
    {
        _weaponsArt = new List<GameObject>();
        _weaponsArt.Add(GameObject.Find("Player/WeaponsHolding/FistArt"));
        _weaponsArt.Add(GameObject.Find("Player/WeaponsHolding/SwordArt"));
        _weaponsArt.Add(GameObject.Find("Player/WeaponsHolding/GunArt"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            _currentWeapon.GetComponent<Weapon>().Attack();
            if(_currentWeapon.GetComponent<Weapon>().GetWeaponName() == "Gun")
            {
                //probably will trigger an event from here to WeaponsUI OR through the gun attack method
            }
        }

        if(_currentWeapon.GetComponent<Weapon>().IsTimeToDropWeapon())
        {
            Debug.Log("Should drop weapon now!");
            ChangeWeapon((int)WeaponEnum.FIST);
        }
    }

    void WeaponDropped(GameObject weaponItem)
    {

    }

    void WeaponPickedUp(string weaponName)
    {
        
    }

    private void WeaponItemPickedUp(GameObject weaponItem)
    {
        string weaponItemName;
        weaponItemName = weaponItem.name;
        ChangeWeapon(WeaponIndexFactory(weaponItemName));
    }

    private void ChangeWeapon(int weaponIndexToChange)
    {
        //_weaponScripts[_currentWeaponIndex].DestroyWeapon();
        Destroy(_currentWeapon);
        _weaponsArt[_currentWeaponIndex].GetComponent<SpriteRenderer>().enabled = false;
        InstantiateWeapon(weaponIndexToChange);

        _currentWeaponIndex = weaponIndexToChange;
        // trigger an event that WeaponsUI will catch to match the UI

    }

    private int WeaponIndexFactory(string weaponName)
    {
        if(weaponName == "SwordItem")
        {
            Debug.Log("Picked up a sword");
            return (int)WeaponEnum.SWORD;
        }
        if(weaponName == "GunItem")
        {
            return (int)WeaponEnum.GUN;
        }
        return (int)WeaponEnum.FIST; //Should not get here
    }

     private void InstantiateWeapon(int weaponIndex)
    {
        _currentWeapon = Instantiate(_weaponPrefabs[weaponIndex], _weaponHolder.position, Quaternion.identity);
        _currentWeapon.transform.eulerAngles = gameObject.transform.eulerAngles;
        _currentWeapon.transform.parent = gameObject.transform;
        _weaponsArt[weaponIndex].GetComponent<SpriteRenderer>().enabled = true;
    }
}
