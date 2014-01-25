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
}
