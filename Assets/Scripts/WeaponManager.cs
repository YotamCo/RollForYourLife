using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponEnum
    {
        FIST = 0,
        SWORD = 1,
        GUN = 2
    }
public class WeaponManager : MonoBehaviour
{
    public delegate void OnChangeWeapon(int newWeaponIndex);
    public static OnChangeWeapon onChangeWeapon;

    public delegate void OnEquipedWeapon(GameObject weaponItem);
     public static OnEquipedWeapon onEquipedWeapon;

    [SerializeField] private List<GameObject> _weaponPrefabs; // fist, sword, gun
    //private List<Weapon> _weaponScripts;
    private List<GameObject> _weaponsArt;
    private Transform _weaponHolder;
    private int _currentWeaponIndex;
    private GameObject _currentInstantiatedWeapon;


    // Start is called before the first frame update
    void Start()
    {
        WeaponItem.onWeaponItemTaken += WeaponItemPickedUp;
        //_weaponScripts = new List<Weapon>();
        _weaponHolder = GameObject.Find("Player/WeaponHolder").transform;

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
            _currentInstantiatedWeapon.GetComponent<Weapon>().Attack();
            if(_currentInstantiatedWeapon.GetComponent<Weapon>().GetWeaponName() == "Gun")
            {
                //probably will trigger an event from here to WeaponsUI OR through the gun attack method
            }
        }

        if(_currentInstantiatedWeapon.GetComponent<Weapon>().IsTimeToDropWeapon())
        {
            ChangeWeapon((int)WeaponEnum.FIST);
        }
    }

    private void WeaponItemPickedUp(GameObject weaponItem)
    {
        ChangeWeapon(WeaponIndexFactory(weaponItem.tag));
        onEquipedWeapon?.Invoke(weaponItem);
    }

    private void ChangeWeapon(int weaponIndexToChange)
    {
        Destroy(_currentInstantiatedWeapon);
        _weaponsArt[_currentWeaponIndex].GetComponent<SpriteRenderer>().enabled = false;
        InstantiateWeapon(weaponIndexToChange);
        _currentWeaponIndex = weaponIndexToChange;
        onChangeWeapon?.Invoke(weaponIndexToChange);
    }

    private int WeaponIndexFactory(string weaponTag)
    {
        if(weaponTag == "SwordWeaponItem")
        {
            Debug.Log("Picked a sword");
            return (int)WeaponEnum.SWORD;
        }
        if(weaponTag == "GunWeaponItem")
        {
            Debug.Log("Picked a gun");
            return (int)WeaponEnum.GUN;
        }
        return (int)WeaponEnum.FIST; //Should not get here
    }

     private void InstantiateWeapon(int weaponIndex)
    {
        _currentInstantiatedWeapon = Instantiate(_weaponPrefabs[weaponIndex], _weaponHolder.position, Quaternion.identity);
        _currentInstantiatedWeapon.transform.eulerAngles = gameObject.transform.eulerAngles;
        _currentInstantiatedWeapon.transform.parent = gameObject.transform;
        _weaponsArt[weaponIndex].GetComponent<SpriteRenderer>().enabled = true;
    }
}
