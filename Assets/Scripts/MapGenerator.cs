using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //[SerializeField] private int _height, _width;
    [SerializeField] private Tile tilePrefab; 
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int initial_x = -6;
    [SerializeField] private int initial_y = 3;

    // start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnNewLevelObstacles(int level)
    {
        string subLevelsLocation = "Assets/Levels/Level_" + level.ToString();
        int numOfSubLevels = (Directory.GetFiles(subLevelsLocation, "*", SearchOption.TopDirectoryOnly).Length) / 2;
        int subLevel = Random.Range(1, numOfSubLevels + 1);
        Debug.Log("numOfSubLevels: " + numOfSubLevels + "   Level loaded is " + level.ToString() + "_" + subLevel.ToString());
        string readText = File.ReadAllText(subLevelsLocation + "/" + level.ToString() + "_" + subLevel.ToString() + ".txt");
        SpawnObstaclesFromText(readText);
    }

    void SpawnObstaclesFromText(string readText)
    {
        int x = initial_x, y = initial_y;
        float tileSize = tilePrefab.transform.localScale.x; // it's a square so x = y
        foreach(char c in readText) // can be '0', '1', '\n', ' '
        {
            if(c == '\n')
            {
                x = initial_x;
                y--;
                continue;
            }
            else if(c == '1')
            {
                var wallSpawned =  Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
            else if(c == ' ')
            x++;
        }
    }
}
