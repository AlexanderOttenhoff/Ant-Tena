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

    private bool _waiting;
    private State CurrentState;

    // Use this for initialization
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _waiting = true;
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
                    Sound.Play();
                    _navAgent.SetDestination(Destination.position);
                    CurrentState = State.Running;
                }
                break;
            case State.Running:
                if (!Sound.isPlaying)
                    Sound.Play();
                if (Vector3.Distance(transform.position, Destination.position) < 1f)
                    CurrentState = State.Waiting;
                break;
            case State.Waiting:
                if (!Sound.isPlaying)
                    Sound.Play();
                if (Vector3.Distance(transform.position, _player.position) < TriggerDistance/2f && SelfDestruct)
                    this.ExecuteAfterSilent(Sound, () => Destroy(gameObject));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (!_waiting)
        {


            if (Vector3.Distance(transform.position, Destination.position) < 1f)
            {
                Debug.Log("arrived");
                //Sound.Stop();

            }
        }
    }

    private IEnumerator DestroyWhenSilent()
    {
        if (Sound.isPlaying)
            yield return 0;
        Destroy(gameObject);
    }
}
