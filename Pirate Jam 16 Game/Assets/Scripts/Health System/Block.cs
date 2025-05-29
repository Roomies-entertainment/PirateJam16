using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Block : MonoBehaviour
{
    public bool blocking { get; private set; }

    [Header("")]
    [SerializeField] protected bool debug;

    [Header("")]
    [SerializeField] private UnityEvent onStartBlocking;
    [SerializeField] private UnityEvent onStopBlocking;

    public virtual void StartBlocking()
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} is blocking");
        }

        blocking = true;

        onStartBlocking?.Invoke();
    }

    public virtual void StopBlocking()
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} stopped blocking");
        }

        blocking = false;

        onStopBlocking?.Invoke();
    }
}
