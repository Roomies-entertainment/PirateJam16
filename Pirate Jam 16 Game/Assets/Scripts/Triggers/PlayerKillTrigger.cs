using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == CollisionM.playerLayer)
        {
            var player = collider.GetComponentInParent<Health>();
            player.IncrementHealth(-player.health, null);
        }         
    }
}
