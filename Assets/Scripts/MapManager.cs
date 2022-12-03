using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    enum SpawnerScriptsIndex
    {
        ENEMY_SPAWNER = 0,
        DIE_SPAWNER = 1,
        WEAPON_ITEM_SPAWNER = 2
    }

    //[SerializeField] private int _height, _width;
    [SerializeField] private Tile tilePrefab; 
    [SerializeField] private GameObject wallPrefab;

    private int _leftX = -6;
    private int _rightX = 6;
    private int _bottomY = -4;
    private int _topY = 3;

    private int [,] _wallLocations; // has 1 where there are walls

    private GameObject _player;
    private PlayerMovement _playerMovementScript;
    private DieSpawner _dieSpawnerScript;
    private EnemySpawner _enemySpawnerScript;

    private List<AbstractSpawnManager> _spawnerScripts;

    // start is called before the first frame update
    void Start()
    {
        _wallLocations = new int[13, 8];

        _player                 = GameObject.Find("Player");
        _dieSpawnerScript       = gameObject.GetComponent<DieSpawner>();
        _enemySpawnerScript     = gameObject.GetComponent<EnemySpawner>();
        InitializeSpawnerScripts();
    }

    private void InitializeSpawnerScripts()
    {
        _spawnerScripts = new List<AbstractSpawnManager>();
        _spawnerScripts.Add(gameObject.GetComponent<EnemySpawner>());
        _spawnerScripts.Add(gameObject.GetComponent<DieSpawner>());
        _spawnerScripts.Add(gameObject.GetComponent<WeaponItemSpawner>());
    }

    public (int, int) getXMapBounds()
    {
        return (_leftX, _rightX);
    }

    public (int, int) getYMapBounds()
    {
        return (_bottomY, _topY);
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
        (int x, int y) = (_leftX, _topY);
        float tileSize = tilePrefab.transform.localScale.x; // it's a square so x = y
        foreach(char c in readText) // can be '0', '1', '\n', ' '
        {
            if(c == '\n')
            {
                x = _leftX;
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
        return subLevel;
    }

    public bool IsWantedPositionOutOfBoundsOrWall(Vector3 position)
    {
        if(IsInMapBounds(position) && !DoesPositionHasWall(position))
            return true;
        return false;
    }

    public bool IsSpawningPositionLegal(Vector3 position)
    {
        if(IsInMapBounds(position) 
            && !DoesPositionHasWall(position)
            && !IsSameAsPlayerPosition(position)
            && !IsSameAsPrefabPosition(position, (int)SpawnerScriptsIndex.ENEMY_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)SpawnerScriptsIndex.DIE_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)SpawnerScriptsIndex.WEAPON_ITEM_SPAWNER))
            return true;
        return false;
    }

    public bool IsMovementPositionLegal(Vector3 position) //TODO: IsSameAsWeaponItemPosition is not implemented yet
    {
        if(IsInMapBounds(position) 
            && !DoesPositionHasWall(position)
            && !IsSameAsPrefabPosition(position, (int)SpawnerScriptsIndex.ENEMY_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)SpawnerScriptsIndex.DIE_SPAWNER)
            && !IsSameAsPrefabPosition(position, (int)SpawnerScriptsIndex.WEAPON_ITEM_SPAWNER))
            return true;
        return false;
    }

    public bool IsInMapBounds(Vector3 position)
    {
        if(position.x < _leftX || position.x > _rightX
            || position.y < _bottomY || position.y > _topY)
            return false;
        return true;
    }
    bool DoesPositionHasWall(Vector3 position)
    {
        (int wallLocationsX, int wallLocationsY) = ParseLocationToWallLocations((int)position.x, (int)position.y);
        if(_wallLocations[wallLocationsX, wallLocationsY] == 1)
        {
            return true;
        }
        return false;
    }

    private bool IsSameAsPlayerPosition(Vector3 pos)
    {
        if(_player == null) //TODO: !! Check how to check if a script is null or inactive
        {
            Debug.LogWarning("Player object does not exist");
            return false;
        }
        Vector3 playerPosition = _player.transform.position;
        if(playerPosition.x == pos.x && playerPosition.y == pos.y)
            return true;
        return false;
    }

    private bool IsSameAsEnemiesPosition(Vector3 pos)
    {
        if(_enemySpawnerScript.enabled == false)
        {
            Debug.LogWarning("EnemySpawner script is disabled");
            return false;
        }
        List<GameObject> enemiesInMap = _enemySpawnerScript.GetPrefabsOnMap();
        foreach(GameObject enemy in enemiesInMap)
        {
            if(enemy.transform.position.x == pos.x && enemy.transform.position.y == pos.y)
                return true;
        }
        return false;
    }

    private bool IsSameAsWeaponItemPosition(Vector3 pos)
    {
        return false; //TODO: Need to implement
    }

    private bool IsSameAsDicePosition(Vector3 pos)
    {
        if(_dieSpawnerScript.enabled == false)
        {
            Debug.LogWarning("DieSpawner script is disabled");
            return false;
        }
        List<GameObject> diceInMap = _dieSpawnerScript.GetPrefabsOnMap();
        foreach(GameObject die in diceInMap)
        {
            if(die.transform.position.x == pos.x && die.transform.position.y == pos.y)
                return true;
        }
        return false;
    }

    private bool IsSameAsPrefabPosition(Vector3 pos, int prefabScriptIndex)
    {
        if(_spawnerScripts[prefabScriptIndex].enabled == false)
        {
            Debug.LogWarning("Spawner script is disabled");
            return false;
        }
        List<GameObject> prefabInMap = _spawnerScripts[prefabScriptIndex].GetPrefabsOnMap();
        foreach(GameObject prefab in prefabInMap)
        {
            if(prefab.transform.position.x == pos.x && prefab.transform.position.y == pos.y)
                return true;
        }
        return false;
    }
    

    (int, int) ParseLocationToWallLocations(int x, int y)
    {
        return (x + 6, y + 4);
    }


}
