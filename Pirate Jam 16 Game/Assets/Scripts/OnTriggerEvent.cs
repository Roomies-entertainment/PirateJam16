using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> onTriggerEnter;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        onTriggerEnter.Invoke(collider);
    }
}
