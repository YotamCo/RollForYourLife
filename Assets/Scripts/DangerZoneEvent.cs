using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneEvent : AbstractEvent
{

    private List<Vector3> _dangerZoneLocations;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TriggerEvent()
    {
        ChooseRandomTilesOnMap();
        SpawnDangerZones();
        ClearDangerZoneLocations();
    }

    private void ChooseRandomTilesOnMap()
    {
        // Check how it's done for spawning of other prefabs: enemy, die, weapon item...
        // I need to make a utility class with this function because I use it in multiple places
    }

    private void SpawnDangerZones()
    {
        // Either first spawn warning prefab and then the actual damage zone
        // Or do a warning animation of the danger zone and spawn the danger zone.
        // Can use particles
    }

    private void ClearDangerZoneLocations()
    {
        int numOfDangerZoneTiles = _dangerZoneLocations.Count;
        for(int i = numOfDangerZoneTiles - 1; i == 0; i--)
        {
            _dangerZoneLocations.RemoveAt(i);
        }
    }
}
