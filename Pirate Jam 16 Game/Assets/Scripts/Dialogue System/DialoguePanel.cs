using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialoguePanel : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject dialogueBox;

    public GameObject continueTextLine;

    public AudioSource dialogueSauce;

    public Image characterImage;

    private int index;
    List<DialogueData> dialogueData = new List<DialogueData>();

    private bool eventFinished = true;

    private void Awake()
    {
        DialogueM.dialoguePanel = this;
        dialogueBox.SetActive(false);
        characterImage.enabled = false;
    }

    void Start()
    {
        if (textComponent == null || dialogueBox == null || continueTextLine == null)
        {
            EditorApplication.isPaused = true;
        }
        textComponent.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == dialogueData[index].dialogueText)
            {
                NextLine();
                continueTextLine.SetActive(false);
            }
            else
            {
                StopAllCoroutines();
                dialogueSauce.Stop();
                textComponent.text = dialogueData[index].dialogueText;
                continueTextLine.SetActive(true);
            }
        }


        if (!eventFinished)
        {
            dialogueData[index]._event.UpdateEvent(out eventFinished);
        }

    }
    public void StartDialogue(List<DialogueData> _dialogue)
    {

        index = 0;
        this.dialogueData = new(_dialogue);
        dialogueBox.SetActive(true);
        DialogueEffects();
        StopAllCoroutines();
        StartCoroutine(TypeLine());


    }

    void CharacterImagePosition()
    {
        if (dialogueData[index].isLeft == true)
        {
            //Placement is on the left of the screen
            characterImage.rectTransform.anchoredPosition = new Vector3(-500, characterImage.rectTransform.anchoredPosition.y, 0);
            characterImage.rectTransform.rotation = Quaternion.LookRotation(Vector3.back);
        } else
        {
            //Placement is on the right of the screen
            characterImage.rectTransform.localPosition = new Vector3(500, characterImage.rectTransform.anchoredPosition.y, 0);
            characterImage.rectTransform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogueData[index].dialogueText.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(dialogueData[index].GetTextSpeed());
        }

        continueTextLine.SetActive(true);
    }

    void NextLine()
    {
        if (index < dialogueData.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            DialogueEffects();

            eventFinished = false;
            dialogueData[index]._event.StartEvent();


            StartCoroutine(TypeLine());
        } else
        {
            dialogueBox.SetActive(false);
        }
    }

    void DialogueEffects()
    {
        CharacterImagePosition();

        if (dialogueData[index].characterAudio != null)
            dialogueSauce.clip = dialogueData[index].characterAudio;

        if (dialogueData[index].characterImage != null)
        {
            characterImage.enabled = true;
            characterImage.sprite = dialogueData[index].characterImage;
        }
        else
        {
            characterImage.enabled = false;
        }

        dialogueSauce.Play();
    }

}

