using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EnemieKilledUI : MonoBehaviour
{
    private EnemySpawner _enemySpawnerScript; 
    public GameObject numOfEnemiesKilled;
    private TextMeshProUGUI _numOfEnemiesKilledText;

    // Start is called before the first frame update
    void Start()
    {
        _numOfEnemiesKilledText = numOfEnemiesKilled.GetComponent<TextMeshProUGUI>();
        Debug.Assert(_numOfEnemiesKilledText != null);
        EnemySpawner.onEnemyDeathUpdateUI += UpdateEnemiesKilledUI; /* It is not directly listening
         to AbstractEnemy.onEnemyDeath because there is a racing to which the event will reach
         sooner. It might cause a delayed enemiesKilledText by 1 */

        GameObject spawnersHolder = GameObject.Find("SpawnersHolder");
        Debug.Assert(spawnersHolder != null); //TODO: should be warning?
        _enemySpawnerScript = spawnersHolder.GetComponent<EnemySpawner>();
    }

    private void UpdateEnemiesKilledUI()
    {
        Debug.Assert(_numOfEnemiesKilledText.text != null);
        _numOfEnemiesKilledText.text = _enemySpawnerScript.GetTotalNumOfEnemiesKilled().ToString();
    }
}
