using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    public AntController Player;
    public int NecessaryAnts;

    private bool nearby;

    private VibrationSource _vibeSource;
    private float _defaultMaxLevel;
    private bool _finish;

    // Use this for initialization
    void Start()
    {
        _vibeSource = GetComponent<VibrationSource>();
        _defaultMaxLevel = _vibeSource.maxLevel;
        _vibeSource.maxLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (nearby && Player.ants.Count >= NecessaryAnts)
        {
            _vibeSource.maxLevel = _defaultMaxLevel;
            if (Vector3.Distance(Player.transform.position, transform.position) < 5f && !_finish)
            {
                _finish = true;
                audio.Play();
                Debug.Log("Win");
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        nearby = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        nearby = false;
    }

}
