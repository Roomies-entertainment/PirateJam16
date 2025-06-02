using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class DialoguePanel : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject dialogueBox;

    public GameObject continueTextLine;

    private int index;
    float textSpeed;
    List <string> textLines = new List <string>();

    private void Awake()
    {
        DialogueM.dialoguePanel = this;
        dialogueBox.SetActive(false);
    }

    void Start()
    {
        if (textComponent == null || dialogueBox == null || continueTextLine == null)
        {
            EditorApplication.isPaused = true;
        }
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == textLines[index])
            {
                NextLine();
                continueTextLine.SetActive(false);
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = textLines[index];
                continueTextLine.SetActive(true);
            }
        }

    }
    public void StartDialogue(string[] textLines, float textSpeed)
    {

        index = 0;
        this.textSpeed = Mathf.Lerp(0.3f, 0.0f, textSpeed * 0.1f);
        this.textLines = new(textLines);
        dialogueBox.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in textLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        continueTextLine.SetActive(true);
    }

    void NextLine()
    {
        if (index < textLines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else
        {
            dialogueBox.SetActive(false);
        }
    }

}
