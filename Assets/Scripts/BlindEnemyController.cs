using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindEnemyController : EnemyController
{
    private Vector3 _blindNextPosition;

    [SerializeField] protected float movingSpeed = 5; //TODO: Should put it in EnemyController

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        MoveRandomly();
    }

    protected void MoveRandomly()
    {
        if(Time.time - lastTimeMoved > movementEverySecs)
        {
            int randDirection = Random.Range(0, numOfPossibleMovingDirections);
            Vector3 possibleNextPosition = transform.position + possibleDirections[randDirection];

            if(mapManagerScript.IsMovementPositionLegal(possibleNextPosition))
            {
                _blindNextPosition = possibleNextPosition;
                lastTimeMoved = Time.time;
            }
        }

        if(transform.position != _blindNextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _blindNextPosition, movingSpeed * Time.deltaTime);
        }
    }

    protected override void SpecificInitializations()
    {
        _blindNextPosition = transform.position;
    }

    protected override void SpecificEnemyDeathEffect()
    {
        //TODO: add later
    }
}
