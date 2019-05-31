using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTestScript : NPC
{
    
    protected override void Start()
    {
        base.Start();

        currentState = "state1";

        dialogueLines = new Dictionary<string, List<string>>
        {
            { "state1", new List<string>() },
            { "state2", new List<string>() },
            { "state3", new List<string>() }
        };

        dialogueLines["state1"].Add("Hello! I am here to teach you about interacting!");
        dialogueLines["state1"].Add("Interacting with the world around you is easy! Just press F while near something to interact with it!");
        dialogueLines["state1"].Add("Try interacting with everything! It may surprise you what you can find...");

        dialogueLines["state2"].Add("Yes, of course! You can walk, talk, and use spells to heal yourself or damage enemies.");
        dialogueLines["state2"].Add("The fabric of the universe is currently unstable, so I can't be certain that will all stay true.");
        dialogueLines["state2"].Add("Did you get all that? I can say all of this again, if you'd like.");

        dialogueLines["state3"].Add("If you've forgotten what I said, I can say it again, if you'd like.");

        playerResponses = new Dictionary<string, List<string>>
        {
            { "state1", new List<string>() },
            { "state2", new List<string>() },
            { "state3", new List<string>() }
        };

        playerResponses["state1"].Add("What can I do?");

        playerResponses["state2"].Add("Got it!");
        playerResponses["state2"].Add("I missed something.");
        playerResponses["state2"].Add("Second part again.");

        playerResponses["state3"].Add("Please do.");
    }

    public override void PlayerDialogueChoice(int playerChoice)
    {
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

        DialogueButtons.dialogueButtons.nextButton.interactable = true;
        DialogueButtons.dialogueButtons.dialogue.text = GetNextLine();
    }


}
