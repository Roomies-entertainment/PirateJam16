using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogueEvent
{
    public EventData eventData;
    public bool waitForCompletion;

    public DialogueEvent(EventData eventData, bool waitForCompletion)
    {
        this.eventData = eventData;
        this.waitForCompletion = waitForCompletion;
    }
}

public class DialoguePanel : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject dialogueBox;

    public GameObject continueTextLine;

    public AudioSource dialogueSauce;

    public Image characterImage;


    private int lineIndex = -1;

    List<DialogueData> dialogueData = new();
    List<DialogueEvent> dialogueEvents = new();

    private bool waitingOnEvent = false;
    private bool lineComplete = false;

    private void Awake()
    {
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

    public void StartDialogue(List<DialogueData> dataList)
    {
        dialogueData = new(dataList);
        dialogueBox.SetActive(true);
        
        foreach (Player p in FindObjectsOfType<Player>()) { // temp code
            p.SetGameplayEnabled(false); }

        StopAllCoroutines();
        NextLine(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && lineIndex >= 0)
        {
            // If line not complete - Set true so that TypeLine() coroutine skips to end
            if (!lineComplete)
            {
                lineComplete = true;
            }
            else if (lineComplete)
            {
                if (dialogueData[lineIndex].eventAtEnd == true)
                {
                    StartEvent();
                }

                NextLine();
            }
        }

        UpdateEvents(out waitingOnEvent);
    }

    void UpdateEvents(out bool waitingOnEvent)
    {
        waitingOnEvent = false;
        
        for (int i = dialogueEvents.Count - 1; i > 0; i--)
        {
            DialogueEvent e = dialogueEvents[i];

            e.eventData.UpdateEvent(out bool finished);

            if (finished)
            {
                dialogueEvents.RemoveAt(i);
            }
            else if (e.waitForCompletion)
            {
                waitingOnEvent = true;
            }
        }
    }

    void NextLine(bool firstLine = false)
    {
        if (firstLine)
            lineIndex = 0;
        else
            lineIndex++;

        lineComplete = false;
        continueTextLine.SetActive(false);

        if (lineIndex < dialogueData.Count)
        {
            StartCoroutine(TypeLine());
            DialogueEffects();

            if (dialogueData[lineIndex].eventAtEnd == false)
            {
                StartEvent();
            }
        }
        else
        {
            EndDialogue();
        }
    }

    void StartEvent()
    {
        dialogueData[lineIndex]._event.StartEvent();

        dialogueEvents.Add(
            new DialogueEvent(
                dialogueData[lineIndex]._event,
                dialogueData[lineIndex].waitForEvent));
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;

        foreach (char c in dialogueData[lineIndex].dialogueText.ToCharArray())
        {
            if (!lineComplete)
            {
                textComponent.text += c;
                yield return new WaitForSeconds(dialogueData[lineIndex].GetTextSpeed());
            }
            else
            {
                break;
            }
        }

        dialogueSauce.Stop();
        textComponent.text = dialogueData[lineIndex].dialogueText;

        continueTextLine.SetActive(true);
    }

    void DialogueEffects()
    {
        CharacterImagePosition();

        if (dialogueData[lineIndex].characterAudio != null)
            dialogueSauce.clip = dialogueData[lineIndex].characterAudio;

        if (dialogueData[lineIndex].characterImage != null)
        {
            characterImage.enabled = true;
            characterImage.sprite = dialogueData[lineIndex].characterImage;
        }
        else
        {
            characterImage.enabled = false;
        }

        dialogueSauce.Play();
    }

    void CharacterImagePosition() //this will need to be adjusted based on the images, all images need to be the same resolution and directional facing for this to work.
    {
        if (dialogueData[lineIndex].isLeft == true)
        {
            //Placement is on the left of the screen
            characterImage.rectTransform.anchoredPosition = new Vector3(-500, characterImage.rectTransform.anchoredPosition.y, 0);
            characterImage.rectTransform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else
        {
            //Placement is on the right of the screen
            characterImage.rectTransform.localPosition = new Vector3(500, characterImage.rectTransform.anchoredPosition.y, 0);
            characterImage.rectTransform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        Debug.Log("End of dialogue scene is being called");

        foreach (Player p in FindObjectsOfType<Player>()) { // temp code
            p.SetGameplayEnabled(true); }

        lineIndex = -1;
    }
}
