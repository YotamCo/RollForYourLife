using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    HealthController _healthControllerScript;
    private int _maxHealth; //Will need when player will receive health somehow
    private int _currentHealth;
    private List<Image> _healthImages;

    // Start is called before the first frame update
    void Start()
    {
        HealthController.onPlayerTakeDamage += LowerHealthUI;
        GameObject player = GameObject.Find("Player");
        Debug.Assert(player != null, "Couldn't find player");
        _healthControllerScript = player.GetComponent<HealthController>();
        if(_healthControllerScript == null)
        {
            Debug.LogWarning("HealthController script is not loaded so removing health UI");
            Destroy(gameObject);
        }
        _maxHealth = _currentHealth = _healthControllerScript.GetHealth();

        InitializeHealthImages();
    }

    private void LowerHealthUI()
    {
        _healthImages[_currentHealth - 1].gameObject.SetActive(false);
        _currentHealth--;
    }

    private void InitializeHealthImages()
    {
        _healthImages = new List<Image>();
        foreach(Transform healthObject in transform)
        {
            _healthImages.Add(healthObject.gameObject.GetComponent<Image>());
        }
    }
}
