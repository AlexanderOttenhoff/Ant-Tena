using UnityEngine;
using System.Collections;

public class RunningAnt : MonoBehaviour
{
    private enum State { Running, Calling, Stopped };

    public Transform Target;
    public float PlayerTooCloseDistance;
    public float PlayerTooFarDistance;
    public float ReachWaypointDistance;
    public Transform[] Waypoints;

    public AudioSource WalkingClip;
    public AudioSource ChirpClip;

    public float ChirpCoolDownTime;

    private NavMeshAgent NavAgent;
    private Transform Player;
    private State _currentState;

    private int _nextWaypoint;
    private float _defaultSpeed;
    private bool _chirpCooldown;

    // Use this for initialization
    void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _currentState = State.Calling;
        WalkingClip = GetComponent<AudioSource>();
        _defaultSpeed = NavAgent.speed;
        NavAgent.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == State.Calling)
        {
            CallingStateUpdate();
        }
        else if (_currentState == State.Running)
        {
            RunningStateUpdate();
        }
    }

    private void RunningStateUpdate()
    {
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance > PlayerTooFarDistance)
        {
            ToCallingState();
        }
        else
        {
            if (!WalkingClip.isPlaying)
            {
                WalkingClip.Play();
            }
            NavAgent.SetDestination(Waypoints[_nextWaypoint].position);
            NavAgent.speed = _defaultSpeed;
            if (Vector3.Distance(transform.position, NavAgent.destination) < ReachWaypointDistance)
            {
                if (_nextWaypoint + 1 >= Waypoints.Length)
                {
                    Debug.Log("Now I am stopped");
                    _currentState = State.Stopped;
                    ChirpClip.Stop();
                    WalkingClip.Stop();
                }
                else
                {
                    _nextWaypoint = _nextWaypoint + 1;
                }
            }
        }
    }

    private void ToCallingState()
    {
        Debug.Log("Now I am calling");
        _currentState = State.Calling;
        NavAgent.speed = 0;
        WalkingClip.Stop();
    }

    private void CallingStateUpdate()
    {
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance < PlayerTooCloseDistance)
        {
            Debug.Log("Now I am running");
            _currentState = State.Running;
            ChirpClip.Stop();
        }
        else
        {
            if (!ChirpClip.isPlaying && !_chirpCooldown)
            {
                _chirpCooldown = true;
                this.ExecuteAfter(ChirpCoolDownTime,
                    () =>{  ChirpClip.Play();   _chirpCooldown = false; });
            }
        }
    }
}
