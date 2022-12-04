using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapManager _mapManagerScript;
    private LevelManager _levelManagerScript;
    private DiceRollManager _diceRollManagerScript;

    public delegate void OnCleanupBeforeLevelUp();
    public static OnCleanupBeforeLevelUp onCleanupBeforeLevelUp;

    // Start is called before the first frame update
    void Start()
    {
        _mapManagerScript       = gameObject.GetComponent<MapManager>();
        _levelManagerScript     = gameObject.GetComponent<LevelManager>();
        _diceRollManagerScript  = gameObject.GetComponent<DiceRollManager>();

        DiceRollManager.onDieRoll += CheckIfReachedTargetScore;

        FirstGameInitialization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckIfReachedTargetScore(int dieRollScore, int totalRollScore)
    {
        if(totalRollScore >= _levelManagerScript.GetLevelTargetScore())
        {
            PassedCurrentLevel();
        }
    }

    void PassedCurrentLevel()
    {
        onCleanupBeforeLevelUp?.Invoke();
        _levelManagerScript.LevelUp();
        _diceRollManagerScript.LevelUp();
        StartCoroutine(DelayCoroutine());
        //mapGeneratorScript.PrepareNewLevelObstacles(levelManagerScript.GetCurrentLevel());
        //mapGeneratorScript.PrepareNewLevelObstacles(2);
    }

    void FirstGameInitialization()
    {
        _mapManagerScript.PrepareNewLevelObstacles(_levelManagerScript.GetCurrentLevel());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _mapManagerScript.PrepareNewLevelObstacles(_levelManagerScript.GetCurrentLevel());
    }
}
