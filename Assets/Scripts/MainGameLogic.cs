using System;
using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum Level
{
    Level1Section1 = 0,
    Level1Section2 = 1,
    Level1Section3 = 2
}

public class MainGameLogic : MonoBehaviour
{
    public bool PlayIntro;
    public EventManager EventManager;
    public StateMachine<GameEvent> PlayerStateMachine;
    public AntController PlayerAnt;
    public GameManager GameManager;

    public string[] SceneNames;

    private MenuState _menuState;
    private ExplorationState _explorationState;
    private DialogueState _dialogueState;
    private static int _currentLevel;
    private LevelData _currentLevelData;

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

        EventManager.Died += EventManagerOnDied;
        EventManager.SectionEnded += EventManagerOnSectionEnded;
        _currentLevel = Application.loadedLevel;
    }

    private void EventManagerOnSectionEnded(EventData eventdata)
    {
        string level = eventdata.Data as string;
        _currentLevel = Array.IndexOf(SceneNames, level);
        audio.clip = GameManager.missionComplete;
        audio.Play();
        PlayerAnt.enabled = false;
        this.ExecuteAfterSilent(audio, () => Application.LoadLevel(_currentLevel));
    }

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("Level " + level + " loaded. Current level:" + _currentLevel);
        _currentLevelData = GameObject.FindGameObjectWithTag(LevelData.GameObjectTag).GetComponent<LevelData>();
        //Debug.Log("current level data: " + (_currentLevelData ? " ok" : "null"));
        MovePlayer(_currentLevelData.StartPosition.position, _currentLevelData.StartPosition.rotation);
    }

    private void MovePlayer(Vector3 position, Quaternion rotation)
    {
        PlayerAnt.transform.position = position;
        PlayerAnt.transform.rotation = rotation;
    }

    private void EnterMenuState()
    {
        PlayerAnt.enabled = false;
    }

    private void ExitMenuState()
    {
        Debug.Log("Game start");
        PlayerAnt.enabled = true;
        if (PlayIntro)
            this.ExecuteAfter(1f, () => audio.PlayOneShot(GameManager.introsPerLevel[_currentLevel]));

        _currentLevelData = GameObject.FindGameObjectWithTag(LevelData.GameObjectTag).GetComponent<LevelData>();
        MovePlayer(_currentLevelData.StartPosition.position, _currentLevelData.StartPosition.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStateMachine.CurrentState == _menuState)
        {
            PlayerStateMachine.Tick(GameEvent.GameStart);
            //            if (GamePad.GetState(PlayerAnt.playerIndex).Buttons.Start == ButtonState.Pressed ||
            //                Input.GetKey(KeyCode.Space))
            //            {
            //                PlayerStateMachine.Tick(GameEvent.GameStart);
            //            }
        }

        if (PlayerStateMachine.CurrentState == _explorationState)
        {
            CheckJumpToLevel();
            //if (PlayerAnt.transform.position.y < 0)
            //{
            //    PlayerStateMachine.Tick(GameEvent.Died);
            //}
        }
    }

    private void CheckJumpToLevel()
    {
        int level = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            level = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            level = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            level = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            level = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            level = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            level = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            level = 6;
        }

        if (level >= 0)
        {
            Application.LoadLevel(level);
        }
    }

    void OnGUI()
    {
        //GUILayout.Label("State: " + PlayerStateMachine.CurrentState);
        //if (PlayerStateMachine.CurrentState == _menuState)
        //{
        //    float width = 100f;
        //    float height = 50f;
        //    GUILayout.BeginArea(new Rect((Screen.width / 2f) - width, (Screen.height / 2f) - height, width, height));
        //    GUILayout.Label("Press 'Space' or 'Start' to begin");
        //    GUILayout.EndArea();
        //}
    }


    //private void EventManagerOnAbandonedAnt(EventData eventdata)
    //{
    //    PlayerStateMachine.Tick(GameEvent.AbandonedAnt);
    //}

    //private void EventManagerOnFoundAnt(EventData eventdata)
    //{
    //    PlayerStateMachine.Tick(GameEvent.FoundAnt);
    //}

    private void EventManagerOnDied(EventData eventdata)
    {
        PlayerStateMachine.Tick(GameEvent.Died);
    }

    private void EnterDialogueState()
    {
        Debug.Log("found ant");
    }

    private void ExitDialogueState()
    {
        Debug.Log("abandoned ant");
    }
}
