using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoopedEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEventStart;
    [SerializeField] private UnityEvent onEventEnd;

    [SerializeField] private float eventDuration = 1f;
    [SerializeField] private float eventInterval = 1f;

    private void Start()
    {
        StartCoroutine(EventLoop());
    }

    private IEnumerator EventLoop()
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(eventInterval);

            onEventStart?.Invoke();

            yield return new WaitForSeconds(eventDuration);

            onEventEnd?.Invoke();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
