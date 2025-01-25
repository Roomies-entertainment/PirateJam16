using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D cloud){
        Destroy(cloud.gameObject);
    }
}
