using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemyController : AbstractEnemy
{
    private List<Vector3> currentPossibleMovements;
    private GameObject player;
    private Vector3 chasingNextPosition;
    // TODO - Refactor
    // Make it work the same on on different computers. delta time
    void Update()
    {
        Move();
    }

    protected override void SpecificInitializations()
    {
        currentPossibleMovements = new List<Vector3>();
        player = GameObject.Find("Player");
    }

    // TODO - Refactor
    // I want to improve the movement of the enemy. Also I'll think of another function
    // and not MoveRandomly in order to not get stuck
    protected override void Move()
    {
        if(player != null) //TODO: Can add here an invisibility option. If player is invisible then MoveRandomly()
        {
            MoveChasingObject(player.transform.position);
        }
    }

    private void MoveChasingObject(Vector3 targetPosition)
    { 
        if(Time.time - lastTimeMoved > movementEverySecs)
        {
            AddCurrentPossibleMovements(targetPosition);
            int randDirection = Random.Range(0, currentPossibleMovements.Count);
            Vector3 possibleNextPosition = transform.position + currentPossibleMovements[randDirection];

            if(validPositionChecker.IsMovementPositionLegal(possibleNextPosition))
            {
                chasingNextPosition = possibleNextPosition;
                lastTimeMoved = Time.time;
            }

            currentPossibleMovements.Clear();
        }
        if(transform.position != chasingNextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, chasingNextPosition, movementSpeed * Time.deltaTime);
        }
    }

    private void AddCurrentPossibleMovements(Vector3 targetPosition)
    {
        if(targetPosition.x - transform.position.x > 0)
            {
                currentPossibleMovements.Add(possibleDirections[0]);
            }
            else if(targetPosition.x - transform.position.x < 0)
            {
                currentPossibleMovements.Add(possibleDirections[1]);
            }

            if(targetPosition.y - transform.position.y > 0)
            {
                currentPossibleMovements.Add(possibleDirections[2]);
            }
            else if(targetPosition.y - transform.position.y < 0)
            {
                currentPossibleMovements.Add(possibleDirections[3]);
            }
    }

    protected override void SpecificEnemyDeathEffect()
    {
        GameObject effect = Instantiate(deathEffectPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(effect, 1);
    }
}
