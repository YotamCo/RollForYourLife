using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    //[SerializeField] private int _height, _width;
    [SerializeField] private Tile tilePrefab; 
    [SerializeField] private GameObject wallPrefab;

    private int [,] _wallLocations; // has 1 where there are walls
    private List<GameObject> wallPrefabs;

    private void Start()
    {
        _wallLocations = new int[13, 8];
        wallPrefabs = new List<GameObject>();
    }

    public void PrepareNewLevelMap(int level)
    {
        ClearWallLocations();
        string subLevelsLocation = "Assets/Levels/Level_" + level.ToString();
        int subLevel = PickRandomSubLevel(level, subLevelsLocation);
        string readText = File.ReadAllText(subLevelsLocation + "/" + level.ToString() + "_" + subLevel.ToString() + ".txt");
        SpawnObstaclesFromText(readText);
    }

    void SpawnObstaclesFromText(string readText)
    {
        (int xSpawnPosition, int ySpawnPosition) = (ConstUtils.LEFT_MAP_X, ConstUtils.TOP_MAP_Y);
        float tileSize = tilePrefab.transform.localScale.x; // it's a square so x = y
        foreach(char c in readText) // can be '0', '1', '\n', ' '
        {
            if(c == '\n')
            {
                xSpawnPosition = ConstUtils.LEFT_MAP_X;
                ySpawnPosition--;
                continue;
            }
            else if(c == '1')
            {
                var wallSpawned = Instantiate(wallPrefab, new Vector3(xSpawnPosition, ySpawnPosition, 0), Quaternion.identity);
                wallPrefabs.Add(wallSpawned);
                (int wallLocationsX, int wallLocationsY) = ParseLocationToWallLocations(xSpawnPosition, ySpawnPosition);
                _wallLocations[wallLocationsX, wallLocationsY] = 1;
            }
            else if(c == ' ')
                xSpawnPosition++;
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

        int n = wallPrefabs.Count;
        for(int i = n - 1; i >= 0; i--)
        {
            GameObject wallPrefab = wallPrefabs[i];
            wallPrefabs.RemoveAt(i);
            //TODO: add animation of walls breaking?
            Destroy(wallPrefab);
        }
    }

    int PickRandomSubLevel(int level, string subLevelsLocation)
    {
        int numOfSubLevels = (Directory.GetFiles(subLevelsLocation, "*", SearchOption.TopDirectoryOnly).Length) / 2;
        int subLevel = Random.Range(1, numOfSubLevels + 1);
        return subLevel;
    }

    public bool DoesPositionHasWall(Vector3 position)
    {
        (int wallLocationsX, int wallLocationsY) = ParseLocationToWallLocations((int)position.x, (int)position.y);
        if(_wallLocations[wallLocationsX, wallLocationsY] == 1)
        {
            return true;
        }
        return false;
    }

    (int, int) ParseLocationToWallLocations(int x, int y)
    {
        return (x + 6, y + 4);
    }
}
