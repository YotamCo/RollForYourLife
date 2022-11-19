using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    /* --------------- Dice Begin --------------- */
    public Image[] diceImages;
    public Sprite[] diceSprites;
    public GameObject dieScore;
    private TextMeshProUGUI _dieScoreText;

    [SerializeField] float _timeToShowDice = 4f;
    private float _lastShowedDice = 0f;

    private int[] _diceValues;
    /* --------------- Dice End --------------- */

    /* --------------- Enemies Begin --------------- */
    private EnemySpawner _enemySpawnerScript; 
    public GameObject numOfEnemiesKilled;
    private TextMeshProUGUI _numOfEnemiesKilledText;
    /* --------------- Enemies End --------------- */


    // Start is called before the first frame update
    void Start()
    {
        _dieScoreText = dieScore.GetComponent<TextMeshProUGUI>();
        _numOfEnemiesKilledText = numOfEnemiesKilled.GetComponent<TextMeshProUGUI>();
        Debug.Assert(_numOfEnemiesKilledText != null);

        _diceValues = new int[]{-1, -1};
        DiceRollManager.onDieRoll += DieWasPickedUp;
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
        if(Time.time - _lastShowedDice > _timeToShowDice)
        {
            TurnOffDiceUI();
        }

        if(_diceValues[0] != -1 && _diceValues[1] != -1)
        {
            UpdateDiceUI();
        }
    }

    private void DieWasPickedUp(int dieScore, int dieIndex)
    {
        _diceValues[dieIndex] = dieScore;
    }

    private void UpdateDiceUI()
    {
        diceImages[0].gameObject.SetActive(true);
        diceImages[0].sprite = diceSprites[_diceValues[0] - 1];
        diceImages[1].gameObject.SetActive(true);
        diceImages[1].sprite = diceSprites[_diceValues[1] - 1];
        dieScore.gameObject.SetActive(true);
        _dieScoreText.text = (_diceValues[0]+ _diceValues[1]).ToString();

        _lastShowedDice = Time.time;
        _diceValues[0] = -1;
        _diceValues[1] = -1;
    }

    private void UpdateEnemiesKilledUI()
    {
        Debug.Assert(_numOfEnemiesKilledText.text != null);
        _numOfEnemiesKilledText.text = _enemySpawnerScript.GetTotalNumOfEnemiesKilled().ToString();
    }

    void TurnOffDiceUI()
    {
        dieScore.gameObject.SetActive(false);
        diceImages[0].gameObject.SetActive(false); ;
        diceImages[1].gameObject.SetActive(false); ;
    }
}
