using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    MapManager mapManagerScript;
    LevelManager levelManagerScript;

    public delegate void OnCleanupBeforeLevelUp();
    public static OnCleanupBeforeLevelUp onCleanupBeforeLevelUp;

    // Start is called before the first frame update
    void Start()
    {
        mapManagerScript = gameObject.GetComponent<MapManager>();
        levelManagerScript = gameObject.GetComponent<LevelManager>();

        DiceRollManager.onRollingSufficientScore += PassedCurrentLevel;

        FirstGameInitialization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PassedCurrentLevel()
    {
        onCleanupBeforeLevelUp?.Invoke();
        levelManagerScript.LevelUp();
        StartCoroutine(DelayCoroutine());
        //mapGeneratorScript.PrepareNewLevelObstacles(levelManagerScript.GetCurrentLevel());
        //mapGeneratorScript.PrepareNewLevelObstacles(2);
    }

    void FirstGameInitialization()
    {
        mapManagerScript.PrepareNewLevelObstacles(levelManagerScript.GetCurrentLevel());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        mapManagerScript.PrepareNewLevelObstacles(levelManagerScript.GetCurrentLevel());
    }
}
