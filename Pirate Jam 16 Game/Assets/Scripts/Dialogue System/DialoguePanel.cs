using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject dialogueBox;

    public GameObject continueTextLine;

    public AudioSource dialogueSauce;

    public Image characterImage;

    private int index;
    List<DialogueData> dialogueText = new List<DialogueData>();

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == dialogueText[index].dialogueText)
            {
                NextLine();
                continueTextLine.SetActive(false);
            }
            else
            {
                StopAllCoroutines();
                dialogueSauce.Stop();
                textComponent.text = dialogueText[index].dialogueText;
                continueTextLine.SetActive(true);
            }
        }

    }
    public void StartDialogue(List<DialogueData> _dialogue)
    {

        index = 0;
        this.dialogueText = new(_dialogue);
        dialogueBox.SetActive(true);
        AudioImage();
        StartCoroutine(TypeLine());
    }

    void CharacterImagePosition()
    {
        if (dialogueText[index].isLeft == true)
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
        foreach (char c in dialogueText[index].dialogueText.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(dialogueText[index].GetTextSpeed());
        }

        continueTextLine.SetActive(true);
    }

    void NextLine()
    {
        if (index < dialogueText.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            AudioImage();
            StartCoroutine(TypeLine());
        } else
        {
            dialogueBox.SetActive(false);
        }
    }

    void AudioImage()
    {
        CharacterImagePosition();

        if (dialogueText[index].characterAudio != null)
            dialogueSauce.clip = dialogueText[index].characterAudio;

        if (dialogueText[index].characterImage != null)
        {
            characterImage.enabled = true;
            characterImage.sprite = dialogueText[index].characterImage;
        }
        else
        {
            characterImage.enabled = false;
        }

        dialogueSauce.Play();
    }

}

