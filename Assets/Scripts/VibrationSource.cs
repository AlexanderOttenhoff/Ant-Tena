using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class VibrationSource : MonoBehaviour {

	public enum VibrationTypes { Constant, Triangle, Sawtooth, Pulse, Sine };
	public enum Motors { Hard, Soft };

	public VibrationTypes vibrationType = VibrationTypes.Constant;
	public Motors motor = Motors.Soft;

	[Range(0, 1)]
	public float maxLevel = 0.1f;

	[Range(0, 1)]
	public float minLevel = 0f;
	public float frequency = 0;

	[Range(0, 2 * Mathf.PI)]
	public float phase = 0;

	[Range(0, 1)]
	public float pulseWidth = 0.5f;

	public bool distanceSensitive = true;

	private VibrationListener listener;
	private float? radius;

	void Start() {
		SphereCollider sc = GetComponent<SphereCollider>();
		if (sc != null)	radius = sc.radius;
		else			radius = null;
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<VibrationListener>() != null)
			listener = other.GetComponent("VibrationListener") as VibrationListener;
		if (listener != null) {
			listener.sourcesInRange.Add(this);
		}
	}

	void OnTriggerExit(Collider other) {
		if (listener != null) {
			listener.sourcesInRange.Remove(this);
			listener = null;
		}
	}

	public float GetVibration() {
		float t = Time.timeSinceLevelLoad;
		float v = 0;
		float pos = Mathf.Repeat(t - phase, 1 / frequency) * frequency;

		switch (vibrationType) {
			case VibrationTypes.Constant:
				v = maxLevel;
				break;
			case VibrationTypes.Triangle:
				if (pos < .5f) {
					v = Mathf.Lerp(minLevel, maxLevel, pos * 2f);
				} else {
					v = Mathf.Lerp(maxLevel, minLevel, (pos - .5f) * 2f);
				}

				break;
			case VibrationTypes.Sawtooth:
				v = Mathf.Lerp(minLevel, maxLevel, pos);
				break;
			case VibrationTypes.Pulse:
				v = Mathf.Lerp(0, 1, pos) < pulseWidth ? maxLevel : minLevel;
				break;
			case VibrationTypes.Sine:
				v = (minLevel + (maxLevel - minLevel) * Mathf.Sin(2 * Mathf.PI * frequency * t + phase));
				break;
		}

		if (distanceSensitive && radius.HasValue) {
			float dist = (listener.transform.position - transform.position).magnitude;
			v *= (radius.Value - dist) / radius.Value;
		}

		return v;
	}
}
