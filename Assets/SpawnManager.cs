using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private bool _needToSpawnDie = false;
    private float _trySpawningEverySec = 1f;
    private float _lastTriedSpawningDie = 0f;
    [SerializeField] private int _dieSpawnChance = 10; //Every 1 second there's 1/_dieSpawnChance to spawn a die
    public GameObject diePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - _lastTriedSpawningDie > _trySpawningEverySec)
        {
            TrySpawningDie();
            _lastTriedSpawningDie = Time.time;
        }
        if(_needToSpawnDie)
        {
            
        }
    }

    void TrySpawningDie()
    {
        int random = Random.Range(0, _dieSpawnChance);
        if(random == 0)
        {
            _needToSpawnDie = true;
        }
    }
}
