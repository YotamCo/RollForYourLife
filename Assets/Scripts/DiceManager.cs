using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{

    public delegate void OnRollingSufficientScore();
    public static OnRollingSufficientScore onRollingSufficientScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("KeyDown");
            PassToNextLevel();
        }
    }

    void PassToNextLevel()
    {
        onRollingSufficientScore?.Invoke();
    }
}
