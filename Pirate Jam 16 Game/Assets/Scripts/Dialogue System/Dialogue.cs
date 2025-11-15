using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

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


public class Dialogue : MonoBehaviour
{

    [SerializeField] List<DialogueData> _dialogue = new List<DialogueData>();

    public void DialogueAwake()
    {
        StaticReferences.dialoguePanel.StartDialogue(_dialogue);
    }


}
