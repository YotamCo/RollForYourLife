using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemyController : BlindEnemyController
{

    private List<Vector3> currentPossibleMovements; //TODO: need to remember to clean list after choosing a direction
    private GameObject _player;
    
    private Vector3 _chasingNextPosition;

    void Update()
    {
        Move();
    }

    protected override void SpecificInitializations()
    {
        currentPossibleMovements = new List<Vector3>();
        _player = GameObject.Find("Player");
        _chasingNextPosition = transform.position;
    }

    protected override void Move()
    {
        if(_player != null) //TODO: !! Check how to check if a script is null or inactive
        //TODO: doesn't work well. Need to debug!
        {
            MoveChasingPlayer();
        }
        else
        {
            _player = GameObject.Find("GameManager");
            if(_player == null)
            {
                MoveRandomly();
            }
            else
            {
                MoveChasingPlayer();
            }
        }
    }

    private void MoveChasingPlayer() //TODO: check if I can add a random movement in case an enemy is stuck
    {
        if(Time.time - lastTimeMoved > movementEverySecs)
        {
            Vector3 playerPosition = _player.transform.position;
            
            if(playerPosition.x - transform.position.x > 0)
            {
                currentPossibleMovements.Add(possibleDirections[0]);
            }
            else if(playerPosition.x - transform.position.x < 0)
            {
                currentPossibleMovements.Add(possibleDirections[1]);
            }

            if(playerPosition.y - transform.position.y > 0)
            {
                currentPossibleMovements.Add(possibleDirections[2]);
            }
            else if(playerPosition.y - transform.position.y < 0)
            {
                currentPossibleMovements.Add(possibleDirections[3]);
            }
            
            int randDirection = Random.Range(0, currentPossibleMovements.Count);
            Vector3 possibleNextPosition = transform.position + currentPossibleMovements[randDirection];

            if(Time.time - lastTimeMoved > (movementEverySecs + 1)) //TODO: change to a better planned variable
            {
                int randDirection2 = Random.Range(0, possibleDirections.Length);
                possibleNextPosition = transform.position + possibleDirections[randDirection2];
            }

            if(mapManagerScript.IsWantedPositionLegal(possibleNextPosition))
            {
                _chasingNextPosition = possibleNextPosition;
                lastTimeMoved = Time.time;
            }
            currentPossibleMovements.Clear();
        }

        if(transform.position != _chasingNextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _chasingNextPosition, movingSpeed * Time.deltaTime);
        }
    }

    protected override void SpecificEnemyDeathEffect()
    {
        throw new System.NotImplementedException();
    }
}
