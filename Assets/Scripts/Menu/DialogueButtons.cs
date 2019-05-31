using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueButtons : MonoBehaviour
{
    public static DialogueButtons dialogueButtons;
    public static NPC speaker;

    public GameObject textWindow;
    public Text dialogue;

    public Button[] playerChoices;

    public Text[] playerChoiceTexts;

    public Button goodbyeButton;
    public Button nextButton;

    private void Awake()
    {
        dialogueButtons = this;
    }

    private void Start()
    {
        dialogueButtons = this;
    }

    private void Update()
    {
    }

    public void Goodbye()
    {
        OpenMenu.openMenu.Resume();
        speaker.ResetDialogue();
        speaker = null;
    }

    public void Next()
    {
        dialogue.text = speaker.GetNextLine();
    }

    public void PlayerChoice(int index)
    {
        foreach (Button button in playerChoices)
        {
            button.interactable = false;
        }
        foreach (Text text in playerChoiceTexts)
        {
            text.text = "";
        }
        speaker.PlayerDialogueChoice(index);
    }
}
