using System;
using UnityEngine;
using System.Collections;

public static class ExtensionMethods {

    public static void ExecuteAfter(this MonoBehaviour mono, float time, Action action)
    {
        mono.StartCoroutine(WaitAndExecute(time, action));
    }

    private static IEnumerator WaitAndExecute(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public static void ExecuteAfterSilent(this MonoBehaviour mono, AudioSource source, Action action)
    {
        mono.StartCoroutine(WaitForSound(source, action));
    }

    private static IEnumerator WaitForSound(AudioSource source, Action action)
    {
        while (source.isPlaying)
            yield return 0;
        action();
    }
}
