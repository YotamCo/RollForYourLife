using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public delegate void OnDiePickedUp(GameObject item);
    public static OnDiePickedUp onDiePickedUp;

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
            DicePickedUp();
        }
    }

    private void DicePickedUp()
    {
        onDiePickedUp?.Invoke(gameObject);
    }
}
