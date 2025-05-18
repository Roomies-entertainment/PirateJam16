using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] private float cooldown = 0f;
    private float enterCooldownTimer;
    private bool enableEnterCooldown;

    [Header("")]
    [SerializeField] private UnityEvent<Collider2D> onTriggerEnter;
    [SerializeField] private UnityEvent<Collider2D> onTriggerExit;

    private Collider2D col;

    private HashSet<Rigidbody2D> rigidbodiesProcessed = new();

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        if (col == null)
            col = gameObject.AddComponent<Collider2D>();

        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (enterCooldownTimer > 0)
        {
            return;
        }

        if (ProcessCollider(collider))
        {
            enableEnterCooldown = true;

            onTriggerEnter?.Invoke(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (ProcessCollider(collider))
        {
            onTriggerExit?.Invoke(collider);
        }
    }

    private bool ProcessCollider(Collider2D collider)
    {
        var rb = collider.GetComponentInParent<Rigidbody2D>();

        if (rb && !rigidbodiesProcessed.Contains(rb))
        {
            rigidbodiesProcessed.Add(rb);

            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        if (enableEnterCooldown)
        {
            enterCooldownTimer = cooldown;

            enableEnterCooldown = false;
        }

        rigidbodiesProcessed.Clear();
    }

    private void Update()
    {
        enterCooldownTimer -= Time.deltaTime;
    }
}
