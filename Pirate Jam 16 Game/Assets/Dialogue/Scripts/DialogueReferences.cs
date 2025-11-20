using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueReferences : MonoBehaviour
{
    public static DialogueReferences dialogueReferences;

    public DialoguePanel dialoguePanel;

    private void Awake()
    {
        dialogueReferences = this;
    }
}
