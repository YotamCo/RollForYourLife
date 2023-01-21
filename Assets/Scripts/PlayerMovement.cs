using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 playerDircetion = Vector3.forward * 0;
    private ValidPositionChecker validPositionChecker;
    
    void Start()
    {
        validPositionChecker = new ValidPositionChecker();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        Vector3 wantedPosition = gameObject.transform.position;
        
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

        transform.eulerAngles = playerDircetion;

        if(validPositionChecker.IsWantedPositionOutOfBoundsOrWall(wantedPosition))
        {
            
            transform.position = wantedPosition;
            
            //moveTime = _movementTimeDelay;
        }
    }
}
