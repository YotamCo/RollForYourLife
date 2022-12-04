using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected GameObject _deathEffectPrefab;
    [SerializeField] protected float movementEverySecs;

    public delegate void OnEnemyDeath(GameObject enemy);
    public static OnEnemyDeath onEnemyDeath;
    
    public delegate void OnEnemyHitPlayer(); //right now enemies do 1 damage so no need to pass enemy and check how much damage it inflicts
    public static OnEnemyHitPlayer onEnemyHitPlayer;

    protected MapManager mapManagerScript;
    protected int numOfPossibleMovingDirections = 4;
    protected Vector3[] possibleDirections;
    protected float lastTimeMoved = 0f;


    // Start is called before the first frame update
    void Start()
    {
        //optionalMoves = {right, left, up, down}
        possibleDirections = new []{ new Vector3(1f, 0f, 0f), new Vector3(-1f, 0f, 0f),
                                new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f)};
        GameObject gameManager = GameObject.Find("GameManager");
        Debug.Assert(gameManager != null);
        mapManagerScript = gameManager.GetComponent<MapManager>();

        SpecificInitializations();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Weapon" || collision.tag == "Bullet")
        {
            Die();
        }

        if(collision.tag == "Player") //TODO: Will be left here because I think after is a monster touches the player it damages him and dies
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
