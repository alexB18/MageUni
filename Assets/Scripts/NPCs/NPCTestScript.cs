using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTestScript : NPC
{
    
    void Start()
    {
        playerChoicesList = new List<GameObject>();
        playerChoicesList.Add(playerChoice1);
        playerChoicesList.Add(playerChoice2);
        playerChoicesList.Add(playerChoice3);
        buttonTextList = new List<Text>();
        buttonTextList.Add(playerChoice1Text);
        buttonTextList.Add(playerChoice2Text);
        buttonTextList.Add(playerChoice3Text);

        currentState = "state1";
        currentPos = 0;

        dialogueLines = new Dictionary<string, List<string>>();
        dialogueLines.Add("state1", new List<string>());
        dialogueLines.Add("state2", new List<string>());
        dialogueLines.Add("state3", new List<string>());
       
        dialogueLines["state1"].Add("Hello! I am here to teach you about interacting!");
        dialogueLines["state1"].Add("Interacting with the world around you is easy! Just press F while near something to interact with it!");
        dialogueLines["state1"].Add("Try interacting with everything! It may surprise you what you can find...");

        dialogueLines["state2"].Add("Yes, of course! You can walk, talk, and use spells to heal yourself or damage enemies.");
        dialogueLines["state2"].Add("The fabric of the universe is currently unstable, so I can't be certain that will all stay true.");
        dialogueLines["state2"].Add("Did you get all that? I can say all of this again, if you'd like.");

        dialogueLines["state3"].Add("If you've forgotten what I said, I can say it again, if you'd like.");

        playerResponses = new Dictionary<string, List<string>>();
        playerResponses.Add("state1", new List<string>());
        playerResponses.Add("state2", new List<string>());
        playerResponses.Add("state3", new List<string>());

        playerResponses["state1"].Add("Can you tell me what all I can do?");

        playerResponses["state2"].Add("Yes, I got everything. Thank you!");
        playerResponses["state2"].Add("No, I missed some of that. Could you start over, please?");
        playerResponses["state2"].Add("Could you just tell me about the things I can do? I got the 'how to interact' bit.");

        playerResponses["state3"].Add("That would be great!");
    }

    public override string GetNextLine()
    {
        string nextLine;

        if (currentPos < (dialogueLines[currentState].Count - 1))
        {
            nextLine = dialogueLines[currentState][currentPos];
            currentPos++;
        }
        else
        {
            nextLine = dialogueLines[currentState][currentPos];
            ShowResponses();
        }       

        return nextLine;
    }

    public override void ResetDialogue()
    {
        currentPos = 0;
        if (currentState != "state3")
        {
            currentState = "state1";
        }
        playerResponsesWindow.SetActive(false);
        foreach (GameObject button in playerChoicesList)
        {
            button.SetActive(false);
        }
    }

    public override void PlayerDialogueChoice(int playerChoice)
    {
        playerResponsesWindow.SetActive(false);
        foreach (GameObject button in playerChoicesList)
        {
            button.SetActive(false);
        }

        currentPos = 0;
        switch (currentState)
        {
            case "state1":
                currentState = "state2";
                break;
            case "state2":
                switch (playerChoice)
                {
                    case 1:
                        currentState = "state3";
                        break;
                    case 2:
                        currentState = "state1";
                        break;
                    case 3:
                        currentState = "state2";
                        break;
                }
                break;
            case "state3":
                currentState = "state1";
                break;
        }

        dialogue.text = GetNextLine();
    }

    private void ShowResponses()
    {
        playerResponsesWindow.SetActive(true);
        int numButtons = 0;
        foreach (string response in playerResponses[currentState])
        {
            buttonTextList[numButtons].text = response;
            playerChoicesList[numButtons].SetActive(true);
            numButtons++;
        }
    }

}
