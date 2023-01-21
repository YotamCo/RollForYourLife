using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour
{
    [SerializeField] protected GameObject deathEffectPrefab;
    [SerializeField] protected float movementSpeed = 5;
    [SerializeField] protected float movementEverySecs;

    public delegate void OnEnemyDeath(GameObject enemy);
    public static OnEnemyDeath onEnemyDeath;
    
    public delegate void OnEnemyHitPlayer(); //right now enemies do 1 damage so no need to pass enemy and check how much damage it inflicts
    public static OnEnemyHitPlayer onEnemyHitPlayer;

    protected ValidPositionChecker validPositionChecker;
    protected int numOfPossibleMovingDirections = 4;
    protected Vector3[] possibleDirections;
    protected float lastTimeMoved = 0f;


    void Start()
    {
        possibleDirections = new []{ new Vector3(1f, 0f, 0f), new Vector3(-1f, 0f, 0f),
                                new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f)};
        validPositionChecker = new ValidPositionChecker();

        SpecificInitializations();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Weapon" || collision.tag == "Bullet")
        {
            Die();
        }

        if(collision.tag == "Player")
        {
            onEnemyHitPlayer?.Invoke();
            Die();
        }
    }

    protected void Die()
    {
        onEnemyDeath?.Invoke(gameObject);
        SpecificEnemyDeathEffect();
    }

    public void KillEnemyOnMapClear()
    {
        Die();
    }

    protected abstract void SpecificInitializations();
    protected abstract void SpecificEnemyDeathEffect();
    protected abstract void Move();
}
