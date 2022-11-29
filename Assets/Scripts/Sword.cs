using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] float _timeBeforeDrops = 5f;
    private float _timeOfInstantiation;

    protected override void SpecificWeaponInitializations()
    {
        _timeOfInstantiation = Time.time;
    }

    public override bool IsTimeToDropWeapon()
    {
        if(Time.time - _timeOfInstantiation > _timeBeforeDrops)
        {
            return true;
        }
        return false;
    }

    public override string GetWeaponName()
    {
        return "Sword";
    }

    public float GetTimeBeforeDrops()
    {
        return _timeBeforeDrops;
    }
}
