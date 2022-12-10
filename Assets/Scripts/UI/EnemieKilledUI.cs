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
         to EnemyController.onEnemyDeath because there is a racing to which the event will reach
         sooner. It might cause a delayed enemiesKilledText by 1 */

        GameObject gameManager = GameObject.Find("GameManager");
        Debug.Assert(gameManager != null);
        _enemySpawnerScript = gameManager.GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateEnemiesKilledUI()
    {
        Debug.Assert(_numOfEnemiesKilledText.text != null);
        _numOfEnemiesKilledText.text = _enemySpawnerScript.GetTotalNumOfEnemiesKilled().ToString();
    }
}
