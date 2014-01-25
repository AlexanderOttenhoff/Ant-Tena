using UnityEngine;
using System.Collections;

public class LevelEndTrigger : MonoBehaviour
{
    public EventManager EventManager;
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
        EventData data = new EventData{EventType = GameEvent.SectionEnded, SourceGameObj = gameObject};
        EventManager.TriggerEvent(data);
    }
}
