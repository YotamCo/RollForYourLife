using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollManager : MonoBehaviour
{

    public delegate void OnRollingSufficientScore();
    public static OnRollingSufficientScore onRollingSufficientScore;

    public delegate void OnRollingWeaponItemSpawn();
    public static OnRollingWeaponItemSpawn onRollingWeaponItemSpawn;

    public delegate void OnDieRoll(int dieScore, int totalRollScore);
    public static OnDieRoll onDieRoll;

    public delegate void OnLevelUp();
    public static OnLevelUp onLevelUp;

    private int[] _dieRollScore;
    private int _totalDieScore = 0;
    private int _totalNumOfRolls = 0;

    [SerializeField] private int minimumScoreNeededForLevelUp = 10;

    // Start is called before the first frame update
    void Start()
    {
        DiceController.onDiePickedUp += RollDie;
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

    private void RollDie(GameObject dieObject) //TODO: add animation and delay for the roll animation
    {
        int randScore = Random.Range(1, 7); //TODO: Make a better roll using function of number of enemies killed
        Debug.Log("Die Roll Score = " + randScore);
        _totalDieScore += randScore;
        onDieRoll?.Invoke(randScore, _totalDieScore);
        _totalNumOfRolls++;
    }

    public void LevelUp()
    {
        _totalDieScore = 0;
        onLevelUp?.Invoke();
    }
}
