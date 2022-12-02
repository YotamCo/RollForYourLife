using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DiceUI : MonoBehaviour
{
    public Image[] diceImages;
    public Sprite[] diceSprites;
    public GameObject dieScore;
    private TextMeshProUGUI _dieScoreText;

    [SerializeField] float _timeToShowDice = 4f;
    private float _lastShowedDice = 0f;

    private int[] _diceValues;


    // Start is called before the first frame update
    void Start()
    {
        _dieScoreText = dieScore.GetComponent<TextMeshProUGUI>();

        _diceValues = new int[]{-1, -1};
        DiceRollManager.onDieRoll += DieWasPickedUp;
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

    void TurnOffDiceUI()
    {
        dieScore.gameObject.SetActive(false);
        diceImages[0].gameObject.SetActive(false); ;
        diceImages[1].gameObject.SetActive(false); ;
    }
}
