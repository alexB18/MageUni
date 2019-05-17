using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NPC : MonoBehaviour
{
    // An NPC is anything that can be interacted with that brings up a dialogue window. Other interactables
    // such as chests or doors (potentially) should have a separate script.

    public GameObject player;
    public float sqrInteractDistance;
    public GameObject textWindow;
    public Text dialogue;
    public string currentState;
    public int currentPos;
    public Button playerChoice1;
    public Button playerChoice2;
    public Button playerChoice3;
    protected List<Button> playerChoicesList;
    public Text playerChoice1Text;
    public Text playerChoice2Text;
    public Text playerChoice3Text;
    protected List<Text> buttonTextList;
    public Button nextButton;

    public Dictionary<string, List<string>> dialogueLines;
    public Dictionary<string, List<string>> playerResponses;

    // Update is called once per frame
    void Update()
    {
        if (!OpenMenu.openMenu.isPaused)
        {
            if (Input.GetButtonDown("Interact"))
            {
                // using sqrMagnitude here to cheapen this calculation--if we have a lot of
                // interactables in a scene there could be big lag without it.
                if ((player.transform.position - transform.position).sqrMagnitude <= sqrInteractDistance)
                {
                    Interact();
                }
            }
        }
    }

    public void Interact()
    {
        OpenMenu.openMenu.Pause();
        DialogueButtons.speaker = this;
        textWindow.SetActive(true);
        dialogue.text = GetNextLine();
        Time.timeScale = 0f;
    }
    
    // GetNextLine() should return a string with the next thing that the NPC says.
    public abstract string GetNextLine();

    public abstract void PlayerDialogueChoice(int playerChoice);

    // ResetDialogue() is called when the "Goodbye" button is pressed, and should do whatever
    // is necessary to reset the NPC's dialogue. This can be a no-op if the NPC's dialogue does not
    // reset between interactions; however, in the test NPC that I set up, this was necessary
    // to prevent the "Goodbye" button + re-interact functioning like the "Next" button.
    public abstract void ResetDialogue();
}
