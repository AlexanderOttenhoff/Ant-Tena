using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class FilterManager : MonoBehaviour {

	public AudioSource[] audioSources;
	public float bandSeparation = 100;
	public float minFrequency = 20;
	public float maxFrequency = 2000;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GamePadState state = GamePad.GetState(PlayerIndex.One);
		foreach (AudioSource source in audioSources) {
			AudioLowPassFilter lpf = source.GetComponent<AudioLowPassFilter>();
			AudioHighPassFilter hpf = source.GetComponent<AudioHighPassFilter>();
			float target = state.Triggers.Left * (maxFrequency - minFrequency - bandSeparation) + minFrequency + bandSeparation / 2;
			lpf.cutoffFrequency = target + bandSeparation / 2;
			hpf.cutoffFrequency = target - bandSeparation / 2;
			Debug.Log(target);
		}
	}
}
