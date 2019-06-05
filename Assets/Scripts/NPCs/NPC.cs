using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NPC : Interactable
{
    // Fragment class is used for player responses
    protected delegate void Fragment();
    // An NPC is anything that can be interacted with that brings up a dialogue window. Other interactables
    // such as chests or doors (potentially) should have a separate script.
    public string currentState;
    public int currentPos = 0;
    protected List<Button> playerChoicesList = new List<Button>();
    protected List<Text> buttonTextList = new List<Text>();
    protected int state = 1;
    protected string previousState = "";

    protected Dictionary<string, List<string>> dialogueLines;
    protected Dictionary<string, List<string>> playerResponses;
    protected Dictionary<string, List<Fragment>> playerResponsesAction;
    protected string defaultText = "Hey";

    protected virtual void Start()
    {
        if (DialogueButtons.dialogueButtons)
        {
            playerChoicesList.AddRange(DialogueButtons.dialogueButtons.playerChoices);
            buttonTextList.AddRange(DialogueButtons.dialogueButtons.playerChoiceTexts);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        //*/
    }

    public override void Interact(GameObject player)
    {
        OpenMenu.openMenu.Pause(false);
        DialogueButtons.speaker = this;
        DialogueButtons.dialogueButtons.textWindow.SetActive(true);
        DialogueButtons.dialogueButtons.goodbyeButton.interactable = false;
        DialogueButtons.dialogueButtons.nextButton.interactable = true;
        currentState = QuestStage.QS + "1";
        DialogueButtons.dialogueButtons.dialogue.text = GetNextLine();
    }
    
    // GetNextLine() should return a string with the next thing that the NPC says.
    public virtual string GetNextLine()
    {
        string nextLine;

        
        if (currentPos < (dialogueLines[currentState].Count - 1))
        {
            nextLine = dialogueLines[currentState][currentPos];
            currentPos++;
        }
        else
        {
            if (dialogueLines.ContainsKey(currentState) && dialogueLines[currentState].Count > 0)
                nextLine = dialogueLines[currentState][currentPos];
            else
                nextLine = defaultText;
            
            if(currentState != previousState)
                ShowResponses();
            if (!playerResponses.ContainsKey(currentState) || playerResponses[currentState].Count == 0)
            {
                currentPos = 0;
                if (playerResponsesAction.ContainsKey(currentState) && playerResponsesAction[currentState]?.Count == 1)
                {
                    // This happens if our dialogue continues to a new stage without player response
                    playerResponsesAction[currentState][0]();
                    if (currentState == "Exit")
                    {
                        DialogueButtons.dialogueButtons.goodbyeButton.interactable = true;
                        DialogueButtons.dialogueButtons.nextButton.interactable = false;
                    }
                }
                else
                {
                    DialogueButtons.dialogueButtons.goodbyeButton.interactable = true;
                    DialogueButtons.dialogueButtons.nextButton.interactable = false;
                }
            }
            else
                DialogueButtons.dialogueButtons.nextButton.interactable = false;
            // We check so that we don't have to hit next to get the same buttons if we have a self loop
            if (previousState == currentState)
            {
                ShowResponses();
                DialogueButtons.dialogueButtons.nextButton.interactable = false;
            }
        }

        return nextLine;
    }

    public virtual void PlayerDialogueChoice(int playerChoice)
    {
        currentPos = 0;
        previousState = currentState;
        playerResponsesAction[currentState][playerChoice]();

        DialogueButtons.dialogueButtons.nextButton.interactable = true;
        DialogueButtons.dialogueButtons.dialogue.text = GetNextLine();
    }

    // ResetDialogue() is called when the "Goodbye" button is pressed, and should do whatever
    // is necessary to reset the NPC's dialogue. This can be a no-op if the NPC's dialogue does not
    // reset between interactions; however, in the test NPC that I set up, this was necessary
    // to prevent the "Goodbye" button + re-interact functioning like the "Next" button.
    public virtual void ResetDialogue()
    {
        // Goodbye, so call the goodbye action
        if(playerResponsesAction.ContainsKey(currentState) && playerResponsesAction[currentState].Count == 1)
            playerResponsesAction[currentState][0]();
        currentPos = 0;
        state = 1;
        currentState = QuestStage.QS + "1";

        DialogueButtons.dialogueButtons.dialogue.text = "";
        foreach (Button button in playerChoicesList)
        {
            button.interactable = false;
        }
        foreach (Text text in buttonTextList)
        {
            text.text = "";
        }
    }

    public void ShowResponses()
    {
        if (playerResponses.ContainsKey(currentState))
        {
            int numButtons = 0;
            foreach (string response in playerResponses[currentState])
            {
                buttonTextList[numButtons].text = response;
                playerChoicesList[numButtons].interactable = true;
                numButtons++;
            }
        }
    }

    protected void Exit()
    {
        currentState = "Exit";
    }
}
