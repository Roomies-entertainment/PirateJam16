using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerDialogue : TriggerEvent
{
    [SerializeField] private Dialogue dialogue;

    protected override void Awake()
    {
        base.Awake();

        gameObject.layer = CollisionM.playerOnlyLayer;
    }

    private void OnEnable()
    {
        onTriggerEnter.AddListener(CallStartDialogue);
    }

    private void OnDisable()
    {
        onTriggerEnter.RemoveListener(CallStartDialogue);
    }

    private void CallStartDialogue(Collider2D collider)
    {
        dialogue.StartDialogue();
    }
}
