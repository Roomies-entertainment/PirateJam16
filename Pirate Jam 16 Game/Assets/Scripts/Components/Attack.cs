using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected float attackRadius = 2f;
    [SerializeField] protected int BaseDamage = 1;

    [Header("")]
    [SerializeField] protected AudioClip attackSound;

    [Header("")]
    [SerializeField] protected UnityEvent onAttack;
}
