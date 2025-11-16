using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionMessages : MonoBehaviour
{
    [SerializeField] private PlayerCollision Collision;

    private void OnCollisionStay2D(Collision2D collision) { Collision.DoOnCollisionStay2D(collision); }
    private void OnCollisionExit2D(Collision2D collision) { Collision.DoOnCollisionExit2D(collision); }
}
