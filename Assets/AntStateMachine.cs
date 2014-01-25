using UnityEngine;
using System.Collections.Generic;

public class AntStateMachine : MonoBehaviour {

	public List<AudioClip> clips;
	void Awake() {
		foreach (AudioClip clip in clips) {
			if (clip != null) {
				gameObject.AddComponent<AudioSource>();
			}
		}
	}
}
