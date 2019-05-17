using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButtons : MonoBehaviour
{
    public static NPC speaker;
    public Text dialogue;


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

    public void PlayerChoice1()
    {
        speaker.PlayerDialogueChoice(1);
    }
    public void PlayerChoice2()
    {
        speaker.PlayerDialogueChoice(2);
    }
    public void PlayerChoice3()
    {
        speaker.PlayerDialogueChoice(3);
    }
}
