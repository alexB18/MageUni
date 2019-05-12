using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTestScript : NPC
{
    private int whichLine = 0;

    public override string GetNextLine()
    {
        string nextLine;

        switch(whichLine)
        {
            case 0:
                nextLine = "Hello! I am here to teach you about interacting!";
                whichLine = 1;
                break;
            case 1:
                nextLine = "Interacting with the world around you is easy! Just press F while near something to interact with it!";
                whichLine = 2;
                break;
            case 2:
                nextLine = "Try interacting with everything! It may surprise you what you can find...";
                whichLine = 3;
                break;
            default:
                nextLine = "I have nothing more to say to you.";
                break;
        }
        
        return nextLine;
    }

    public override void ResetDialogue()
    {
        whichLine = 0;
    }
}
