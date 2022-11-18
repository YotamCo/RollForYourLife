using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollManager : MonoBehaviour
{

    public delegate void OnRollingSufficientScore();
    public static OnRollingSufficientScore onRollingSufficientScore;

    private int[] _dieRollScore;
    private int _totalNumOfRolls = 0;
    // Start is called before the first frame update
    void Start()
    {
        DiceController.onDiePickedUp += RollDie;
        _dieRollScore = new int[2];
    }

    // Update is called once per frame
    void Update()
    {
        if(isReadyToRollDice())
        {
            TriggerEventBasedOnScore(_dieRollScore[0] + _dieRollScore[1]);
            _dieRollScore[1] = -1;
        }

        /*if (Input.GetKeyDown("space"))
        {
            Debug.Log("KeyDown");
            PassToNextLevel();
        }*/
    }

    private void TriggerEventBasedOnScore(int totalDiceScore)
    {
        if(totalDiceScore > 2 && totalDiceScore <= 5)
        {

        }
        else if(totalDiceScore > 5 && totalDiceScore <= 12)
        {
            onRollingSufficientScore?.Invoke();
        }
    }

    private bool isReadyToRollDice()
    {
        if(_totalNumOfRolls > 0 && (_totalNumOfRolls % 2 == 0) && _dieRollScore[1] != -1)
            return true;
        return false;
    }
    /*void PassToNextLevel()
    {
        onRollingSufficientScore?.Invoke();
    }*/

    private void RollDie(GameObject dieObject) //TODO: add animation and delay for the roll animation
    {
        int randScore = Random.Range(1, 7); //TODO: Make a better roll using function of number of enemies killed
        Debug.Log("Die Roll Score = " + randScore);
        _dieRollScore[_totalNumOfRolls % 2] = randScore;
        _totalNumOfRolls++;
    }
}
