using UnityEngine;
using System.Collections.Generic;

public class AntStateMachine : MonoBehaviour {

	public GameManager manager;

	public int numberOfStates = 1;
	public int stateNumber = 0;
	public float audioInterval = 5;
	public AudioClip[] clipSequence;

	private float lastPlayTime;
	private AudioSource audioSource;

	void Start() {
		RandomizeAudioClips();
		lastPlayTime = Time.timeSinceLevelLoad;
		audioSource = GetComponent<AudioSource>();
	}

	public bool TestAudioClip(AudioClip playerChoice) {
		if (playerChoice == clipSequence[stateNumber]) {
			stateNumber++;
			return true;
		} else {
			stateNumber = 0;
			RandomizeAudioClips();
			return false;
		}
	}

	private void RandomizeAudioClips() {
		clipSequence = new AudioClip[numberOfStates];
		for (int i = 0; i < numberOfStates; i++) {
			clipSequence[i] = manager.antClips[Random.Range(0, manager.antClips.Count - 1)];
		}
	}

	void Update() {
		if (Time.timeSinceLevelLoad > lastPlayTime + audioInterval) {
			audioSource.PlayOneShot(clipSequence[stateNumber]);
		}
	}

}
