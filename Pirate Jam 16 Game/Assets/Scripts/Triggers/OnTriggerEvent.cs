using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> onTriggerEnter;
    [SerializeField] private UnityEvent<Collider2D> onTriggerExit;
/* 
    [SerializeField] private bool useCollisionMatrix;
     */
    private void OnTriggerEnter2D(Collider2D collider)
    {
/* 
        if (useCollisionMatrix && Collisions.IgnoreColliderInMatrix(gameObject, collider))
            return;
         */    
        onTriggerEnter?.Invoke(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
/*         
        if (useCollisionMatrix && Collisions.IgnoreColliderInMatrix(gameObject, collider))
            return;
          */   
        onTriggerExit?.Invoke(collider);
    }
}
