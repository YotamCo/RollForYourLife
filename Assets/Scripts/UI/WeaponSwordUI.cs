using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwordUI : MonoBehaviour
{
    private float _swordDuration;
    private bool _wasInitialized = false;
    private float _activatedAtTime;
    private Sword _sworedScript;
    private TextMeshProUGUI _timeUntilDropSword;


    private void FixedUpdate()
    {
        Debug.Log("Got to FixedUpdate");
        UpdateSwordUI(); //TODO: make sure it needs to be in FixedUpdate and not Update
    }

    private void OnEnable()
    {
        Debug.Log("Got to OnEnable");
        if(!_wasInitialized)
        {
            InitializeUI();
        }
    }

    private void InitializeUI()
    {
        //GameObject swordPrefab = GameObject.Find("SwordObject");
        //Debug.Assert(swordPrefab != null); 
        //_sworedScript = swordPrefab.GetComponent<Sword>();
        //_swordDuration = _sworedScript.GetTimeBeforeDrops();
        _swordDuration = 5f;
        _activatedAtTime = Time.time;

        _timeUntilDropSword = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        _wasInitialized = true;
    }

    private void UpdateSwordUI()
    {
        float timePassed = Time.time - _activatedAtTime;
        float timeLeft = _swordDuration - timePassed;
        _timeUntilDropSword.text = timeLeft.ToString("n1");
    }
}
