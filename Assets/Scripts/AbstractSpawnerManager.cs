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
    // Start is called before the first frame update
    protected void Start()
    {
        _gameManager = GameObject.Find("GameManager");
        _mapManagerScript = _gameManager.GetComponent<MapManager>();
        GameManager.onCleanupBeforeLevelUp += ClearPrefabsOnMap;
        prefabsOnMap = new List<GameObject>();

        (_leftX, _rightX) = getXMapBounds();
        (_bottomY, _topY) = getYMapBounds();

        SpecificInitializations();
    }

    // Update is called once per frame
    protected void Update()
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

    protected void addToPrefabsOnMap(GameObject spawnedPrefab)
    {
        prefabsOnMap.Add(spawnedPrefab);
    }

    protected void removeFromPrefabsOnMap(GameObject removedPrefab)
    {
        prefabsOnMap.Remove(removedPrefab);  //TODO: will be used when implementing enemy dying and picking up die/weapon 
    }

    public List<GameObject> GetPrefabsOnMap()
    {
        return prefabsOnMap;
    }

    protected (int, int) getXMapBounds()
    {
        return _mapManagerScript.getXMapBounds();
    }

    protected (int, int) getYMapBounds()
    {
        return _mapManagerScript.getYMapBounds();
    }

    protected bool IsSpawningPositionLegal(Vector3 pos)
    {
        return _mapManagerScript.IsWantedPositionLegal(pos);
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
        Debug.Log("destroying the prefabs");
        for(int i = prefabsOnMap.Count - 1; i >= 0; i--)
        {
            GameObject prefabToDestroy = prefabsOnMap[i];
            prefabsOnMap.RemoveAt(i);
            /*TODO: Later to add a proper protected function that each inherited class will
            / implement and remove the prefab. For example special animation for monster dying.*/
            Destroy(prefabToDestroy);
        }
    }

    protected abstract void SpecificInitializations();
    protected abstract bool SpecificShouldSpawnPrefab(); 

    /*
    returns an index to prefabsToSpawn
    */
    protected abstract int ChooseWhichToSpawn();
    protected abstract void Spawn(Vector3 spawningPosition);
}
