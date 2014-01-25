using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntStateMachine : MonoBehaviour {

	public GameManager manager;
	public enum State { Lost, Calling, Following, Safe }

	public State currentState = State.Lost;

	//Variables for Calling State
	public int numberOfCalls = 3;
	public int callNumber = 0;
	public float callInterval = 5;
	public AudioClip[] clipSequence;
	public AudioClip victory;

	//Variables for Following State

	private AudioSource audioSource;
	private bool isPlaying = false;

	void Start() {
		RandomizeAudioClips();
		audioSource = GetComponent<AudioSource>();
	}

	public void TestAudioClip(AudioClip playerChoice) {
		if (playerChoice == clipSequence[callNumber]) {
			callNumber++;
		} else {
			callNumber = 0;
			RandomizeAudioClips();
		}
		if (callNumber == numberOfCalls) {
			currentState = State.Following;
			audioSource.Stop();
			audioSource.PlayOneShot(victory);
		}
	}

	private void RandomizeAudioClips() {
		clipSequence = new AudioClip[numberOfCalls + 1];
		for (int i = 0; i < numberOfCalls; i++) {
			clipSequence[i] = manager.antClips[Random.Range(0, manager.antClips.Count)];
		}
		clipSequence[numberOfCalls] = victory;

	}

	void Update() {
		switch (currentState) {
			case State.Lost:
				break;
			case State.Calling:
				if (!isPlaying) {
					StartCoroutine(SoundLoop());
				}
				break;
			case State.Following:
				break;
			case State.Safe:
				break;
		}
	}

	IEnumerator SoundLoop() {
		for (; ; ) {
			isPlaying = true;
			if (callNumber < numberOfCalls && currentState == State.Calling) {
				audioSource.PlayOneShot(clipSequence[callNumber]);
				yield return new WaitForSeconds(callInterval);
			} else {
				isPlaying = false;
				yield break;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.SendMessage("RegisterAnt", this);
			Debug.Log("Lost -> Calling");
			if (currentState == State.Lost) currentState = State.Calling;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			other.SendMessage("UnRegisterAnt", this);
		}
	}
}
