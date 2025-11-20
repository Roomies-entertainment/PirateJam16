using System.Collections.Generic;
using UnityEngine;
using System;

public class Dialogue : MonoBehaviour
{

    [SerializeField] List<DialogueData> _dialogue = new List<DialogueData>();

    public void StartDialogue()
    {
        DialogueReferences.dialogueReferences.dialoguePanel.StartDialogue(_dialogue);
    }
}


[Serializable]
public class DialogueData
{
    public string dialogueText;

    [Header("If text speed is 0, Defaults to 8")]
    [SerializeField] private float textSpeed = 8f;
    public float GetTextSpeed()
    {
        if (textSpeed == 0f)
            textSpeed = 8f;

        return Mathf.Lerp(0.3f, 0.0f, textSpeed * 0.1f); ;

    }

    [Header("")]
    public Sprite characterImage;
    public AudioClip characterAudio;

    [Header("What side do you want the character to be?")]
    public bool isLeft = true;

    [Header("Do you want the event to come first or last?")]
    public bool eventAtEnd = false;
    [Header("Should the dialogue panel wait for the event to finish before moving to next?")]
    public bool waitForEvent = false;
    [Space]
    public EventData _event;
}