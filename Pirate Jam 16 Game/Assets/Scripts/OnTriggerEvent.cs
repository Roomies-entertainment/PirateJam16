using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider> onTriggerEnter;
    
    private void OnTriggerEnter(Collider collider)
    {
        onTriggerEnter.Invoke(collider);
    }
}
