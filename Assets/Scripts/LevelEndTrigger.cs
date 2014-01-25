using UnityEngine;
using System.Collections;

public class LevelEndTrigger : MonoBehaviour
{
    public EventManager EventManager;
    public string goToLevel;
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
        EventData data = new EventData { EventType = GameEvent.SectionEnded, SourceGameObj = gameObject, Data = goToLevel };
        EventManager.TriggerEvent(data);
    }
}
