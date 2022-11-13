using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject gameManager;
    MapManager mapManagerScript;
    // Start is called before the first frame update
    Vector3 playerDircetion = Vector3.forward * 0;
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        mapManagerScript = gameManager.GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 wantedPosition = transform.position;
        
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

        if(mapManagerScript.IsWantedPositionLegal(wantedPosition))
        {
            
            transform.position = wantedPosition;
            
            //moveTime = _movementTimeDelay;
        }
    }
}
