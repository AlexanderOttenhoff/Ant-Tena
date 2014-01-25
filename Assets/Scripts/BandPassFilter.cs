using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioLowPassFilter), typeof(AudioHighPassFilter))]
public class BandPassFilter : MonoBehaviour
{
    public const float MaxFrequency = 22000.0f;
    public const float MinFrequency = 0f;

    public float HighPassCutoff;
    public float LowPassCutoff;
    public bool OffOnAwake;

    private AudioLowPassFilter _lowPass;
    private AudioHighPassFilter _highPass;

	// Use this for initialization
	void Start ()
	{
	    _lowPass = GetComponent<AudioLowPassFilter>();
	    _highPass = GetComponent<AudioHighPassFilter>();
	    if (OffOnAwake)
	    {
	        _lowPass.enabled = false;
	        _highPass.enabled = false;
	    }
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _lowPass.cutoffFrequency = LowPassCutoff;
	    _highPass.cutoffFrequency = HighPassCutoff;
	}

    /// <summary>
    /// Activates a bandpass filter which allows frequencies between highPassCutoff and lowPassCutoff.
    /// </summary>
    public void ActivateFilter(float highPassCutoff, float lowPassCutoff)
    {
        LowPassCutoff = lowPassCutoff;
        HighPassCutoff = highPassCutoff;
        _lowPass.enabled = true;
        _lowPass.enabled = true;
    }

    public void Deactivate()
    {
        _lowPass.enabled = false;
        _lowPass.enabled = false;
    }
}
