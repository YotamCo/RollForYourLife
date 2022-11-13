using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //[SerializeField] private int _height, _width;
    [SerializeField] private Tile tilePrefab; 
    [SerializeField] private GameObject wallPrefab;

    private (int, int) _topLeft = (-6, 3);
    private (int, int) _topRight = (6, 3);
    private (int, int) _bottomLeft = (-6, -4);
    private (int, int) _bottomRight = (6, -4);

    private int [,] _wallLocations; // has 1 where there are walls

    // start is called before the first frame update
    void Start()
    {
        _wallLocations = new int[13, 8];
    }

    void Update()
    {
    }

    public void PrepareNewLevelObstacles(int level)
    {
        ClearWallLocations();
        string subLevelsLocation = "Assets/Levels/Level_" + level.ToString();
        int subLevel = PickRandomSubLevel(level, subLevelsLocation);
        string readText = File.ReadAllText(subLevelsLocation + "/" + level.ToString() + "_" + subLevel.ToString() + ".txt");
        SpawnObstaclesFromText(readText);
    }

    void SpawnObstaclesFromText(string readText)
    {
        (int x, int y) = _topLeft;
        float tileSize = tilePrefab.transform.localScale.x; // it's a square so x = y
        foreach(char c in readText) // can be '0', '1', '\n', ' '
        {
            if(c == '\n')
            {
                x = _topLeft.Item1;
                y--;
                continue;
            }
            else if(c == '1')
            {
                var wallSpawned = Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                (int wallLocationsX, int wallLocationsY) = ParseLocationToWallLocations(x, y);
                _wallLocations[wallLocationsX, wallLocationsY] = 1;
            }
            else if(c == ' ')
                x++;
        }
    }

    void ClearWallLocations()
    {
        for(int i = 0; i < _wallLocations.GetUpperBound(0); i++)
        {
            for(int j = 0; j < _wallLocations.GetUpperBound(1); j++)
            {
                _wallLocations[i, j] = 0;
            }
        }
    }

    int PickRandomSubLevel(int level, string subLevelsLocation)
    {
        int numOfSubLevels = (Directory.GetFiles(subLevelsLocation, "*", SearchOption.TopDirectoryOnly).Length) / 2;
        int subLevel = Random.Range(1, numOfSubLevels + 1);
        Debug.Log("numOfSubLevels: " + numOfSubLevels + "   Level loaded is " + level.ToString() + "_" + subLevel.ToString());
        return subLevel;
    }

    bool IsInMapBounds(Vector3 position)
    {
        if(position.x < _topLeft.Item1 || position.x > _topRight.Item1 
        || position.y < _bottomLeft.Item2 || position.y > _topLeft.Item2)
            return false;
        return true;
    }

    public bool IsWantedPositionLegal(Vector3 position)
    {
        if(IsInMapBounds(position) && !DoesPositionHasWall(position))
            return true;
        return false;
    }

    bool DoesPositionHasWall(Vector3 position)
    {
        (int wallLocationsX, int wallLocationsY) = ParseLocationToWallLocations((int)position.x, (int)position.y);
        if(_wallLocations[wallLocationsX, wallLocationsY] == 1)
            return true;
        return false;
    }

    (int, int) ParseLocationToWallLocations(int x, int y)
    {
        return (x + 6, y + 4);
    }
}
