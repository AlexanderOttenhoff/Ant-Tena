using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntStateMachine : MonoBehaviour
{

    public GameManager manager;
    public enum State { Lost, Calling, Following, Safe }

    public State currentState = State.Lost;

    //Variables for Calling State
    public int numberOfCalls = 3;
    public int callNumber = 0;
    public float callInterval = 5;
    public float punishmentTime = 1;
    public AudioClip[] clipSequence;
    public AudioClip victory;
    public AudioClip[] playerChoices;

    //Variables for Following State
    public Transform target;
    public float moveSpeed = 200f;
    public float rotationSpeed = 100f;
    public float minDistanceToPlayer = 10f;

    private AudioSource audioSource;
    private VibrationSource vibrationSource;
    private bool isPlaying = false;

    void Start()
    {
        RandomizeAudioClips();
        audioSource = GetComponent<AudioSource>();
        vibrationSource = GetComponent<VibrationSource>();
    }

    public void TestAudioClip(AudioClip playerChoice)
    {
        if (currentState == State.Following) return;

        if (callNumber < numberOfCalls)
        {
            playerChoices[callNumber] = playerChoice;
            callNumber++;
        }
        if (callNumber == numberOfCalls)
        {
            bool success = true;
            for (int i = 0; i < numberOfCalls; i++)
            {
                if (playerChoices[i] != clipSequence[i])
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                Debug.Log("player correct");
                // test here, otherwise store and increment
                currentState = State.Following;
                target = GameObject.FindWithTag("Player").transform;
                audioSource.Stop();
                audioSource.PlayOneShot(victory);
            }
            else
            {
                Debug.Log("player wrong");
                callNumber = 0;
                StartCoroutine(PunishPlayer());
                RandomizeAudioClips();
            }
        }
    }

    private void RandomizeAudioClips()
    {
        clipSequence = new AudioClip[numberOfCalls + 1];
        for (int i = 0; i < numberOfCalls; i++)
        {
            clipSequence[i] = manager.antClips[Random.Range(0, manager.antClips.Count)];
        }
        clipSequence[numberOfCalls] = victory;
        playerChoices = new AudioClip[numberOfCalls];
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Lost:
                if (!audio.isPlaying)
                {
                    this.ExecuteAfter(Random.Range(1, 3), () => audio.Play());
                }
                break;
            case State.Calling:
                if (!isPlaying)
                {
                    StartCoroutine(SoundLoop());
                }
                break;
            case State.Following:
                Vector3 moveVector = target.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector), rotationSpeed * Time.deltaTime);
                if (moveVector.magnitude >= minDistanceToPlayer)
                {
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                }
                break;
            case State.Safe:
                break;
        }
    }

    IEnumerator SoundLoop()
    {
        for (; ; )
        {
            isPlaying = true;
            if (callNumber < numberOfCalls && currentState == State.Calling)
            {
                for (int i = 0; i < numberOfCalls; i++)
                {
                    audioSource.clip = clipSequence[i];
                    audioSource.Play();
                    while (audioSource.isPlaying)
                        yield return 0;
                }
                //audioSource.PlayOneShot(clipSequence[callNumber]);
                yield return new WaitForSeconds(callInterval);
            }
            else
            {
                isPlaying = false;
                yield break;
            }
        }
    }

    IEnumerator PunishPlayer()
    {
        print(Time.time);
        vibrationSource.touchOnly = false;
        vibrationSource.motor = VibrationSource.Motors.Hard;
        GameObject.FindGameObjectWithTag("Player").GetComponent<VibrationListener>().sourcesInRange.Add(vibrationSource);
        yield return new WaitForSeconds(punishmentTime);
        vibrationSource.touchOnly = true;
        vibrationSource.motor = VibrationSource.Motors.Soft;
        GameObject.FindGameObjectWithTag("Player").GetComponent<VibrationListener>().sourcesInRange.Remove(vibrationSource);
        print(Time.time);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("RegisterAnt", this);
            Debug.Log("Lost -> Calling");
            if (currentState == State.Lost) currentState = State.Calling;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("UnRegisterAnt", this);
        }
    }
}
