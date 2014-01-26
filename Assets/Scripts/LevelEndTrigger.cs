using UnityEngine;
using System.Collections;

public class LevelEndTrigger : MonoBehaviour
{
    public EventManager EventManager;
    public string Data;
    public GameEvent Event;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventData data = new EventData { EventType = Event, SourceGameObj = gameObject, Data = Data };
            EventManager.TriggerEvent(data);    
        }
        
    }
}
