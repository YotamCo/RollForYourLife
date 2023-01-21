using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollManager : MonoBehaviour
{
    public delegate void OnDieRoll(int dieScore, int totalRollScore);
    public static OnDieRoll onDieRoll;

    public delegate void OnZeroTotalDieScore();
    public static OnZeroTotalDieScore onZeroTotalDieScore;

    private int[] _dieRollScore;
    private int _totalDieScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        DiceController.onDiePickedUp += RollDie;
    }

    private void TriggerEventBasedOnScore(int totalDiceScore)
    {
        /*if(totalDiceScore >= 2 && totalDiceScore < minimumScoreNeededForLevelUp)
        {
        }
        else if(totalDiceScore >= minimumScoreNeededForLevelUp && totalDiceScore <= 12)
        {
        }*/
    }

    private void RollDie(GameObject dieObject) //TODO: add animation and delay for the roll animation
    {
        int randScore = Random.Range(1, 7);
        _totalDieScore += randScore;
        onDieRoll?.Invoke(randScore, _totalDieScore);
    }

    public void ZeroTotalDieScore()
    {
        _totalDieScore = 0;
        onZeroTotalDieScore?.Invoke();
    }
}
