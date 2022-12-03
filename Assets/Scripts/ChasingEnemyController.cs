using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemyController : BlindEnemyController
{
    [SerializeField] float _timeUntilDefinedAsStuck = 1f;

    private List<Vector3> currentPossibleMovements;
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
        if(_player != null) //TODO: Can add here an invisibility option. If player is invisible then MoveRandomly()
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

    private void MoveChasingPlayer()
    {
        if(Time.time - lastTimeMoved > movementEverySecs)
        {
            Vector3 playerPosition = _player.transform.position;
            AddCurrentPossibleMovements(playerPosition);
            
            Vector3 possibleNextPosition = transform.position;
            bool enemyIsInPlayerPosition = currentPossibleMovements.Count == 0;

            if(!enemyIsInPlayerPosition)
            {
                int randDirection = Random.Range(0, currentPossibleMovements.Count);
                possibleNextPosition += currentPossibleMovements[randDirection];
            }
            
            bool hasntMovedBecauseStucked = Time.time - lastTimeMoved > (movementEverySecs + _timeUntilDefinedAsStuck);
            if(hasntMovedBecauseStucked)
            {
                int randDirection2 = Random.Range(0, possibleDirections.Length);
                possibleNextPosition = transform.position + possibleDirections[randDirection2];
            }

            if(mapManagerScript.IsMovementPositionLegal(possibleNextPosition))
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

    private void AddCurrentPossibleMovements(Vector3 playerPosition)
    {
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
    }

    protected override void SpecificEnemyDeathEffect()
    {
        GameObject effect = Instantiate(_deathEffectPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(effect, 1);
    }
}
