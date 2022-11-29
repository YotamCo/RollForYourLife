using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    public delegate void OnWeaponItemTaken(GameObject item);
    public static OnWeaponItemTaken onWeaponItemTaken;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            WeaponItemTaken();
        }
    }

    public void WeaponItemTaken()
    {
        onWeaponItemTaken?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
