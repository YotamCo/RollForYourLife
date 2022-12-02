using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    private List<Image> _weaponImages;
    private GameObject _weaponImagesParent;
    private List<GameObject> _weaponStatus;
    private GameObject _weaponStatusParent;

    private int _currentWeaponIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        WeaponManager.onChangeWeapon += UpdateWeaponStatus;
        _weaponImagesParent = GameObject.Find("WeaponUI/WeaponImages");
        Debug.Assert(_weaponImagesParent != null);
        _weaponStatusParent = GameObject.Find("WeaponUI/WeaponStatus");
        Debug.Assert(_weaponStatusParent != null);

        InitializeWeaponImages();
        InitializeWeaponStatus();
    }

    private void UpdateWeaponStatus(int newWeaponIndex)
    {
        // Removing current UI weapon status
        _weaponImages[_currentWeaponIndex].gameObject.SetActive(false);
        _weaponStatus[_currentWeaponIndex].SetActive(false);
        // Turn on the new UI weapon status
        _weaponImages[newWeaponIndex].gameObject.SetActive(true);
        _weaponStatus[newWeaponIndex].SetActive(true);

        _currentWeaponIndex = newWeaponIndex;
    }

    private void InitializeWeaponImages()
    {
        _weaponImages = new List<Image>();
        foreach(Transform weaponImage in _weaponImagesParent.transform)
        {
            _weaponImages.Add(weaponImage.gameObject.GetComponent<Image>());
        }
    }

    private void InitializeWeaponStatus()
    {
        _weaponStatus = new List<GameObject>();
        foreach(Transform weaponStatus in _weaponStatusParent.transform)
        {
            _weaponStatus.Add(weaponStatus.gameObject);
        }
    }
}
