using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure;

[RequireComponent(typeof(AntController))]
public class VibrationListener : MonoBehaviour {

	public List<VibrationSource> sourcesInRange;

	private AntController controller;

	public void Start() {
		controller = GetComponent<AntController>();
	}

	public void Update() {
		float left=0, right=0;
		foreach (VibrationSource source in sourcesInRange) {
			if (source.motor== VibrationSource.Motors.Hard) {
				left += source.GetVibration();
			} else {
				right += source.GetVibration();
			}
		}
		GamePad.SetVibration(controller.playerIndex, left, right);
	}

	void OnCollisionEnter(Collision other) {
		VibrationSource vs = other.collider.GetComponent("VibrationSource") as VibrationSource;
		if (!sourcesInRange.Contains(vs)) {
			sourcesInRange.Add(vs);
			vs.listener = this;
		}
	}

	void OnCollisionExit(Collision other) {
		VibrationSource vs = other.collider.GetComponent("VibrationSource") as VibrationSource;
		if (sourcesInRange.Contains(vs)) {
			vs.listener = null;
			sourcesInRange.Remove(vs);
		}
	}
}
