using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int level = 1;

    GameObject mapGenerator;
    MapGenerator mapGeneratorScript;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = GameObject.Find("MapGenerator");
        mapGeneratorScript =  mapGenerator.GetComponent<MapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelUp()
    {
        level++;
        mapGeneratorScript.SpawnNewLevelObstacles(level);
    }
}
