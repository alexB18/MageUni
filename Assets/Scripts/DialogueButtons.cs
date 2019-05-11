using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButtons : MonoBehaviour
{
    public void Goodbye()
    {
        OpenMenu.isPaused = false;
        Time.timeScale = 1f;
    }
}
