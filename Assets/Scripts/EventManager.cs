using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{   
    //TODO: I feel like I can use a ScriptableObject here
    List<AbstractEvent> _dieRollEvents;

    // Start is called before the first frame update
    void Start()
    {
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        _dieRollEvents = new List<AbstractEvent>();

        for(int i = 0; i < transform.childCount; i++)
        {
             GameObject dieRollEventObject = gameObject.transform.GetChild(i).gameObject;
             _dieRollEvents.Add(dieRollEventObject.GetComponent<AbstractEvent>());
        }

        ShuffleEventOrder();
    }

    private void ShuffleEventOrder()
    {
        //TODO: implement later: Take _dieRollEvents and pick random 6 events from it
    }

    public void TriggerEventBasedOnRollScore(int rollScore)
    {
        /*if(rollScore >= _dieRollEvents.Count)
        {
            return;
        }
        _dieRollEvents[rollScore].TriggerEvent();*/
    }
}
