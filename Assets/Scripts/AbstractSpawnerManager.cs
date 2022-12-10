using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawnManager : MonoBehaviour
{
    protected GameObject _gameManager;
    protected MapManager _mapManagerScript;
    protected int _leftX;
    protected int _rightX;
    protected int _bottomY;
    protected int _topY;
    protected bool _shouldSpawnPrefab = false;
    [SerializeField] protected GameObject[] prefabsToSpawn;
    protected List<GameObject> prefabsOnMap;
    protected bool _spawningEnabled = true;

    // Start is called before the first frame update
    protected void Start()
    {
        _gameManager = GameObject.Find("GameManager");
        _mapManagerScript = _gameManager.GetComponent<MapManager>();
        LevelTransitionManager.onCleanupBeforeLevelUp += ClearPrefabsOnMap;
        prefabsOnMap = new List<GameObject>();

        (_leftX, _rightX) = GetXMapBounds();
        (_bottomY, _topY) = GetYMapBounds();

        SpecificInitializations();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(_spawningEnabled)
        {
            if(_shouldSpawnPrefab || ShouldSpawnPrefab())
            {
                Vector3 spawnPosition = ChooseSpawningPosition();
                if(IsSpawningPositionLegal(spawnPosition))
                {
                    Spawn(spawnPosition);
                    _shouldSpawnPrefab = false;
                }
            }
        }
    }

    protected void AddToPrefabsOnMap(GameObject spawnedPrefab)
    {
        prefabsOnMap.Add(spawnedPrefab);
    }

    protected void RemoveFromPrefabsOnMap(GameObject removedPrefab)
    {
        prefabsOnMap.Remove(removedPrefab);
    }

    public List<GameObject> GetPrefabsOnMap()
    {
        return prefabsOnMap;
    }

    protected (int, int) GetXMapBounds()
    {
        return _mapManagerScript.getXMapBounds();
    }

    protected (int, int) GetYMapBounds()
    {
        return _mapManagerScript.getYMapBounds();
    }

    protected bool IsSpawningPositionLegal(Vector3 pos)
    {
        return _mapManagerScript.IsSpawningPositionLegal(pos);
    }

    protected Vector3 ChooseSpawningPosition()
    {
        int xPos = Random.Range(_leftX, _rightX + 1);
        int yPos = Random.Range(_bottomY, _topY + 1);
        return new Vector3(xPos, yPos, 0);
    }

    protected bool ShouldSpawnPrefab()
    {
        if(SpecificShouldSpawnPrefab())
        {
            _shouldSpawnPrefab = true;
            return true;
        }
        return false;
    }

    protected void ClearPrefabsOnMap()
    {
        for(int i = prefabsOnMap.Count - 1; i >= 0; i--)
        {
            GameObject prefabToDestroy = prefabsOnMap[i];
            DestroyPrefab(prefabToDestroy);
        }
    }

    public void ChangeSpawningStatus(bool isSetToSpawning)
    {
        _spawningEnabled = isSetToSpawning;
    }

    protected abstract void SpecificInitializations();
    protected abstract bool SpecificShouldSpawnPrefab(); 
    protected abstract int ChooseWhichToSpawn();
    protected abstract void Spawn(Vector3 spawningPosition);
    protected abstract void DestroyPrefab(GameObject prefabObject);
}
