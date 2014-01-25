using UnityEngine;
using System.Collections;

public class AntNPC : MonoBehaviour
{
    public EventManager EventManager;
    private string PlayerTag = "Player";

    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(PlayerTag))
        {
            EventData data = new EventData{EventType = GameEvent.FoundAnt, SourceGameObj = gameObject};
            EventManager.TriggerEvent(data);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(PlayerTag))
        {
            EventData data = new EventData { EventType = GameEvent.AbandonedAnt, SourceGameObj = gameObject };
            EventManager.TriggerEvent(data);
        }
    }
}
