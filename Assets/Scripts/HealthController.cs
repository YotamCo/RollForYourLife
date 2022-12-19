using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] float _invincibilityPeriod = 1f;
    [SerializeField] bool _isPlayerInvincible = false;

    private float _lastTimeTookDamage = 0f;
    private int _health = 3;

    public delegate void OnPlayerTakeDamage();
    public static OnPlayerTakeDamage onPlayerTakeDamage;

    // Start is called before the first frame update
    void Start()
    {
        EnemyController.onEnemyHitPlayer += GotHit;
    }

    public int GetHealth()
    {
        return _health;
    }

    private void GotHit()
    {
        if(Time.time - _lastTimeTookDamage > _invincibilityPeriod 
        && !_isPlayerInvincible)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        Debug.Log("Player got hit");
        _health--;
        if(_health <= 0)
        {
            //Invoke event player died
            Destroy(gameObject);
        }
        onPlayerTakeDamage?.Invoke();
        _lastTimeTookDamage = Time.time;
    }
}
