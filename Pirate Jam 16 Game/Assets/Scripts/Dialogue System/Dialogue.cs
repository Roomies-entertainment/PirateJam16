using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dialogue : MonoBehaviour
{
    public void DialogueAwake()
    {
        DialogueM.dialoguePanel.StartDialogue(lines, textSpeed);
    }

    public string[] lines;
    public float textSpeed = 1;

    





}
