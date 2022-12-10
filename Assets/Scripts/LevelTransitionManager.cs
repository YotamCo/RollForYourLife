using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelTransitionManager : MonoBehaviour
{
    enum SpawnerScriptsIndex //TODO: move it to a more generic place. Understand where does it fit
    {
        ENEMY_SPAWNER = 0,
        DIE_SPAWNER = 1,
        WEAPON_ITEM_SPAWNER = 2
    }

    [SerializeField] int[] levelTargetScore;
    [SerializeField] float _timeUntilTriggerNewLevelScene = 1f;
    [SerializeField] float _timeUntilGeneratingNewLevelMap = 0.3f;

    private List<AbstractSpawnManager> _spawnerScripts;

    private MapManager _mapManagerScript;
    private DiceRollManager _diceRollManagerScript;

    public delegate void OnCleanupBeforeLevelUp();
    public static OnCleanupBeforeLevelUp onCleanupBeforeLevelUp;

    public delegate void OnUpdatingTargetScoreWhenLevelUp(int newScore);
    public static OnUpdatingTargetScoreWhenLevelUp onUpdatingTargetScoreWhenLevelUp;

    private int _currentLevel = 1;


    private void Start()
    {
        DiceRollManager.onDieRoll += CheckIfReachedTargetScore;
        _mapManagerScript       = gameObject.GetComponent<MapManager>();
        _diceRollManagerScript  = gameObject.GetComponent<DiceRollManager>();
        InitializeSpawnerScripts();
        FirstLevelInitializations();
    }

    private void InitializeSpawnerScripts()
    {
        _spawnerScripts = new List<AbstractSpawnManager>();
        _spawnerScripts.Add(gameObject.GetComponent<EnemySpawner>());
        _spawnerScripts.Add(gameObject.GetComponent<DieSpawner>());
        _spawnerScripts.Add(gameObject.GetComponent<WeaponItemSpawner>());
    }

    void FirstLevelInitializations()
    {
        _mapManagerScript.PrepareNewLevelMap(GetCurrentLevel());
    }

    private int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public int GetLevelTargetScore()
    {
        return levelTargetScore[GetCurrentLevel() - 1];
    }

    private void CheckIfReachedTargetScore(int dieRollScore, int totalRollScore)
    {
        if(totalRollScore >= GetLevelTargetScore())
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        ChangeSpawningStatus(false);
        ClearPrefabsFromMap();
        _currentLevel++;
        StartCoroutine(DelayCoroutine());
    }

    private void ClearPrefabsFromMap()
    {
        onCleanupBeforeLevelUp?.Invoke(); //Listeners: AbstractSpawnerManager
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(_timeUntilTriggerNewLevelScene);
        TransitionToNextLevel();
    }

    private void TransitionToNextLevel()
    {
        onUpdatingTargetScoreWhenLevelUp?.Invoke(GetLevelTargetScore()); //Listeners: TargetScoreUI
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(PrepareNewLevelMapCoroutine());
        _diceRollManagerScript.ZeroTotalDieScore();
    }

    IEnumerator PrepareNewLevelMapCoroutine()
    {
        yield return new WaitForSeconds(_timeUntilGeneratingNewLevelMap); //TODO: need to change to a smarter check if the new level scene was already loaded
        _mapManagerScript.PrepareNewLevelMap(GetCurrentLevel());
        ChangeSpawningStatus(true);
    }

    private void ChangeSpawningStatus(bool isSetToSpawning)
    {
        foreach(AbstractSpawnManager spawnerScript in _spawnerScripts)
        {
            spawnerScript.ChangeSpawningStatus(isSetToSpawning);
        }
    }
}
