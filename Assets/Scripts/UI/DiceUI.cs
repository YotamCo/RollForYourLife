using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DiceUI : MonoBehaviour
{
    public Image dieImage;
    public Sprite[] diceSprites;
    public GameObject dieScore;
    private TextMeshProUGUI _totalDieScoreText;

    [SerializeField] float _timeToShowDice = 4f;
    private float _lastShowedDice = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _totalDieScoreText = dieScore.GetComponent<TextMeshProUGUI>();
        DiceRollManager.onDieRoll += UpdateDiceUI;
        DiceRollManager.onZeroTotalDieScore += ZeroTotalRollScore;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - _lastShowedDice > _timeToShowDice)
        {
            TurnOffDiceUI();
        }
    }

    private void UpdateDiceUI(int dieScore, int totalDieScore)
    {
        dieImage.gameObject.SetActive(true);
        dieImage.sprite = diceSprites[dieScore - 1];
        int newTotalDieScore = totalDieScore;
        _totalDieScoreText.text = newTotalDieScore.ToString();

        _lastShowedDice = Time.time;
    }

    public void ZeroTotalRollScore()
    {
        int newTotalDieScore = 0;
        _totalDieScoreText.text = newTotalDieScore.ToString();
    }

    void TurnOffDiceUI()
    {
        dieImage.gameObject.SetActive(false); ;
    }
}
