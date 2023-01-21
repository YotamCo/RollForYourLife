using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelTransitionManager : MonoBehaviour
{
    [SerializeField] int[] levelTargetScore;
    [SerializeField] float timeUntilTriggerNewLevelScene = 1f;
    [SerializeField] float timeUntilGeneratingNewLevelMap = 0.3f;
    [SerializeField] GameObject passedLevelScreen; //TODO: make nicer

    private SpawnerManager spawnerManager;
    private PlayerMovement playerMovement; //TODO: refactor name or split to another script
    private WeaponManager weaponManager;

    private WallSpawner wallSpawnerScript;
    private DiceRollManager diceRollManagerScript;

    public delegate void OnUpdatingTargetScoreWhenLevelUp(int newTargetScore);
    public static OnUpdatingTargetScoreWhenLevelUp onUpdatingTargetScoreWhenLevelUp;

    private int currentLevel = 1;


    private void Start()
    {
        DiceRollManager.onDieRoll += CheckIfReachedTargetScore;
        GameObject spawnersHolder = GameObject.Find("SpawnersHolder");
        wallSpawnerScript         = spawnersHolder.GetComponent<WallSpawner>();
        diceRollManagerScript     = gameObject.GetComponent<DiceRollManager>();
        spawnerManager            = gameObject.GetComponent<SpawnerManager>();
        GameObject player         = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        weaponManager = player.GetComponent<WeaponManager>();

        currentLevel = 1; //TODO: need to initialize this value to what is being chosen from the main menu
        TransitionToNextLevel(); 
    }

    void FinishedLevel()
    {
        playerMovement.enabled = false;
        spawnerManager.ChangeSpawningStatus(false);
        spawnerManager.ClearPrefabsFromMap();
        // Clear walls

        
        
        passedLevelScreen.SetActive(true);
    }

    public void TransitionToNextLevel()
    {
        passedLevelScreen.SetActive(false);
        onUpdatingTargetScoreWhenLevelUp?.Invoke(GetLevelTargetScore()); //Listeners: TargetScoreUI
        diceRollManagerScript.ZeroTotalDieScore();
        // ~~~~~ Move player to a certain position. Maybe an entrance animation ~~~~~~~
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(-3, -3, 0); //TODO: use a script (I need to create one)that will be in charge of moving the player
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        wallSpawnerScript.PrepareNewLevelMap(GetCurrentLevel()); //TODO: separating between removing the walls and creating new ones. 2 separate functions
        spawnerManager.ChangeSpawningStatus(true);
        weaponManager.DropWeapon();
        playerMovement.enabled = true;        
    }

    IEnumerator PrepareNewLevelMapCoroutine()
    {
        yield return new WaitForSeconds(timeUntilGeneratingNewLevelMap); //TODO: need to change to a smarter check if the new level scene was already loaded
        wallSpawnerScript.PrepareNewLevelMap(GetCurrentLevel());
        spawnerManager.ChangeSpawningStatus(true);
    }

    private void CheckIfReachedTargetScore(int dieRollScore, int totalRollScore)
    {
        if(totalRollScore >= GetLevelTargetScore())
        {
            currentLevel++; // TODO: see how to make it nicer
            FinishedLevel();
        }
    }


    void FirstLevelInitializations()
    {
        wallSpawnerScript.PrepareNewLevelMap(GetCurrentLevel());
    }

    private int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetLevelTargetScore()
    {
        return levelTargetScore[GetCurrentLevel() - 1];
    }

    /*private void CheckIfReachedTargetScore(int dieRollScore, int totalRollScore)
    {
        if(totalRollScore >= GetLevelTargetScore())
        {
            LevelUp();
        }
    }*/

    /*private void LevelUp()
    {
        spawnerManager.ChangeSpawningStatus(false);
        spawnerManager.ClearPrefabsFromMap();
        currentLevel++;
        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(timeUntilTriggerNewLevelScene);
        TransitionToNextLevel();
    }

    private void TransitionToNextLevel()
    {
        onUpdatingTargetScoreWhenLevelUp?.Invoke(GetLevelTargetScore()); //Listeners: TargetScoreUI
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(PrepareNewLevelMapCoroutine());
        diceRollManagerScript.ZeroTotalDieScore();
    }

    IEnumerator PrepareNewLevelMapCoroutine()
    {
        yield return new WaitForSeconds(timeUntilGeneratingNewLevelMap); //TODO: need to change to a smarter check if the new level scene was already loaded
        wallSpawnerScript.PrepareNewLevelMap(GetCurrentLevel());
        spawnerManager.ChangeSpawningStatus(true);
    }*/
}
