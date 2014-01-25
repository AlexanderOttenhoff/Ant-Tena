using System;
using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MainGameLogic : MonoBehaviour
{
    public EventManager EventManager;
    public StateMachine<GameEvent> PlayerStateMachine;
    public AntController PlayerAnt;

    private MenuState _menuState;
    private ExplorationState _explorationState;
    private DialogueState _dialogueState;

    // Use this for initialization
    void Start()
    {
        _menuState = new MenuState();
        _explorationState = new ExplorationState();
        _dialogueState = new DialogueState();

        _menuState.Transitions.Add(GameEvent.GameStart, _explorationState);// game start: menu -> exploration
        _explorationState.Transitions.Add(GameEvent.FoundAnt, _dialogueState); // found ant: exploration -> dialogue
        _explorationState.Transitions.Add(GameEvent.Died, _menuState); // died: exploration -> menu
        _dialogueState.Transitions.Add(GameEvent.AbandonedAnt, _explorationState); // left ant: dialogue -> exploration
        _dialogueState.Transitions.Add(GameEvent.DialogueSuccess, _explorationState); // dialogue success: dialogue -> exploration

        PlayerStateMachine = new StateMachine<GameEvent>(_menuState);

        EventManager.FoundAnt += EventManagerOnFoundAnt;
        EventManager.AbandonedAnt += EventManagerOnAbandonedAnt;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStateMachine.CurrentState == _menuState)
        {
            if (GamePad.GetState(PlayerAnt.playerIndex).Buttons.Start == ButtonState.Pressed ||
                Input.GetKey(KeyCode.Space))
            {
                PlayerStateMachine.Tick(GameEvent.GameStart);
            }
        }

        if (PlayerStateMachine.CurrentState == _explorationState)
        {
            // play natural sounds
        }
        else if (PlayerStateMachine.CurrentState == _dialogueState)
        {
            // read sound input
            // play ant sounds
        }
    }

    private void EventManagerOnAbandonedAnt(EventData eventdata)
    {
        Debug.Log("abandoned ant");
        PlayerStateMachine.Tick(GameEvent.AbandonedAnt);
    }

    private void EventManagerOnFoundAnt(EventData eventdata)
    {
        Debug.Log("found ant");
        PlayerStateMachine.Tick(GameEvent.FoundAnt);
    }
}
