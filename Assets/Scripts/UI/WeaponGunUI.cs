using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponGunUI : MonoBehaviour
{
    private const int numOfBulletsInBasicGun = 3;
    private int _numOfBulletsLeft;
    private List<Image> _bulletImages;
    private bool _wasInitialized = false;

    private void OnEnable()
    {
        if(!_wasInitialized)
        {
            InitializeBulletsStatus();
        }
        RefillAllBulletImages();
    }

    private void RefillAllBulletImages()
    {
        foreach(Image bulletImage in _bulletImages)
        {
            bulletImage.gameObject.SetActive(true);
        }
        _numOfBulletsLeft = numOfBulletsInBasicGun;
    }

    private void UpdateBulleUI()
    {
        Debug.Assert(_numOfBulletsLeft > 0);
        _bulletImages[_numOfBulletsLeft-1].gameObject.SetActive(false);
        _numOfBulletsLeft--;
    }

    private void InitializeBulletsStatus()
    {
        Gun.onGunFires += UpdateBulleUI;
        _bulletImages = new List<Image>();
        foreach(Transform weaponStatus in transform)
        {
            _bulletImages.Add(weaponStatus.gameObject.GetComponent<Image>());
        }
        _numOfBulletsLeft = numOfBulletsInBasicGun;
        _wasInitialized = true;
    }
}
