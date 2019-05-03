using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject menu;
    void Update()
    {
        if (!isPaused)
        {
            if (Input.GetButtonDown("Menu"))
            {
                menu.SetActive(true);
                isPaused = true;
                Time.timeScale = 0f;
            }
        }
    }
}
