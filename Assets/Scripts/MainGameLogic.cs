using System;
using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MainGameLogic : MonoBehaviour
{
    public EventManager EventManager;
    public StateMachine<GameEvent> PlayerStateMachine;
    public AntController PlayerAnt;
    public GameManager GameManager;

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
        _menuState.OnExitState += ExitMenuState;
        _menuState.OnEnterState += EnterMenuState;

        _explorationState.Transitions.Add(GameEvent.FoundAnt, _dialogueState); // found ant: exploration -> dialogue
        _explorationState.Transitions.Add(GameEvent.Died, _menuState); // died: exploration -> menu

        _dialogueState.OnEnterState += EnterDialogueState;
        _dialogueState.OnExitState += ExitDialogueState;
        _dialogueState.Transitions.Add(GameEvent.AbandonedAnt, _explorationState); // left ant: dialogue -> exploration
        _dialogueState.Transitions.Add(GameEvent.DialogueSuccess, _explorationState); // dialogue success: dialogue -> exploration

        PlayerStateMachine = new StateMachine<GameEvent>(_menuState);
        
        EventManager.FoundAnt += EventManagerOnFoundAnt;
        EventManager.AbandonedAnt += EventManagerOnAbandonedAnt;
        EventManager.Died += EventManagerOnDied;
        
    }

    private void EnterMenuState()
    {
        PlayerAnt.enabled = false;
        // <FOR DEBUGGING>
        PlayerAnt.GetComponent<FPSInputController>().enabled = false;
        PlayerAnt.GetComponentInChildren<MouseLook>().enabled = false;
        // </FOR DEBUGGING>
    }

    private void ExitMenuState()
    {
        Debug.Log("Game start");
        PlayerAnt.enabled = true;
        // <FOR DEBUGGING>
        PlayerAnt.GetComponent<FPSInputController>().enabled = true;
        PlayerAnt.GetComponentInChildren<MouseLook>().enabled = true;
        // </FOR DEBUGGING>
        this.ExecuteAfter(1f, () => GameManager.IntroSound.Play());
        PlayerAnt.transform.position = GameManager.StartPosition.position;
    }

    private void EnterDialogueState()
    {
        Debug.Log("found ant");
    }

    private void ExitDialogueState()
    {
        Debug.Log("abandoned ant");
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
            if (PlayerAnt.transform.position.y < 0)
            {
                PlayerStateMachine.Tick(GameEvent.Died);
            }
            // play natural sounds
        }
        else if (PlayerStateMachine.CurrentState == _dialogueState)
        {
            // read sound input
            // play ant sounds
        }
    }

    void OnGUI()
    {
        GUILayout.Label("State: " + PlayerStateMachine.CurrentState);
        if (PlayerStateMachine.CurrentState == _menuState)
        {
            float width = 100f;
            float height = 50f;
            GUILayout.BeginArea(new Rect((Screen.width / 2f) - width, (Screen.height / 2f) - height, width, height));
            GUILayout.Label("Press 'Space' or 'Start' to begin");
            GUILayout.EndArea();
        }
    }


    private void EventManagerOnAbandonedAnt(EventData eventdata)
    {
        PlayerStateMachine.Tick(GameEvent.AbandonedAnt);
    }

    private void EventManagerOnFoundAnt(EventData eventdata)
    {
        PlayerStateMachine.Tick(GameEvent.FoundAnt);
    }

    private void EventManagerOnDied(EventData eventdata)
    {
        PlayerStateMachine.Tick(GameEvent.Died);
    }
}
