using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class Wait
{
    public static void WaitForIt(this MonoBehaviour mono, float delay, UnityAction action)
    {
        mono.StartCoroutine(StartWaiting(delay, action));
    }

    private static IEnumerator StartWaiting(float delay, UnityAction action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
        yield break;
    }
}
