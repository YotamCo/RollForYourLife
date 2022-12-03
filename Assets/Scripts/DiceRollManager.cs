using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollManager : MonoBehaviour
{

    public delegate void OnRollingSufficientScore();
    public static OnRollingSufficientScore onRollingSufficientScore;

    public delegate void OnRollingWeaponItemSpawn();
    public static OnRollingWeaponItemSpawn onRollingWeaponItemSpawn;

    public delegate void OnDieRoll(int dieScore, int dieIndex);
    public static OnDieRoll onDieRoll;

    private int[] _dieRollScore;
    private int _totalNumOfRolls = 0;

    [SerializeField] private int minimumScoreNeededForLevelUp = 10;

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
    }

    private void TriggerEventBasedOnScore(int totalDiceScore)
    {
        if(totalDiceScore >= 2 && totalDiceScore < minimumScoreNeededForLevelUp)
        {
            onRollingWeaponItemSpawn?.Invoke();
        }
        else if(totalDiceScore >= minimumScoreNeededForLevelUp && totalDiceScore <= 12)
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
        onDieRoll?.Invoke(randScore, _totalNumOfRolls % 2);
        _dieRollScore[_totalNumOfRolls % 2] = randScore;
        _totalNumOfRolls++;
    }
}
