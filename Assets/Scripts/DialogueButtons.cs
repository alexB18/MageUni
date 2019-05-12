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
        OpenMenu.isPaused = false;
        Time.timeScale = 1f;
        speaker.ResetDialogue();
        speaker = null;
    }

    public void Next()
    {
        dialogue.text = speaker.GetNextLine();
    }
}
