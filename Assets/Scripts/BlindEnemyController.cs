using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindEnemyController : EnemyController
{
    private Vector3 _nextPositon;
    private float _lastTimeMoved = 0f;

    [SerializeField] private float _movingSpeed = 5; //TODO: Should put it in EnemyController

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        
        if(Time.time - _lastTimeMoved > _movementEverySecs)
        {
            int randDirection = Random.Range(0, numOfPossibleMovingDirections);
            Vector3 possibleNextPosition = transform.position + possibleDirections[randDirection];

            if(mapManagerScript.IsWantedPositionLegal(possibleNextPosition))
            {
                _nextPositon = possibleNextPosition;
                _lastTimeMoved = Time.time;
            }
        }

        if(transform.position != _nextPositon)
        {
                transform.position = Vector3.MoveTowards(transform.position, _nextPositon, _movingSpeed * Time.deltaTime);
        }
    }

    protected override void SpecificInitializations()
    {
        _nextPositon = transform.position;
    }

    protected override void SpecificEnemyDeathEffect()
    {
        //TODO: add later
    }
}
