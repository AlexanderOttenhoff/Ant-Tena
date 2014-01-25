using UnityEngine;
using System.Collections;

public enum GameEvent
{
    GameStart,
    FoundAnt,
    AbandonedAnt,
    DialogueSuccess,
    DialogueFailed,
    Died
}

public class DialogueState : State<GameEvent>
{
    public DialogueState() : base("dialogue") { }
}

public class ExplorationState : State<GameEvent>
{
    public ExplorationState() : base("explore") { }
}

public class MenuState : State<GameEvent>
{
    public MenuState() : base("menu") { }
}

public class CutSceneState : State<GameEvent>
{
    public CutSceneState() : base("cutscene") { }
}