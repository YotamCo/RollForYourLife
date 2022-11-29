using System.Collections;
using UnityEngine;

public class Fist : Weapon
{

    public override bool IsTimeToDropWeapon()
    {
        return false;    
    }

    public override string GetWeaponName()
        {
            return "Fist";
        }
        

}
