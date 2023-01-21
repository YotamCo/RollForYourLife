using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindEnemyController : AbstractEnemy
{
    private Vector3 blindNextPosition;

    //TODO - Refactor
    // Make it work the same on on different computers. delta time
    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        MoveRandomly();
    }

    private void MoveRandomly()
    {
        if(Time.time - lastTimeMoved > movementEverySecs)
        {
            int randDirection = Random.Range(0, numOfPossibleMovingDirections);
            Vector3 possibleNextPosition = transform.position + possibleDirections[randDirection];

            if(validPositionChecker.IsMovementPositionLegal(possibleNextPosition)) 
            {
                blindNextPosition = possibleNextPosition;
                lastTimeMoved = Time.time;
            }
        }

        if(transform.position != blindNextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, blindNextPosition, movementSpeed * Time.deltaTime);
        }
    }

    protected override void SpecificInitializations()
    {
        blindNextPosition = transform.position;
    }

    protected override void SpecificEnemyDeathEffect()
    {
        GameObject effect = Instantiate(deathEffectPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(effect, 1);
    }
}
