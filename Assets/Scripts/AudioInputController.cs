using System;
using UnityEngine;

[Serializable]
public class FilterData
{
    public float HighPassCutoff;
    public float LowPassCutoff;
}

public class AudioInputController : MonoBehaviour
{
    public BandPassFilter BandPass;
    public string LeftButton;
    public string RightButton;
    public FilterData FilterLeftButton;
    public FilterData FilterRightButton;


    // Use this for initialization
    void Start()
    {
        if (!BandPass)
            enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool leftFilter = Input.GetButton(LeftButton);
        bool rightFilter = Input.GetButton(RightButton);
        if (leftFilter)
        {
            BandPass.ActivateFilter(FilterLeftButton.HighPassCutoff, FilterLeftButton.LowPassCutoff);
        }else if (rightFilter)
        {
            BandPass.ActivateFilter(FilterRightButton.HighPassCutoff, FilterRightButton.LowPassCutoff);
        }
        else
        {
            BandPass.Deactivate(); // deactivate filters
        }
    }
}
