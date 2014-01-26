using UnityEngine;
using System.Collections;

public class L2S3 : MonoBehaviour
{
    public AudioClip ListenToYourFriends;
    public AudioClip FindFood;
    private AntController controller;
    private bool _firstAnt;

    // Use this for initialization
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<AntController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_firstAnt && controller.ants.Count == 1)
        {
            audio.clip = ListenToYourFriends;
            audio.Play();
            _firstAnt = true;
        }
        else if (controller.ants.Count == 3)
        {
            audio.clip = FindFood;
            audio.Play();
            this.ExecuteAfterSilent(audio, () => { enabled = false; });
        }
    }

    //void OnTriggerEnter(Collider collider)
    //{
    //    Debug.Log("trigger enter");
    //    if (collider.name == "RunningAnt")
    //    {
    //        audio.Play();
    //    }
    //}
}
