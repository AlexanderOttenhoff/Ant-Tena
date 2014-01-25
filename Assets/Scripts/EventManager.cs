using System;
using UnityEngine;
using System.Collections;

public struct EventData
{
    public GameEvent EventType;
    public Vector3 Position;
    public GameObject SourceGameObj;
}

public class EventManager : MonoBehaviour
{

    public delegate void GameEventHandler(EventData eventdata);

    public event GameEventHandler AbandonedAnt;
    public event GameEventHandler FoundAnt;
    public event GameEventHandler Died;
    public event GameEventHandler DialogueSucceeded;
    public event GameEventHandler DialogueFailed;
    public event GameEventHandler SectionEnded;



    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerEvent(EventData evt)
    {
        switch (evt.EventType)
        {
            case GameEvent.FoundAnt:
                OnFoundAnt(evt);
                break;
            case GameEvent.DialogueSuccess:
                OnDialogueSucceeded(evt);
                break;
            case GameEvent.DialogueFailed:
                OnDialogueFailed(evt);
                break;
            case GameEvent.Died:
                OnDied(evt);
                break;
            case GameEvent.AbandonedAnt:
                OnAbandonedAnt(evt);
                break;
            case GameEvent.SectionEnded:
                OnSectionEnded(evt);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual void OnFoundAnt(EventData eventdata)
    {
        GameEventHandler handler = FoundAnt;
        if (handler != null) handler(eventdata);
    }

    protected virtual void OnDied(EventData eventdata)
    {
        GameEventHandler handler = Died;
        if (handler != null) handler(eventdata);
    }

    protected virtual void OnDialogueSucceeded(EventData eventdata)
    {
        GameEventHandler handler = DialogueSucceeded;
        if (handler != null) handler(eventdata);
    }

    protected virtual void OnDialogueFailed(EventData eventdata)
    {
        GameEventHandler handler = DialogueFailed;
        if (handler != null) handler(eventdata);
    }

    protected virtual void OnAbandonedAnt(EventData eventdata)
    {
        GameEventHandler handler = AbandonedAnt;
        if (handler != null) handler(eventdata);
    }

    protected virtual void OnSectionEnded(EventData eventdata)
    {
        GameEventHandler handler = SectionEnded;
        if (handler != null) handler(eventdata);
    }
}
