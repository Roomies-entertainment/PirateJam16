using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider){
        Destroy(collider.gameObject);
    }
}
