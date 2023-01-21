using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSmooth : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float _movementTimeDelay = 0.1f;
    private float moveTime;
    ValidPositionChecker validPositionChecker;
    // Start is called before the first frame update
    Vector3 playerDircetion = Vector3.forward * 0;
    Vector3 wantedPosition;
    void Start()
    {
        validPositionChecker = new ValidPositionChecker();

        wantedPosition = transform.position;

        moveTime = _movementTimeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime -= Time.deltaTime;
           
        MovePlayer();
    }

    void MovePlayer()
    {
        transform.eulerAngles = playerDircetion;

        //if(mapManagerScript.IsWantedPositionLegal(wantedPosition))
        //{
            
            //transform.position = wantedPosition;
            if(transform.position != wantedPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, wantedPosition, movementSpeed * Time.deltaTime);
                moveTime = _movementTimeDelay;
            }
            //moveTime = _movementTimeDelay;
        //}
        if (moveTime < 0)
        {
            if (Input.GetKeyDown("up"))
            {
                wantedPosition += new Vector3(0, 1, 0);
                playerDircetion = Vector3.forward * 90;
            }
            else if (Input.GetKeyDown("down"))
            {
                wantedPosition += new Vector3(0, -1, 0);
                playerDircetion = Vector3.forward * 270;
            }
            else if (Input.GetKeyDown("right"))
            {
                wantedPosition += new Vector3(1, 0, 0);
                playerDircetion = Vector3.forward * 0;
            }
            else if (Input.GetKeyDown("left"))
            {
                wantedPosition += new Vector3(-1, 0, 0);
                playerDircetion = Vector3.forward * 180;
            }

            if(!validPositionChecker.IsMovementPositionLegal(wantedPosition))
            {
                wantedPosition = transform.position;
            }
        }

        

        
    }
}
