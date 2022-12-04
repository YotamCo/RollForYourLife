using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int[] levelTargetScore;
    private int _currentLevel = 1;

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public int GetLevelTargetScore()
    {
        return levelTargetScore[_currentLevel - 1];
    }

    public void LevelUp()
    {
        _currentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
