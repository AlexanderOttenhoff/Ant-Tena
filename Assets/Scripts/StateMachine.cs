using System;
using System.Collections.Generic;

// CurrentState<T>.event is not being used... But it is.
#pragma warning disable 67

/// <summary>
/// Represents a generic state machine. You must create it with the event type has a type parameter.
/// Then you setup your states accordingly and provide it with the initial state.
/// </summary>
/// <typeparam name="T">The event type. For simplicity, make this an enum.</typeparam>
public class StateMachine<T>
{
    /// <summary>
    /// The initial state. On Reset() we revert it to this state.
    /// </summary>
    public State<T> InitialState;

    /// <summary>
    /// The CurrentState.
    /// </summary>
    public State<T> CurrentState;


    /// <summary>
    /// ctor.
    /// Provide the initial state. Its OnStateEntered events will be triggered.
    /// </summary>
    /// <param name="initialState">The initial state.</param>
    public StateMachine(State<T> initialState)
    {
        if (initialState == null)
            throw new ArgumentNullException("The initial state cannot be null");
        InitialState = initialState;
        Reset(false);
    }

    /// <summary>
    /// Reset the state machine to its initial state. The initial state's OnEnterState event will be triggered.
    /// </summary>
    /// <param name="tickStateExit">Whether or not to trigger the OnExitState event of the current event (prior reseting).</param>
    public void Reset(bool tickStateExit)
    {
        if (tickStateExit)
            CurrentState.StateExited();
        CurrentState = InitialState;
        CurrentState.StateEntered();
    }


    /// <summary>
    /// Tick the state machine with a new event. It will transit to a new state if the current state has a transition defined for this
    /// event and the resulting state is different that the current one.
    /// </summary>
    /// <param name="evt">The event to inject into the state machine.</param>
    public void Tick(T evt)
    {
        State<T> nextState;
        if (CurrentState.Transitions.TryGetValue(evt, out nextState) && nextState != CurrentState)
        {
            CurrentState.StateExited();
            CurrentState = nextState;
            CurrentState.StateEntered();
        }
    }
}


/// <summary>
/// Represents a state. It holds a transition table and the diverse events to be called whenever we enter/exit the state.
/// </summary>
/// <typeparam name="T">The type of transition events this state supports.</typeparam>
public class State<T>
{
    public string Name;
    public Dictionary<T, State<T>> Transitions;
    public event Action OnEnterState;
    public event Action OnExitState;

    public State(string name)
    {
        Name = name;
        Transitions = new Dictionary<T, State<T>>();
    }

    public void StateEntered()
    {
        if (OnEnterState != null)
            OnEnterState();
    }

    public void StateExited()
    {
        if (OnExitState != null)
            OnExitState();
    }
}