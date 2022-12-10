using UnityEngine;
using TMPro;

public class TargetScoreUI : MonoBehaviour
{
    private int _levelTargetScore;
    public TextMeshProUGUI _levelTargetScoreText;
    LevelTransitionManager _levelTransitionManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        LevelTransitionManager.onUpdatingTargetScoreWhenLevelUp += UpdateTargetScoreUI;
        GameObject gameManager = GameObject.Find("GameManager");

        _levelTransitionManagerScript = gameManager.GetComponent<LevelTransitionManager>();
        _levelTargetScore = _levelTransitionManagerScript.GetLevelTargetScore(); //TODO:Error Check - Make sure it's not causing problems
        _levelTargetScoreText.text = _levelTargetScore.ToString();
    }

    private void UpdateTargetScoreUI(int newScore)
    {
        _levelTargetScoreText.text = newScore.ToString();
    }
}
