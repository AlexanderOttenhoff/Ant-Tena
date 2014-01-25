using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure;

[RequireComponent(typeof(AntController))]
public class VibrationListener : MonoBehaviour {

	public List<VibrationSource> sourcesInRange = new List<VibrationSource>();

	private AntController antController;
	private CharacterController characterController;

	public void Start() {
		antController = GetComponent<AntController>();
	}

	public void Update() {
		float left = 0, right = 0;
		foreach (VibrationSource source in sourcesInRange) {
			if (source.motor == VibrationSource.Motors.Hard) {
				left += source.GetVibration();
			} else {
				right += source.GetVibration();
			}
		}
		if (antController.playerIndex != null) GamePad.SetVibration(antController.playerIndex, left, right);
	}
}
