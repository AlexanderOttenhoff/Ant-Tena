using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VibrationSource))]
public class Anthill : MonoBehaviour {

	VibrationSource vibrationSource;

	// Use this for initialization
	void Start () {
		vibrationSource = GetComponent<VibrationSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
