using System;
using UnityEngine;
using System.Collections;

public class DistractAnt : MonoBehaviour
{
    private enum State
    {
        Hidden,
        Running,
        Waiting
    }

    public Transform Destination;
    public float TriggerDistance;
    public AudioSource Sound;
    public bool SelfDestruct;

    private NavMeshAgent _navAgent;
    private Transform _player;

    private bool playSound;
    private State CurrentState;

    // Use this for initialization
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        CurrentState = State.Hidden;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case State.Hidden:
                if (Vector3.Distance(transform.position, _player.position) < TriggerDistance)
                {
                    // play sound and move
                    _navAgent.SetDestination(Destination.position);
                    CurrentState = State.Running;
                    playSound = true;
                }
                break;
            case State.Running:
                if (Vector3.Distance(transform.position, Destination.position) < 1f)
                    CurrentState = State.Waiting;
                break;
            case State.Waiting:
                if (Vector3.Distance(transform.position, _player.position) < TriggerDistance/2f && SelfDestruct)
                {
                    playSound = false;
                    this.ExecuteAfterSilent(Sound, () => Destroy(gameObject));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (playSound && !Sound.isPlaying)
        {
            Sound.Play();
        }
    }

    private IEnumerator DestroyWhenSilent()
    {
        if (Sound.isPlaying)
            yield return 0;
        Destroy(gameObject);
    }
}
