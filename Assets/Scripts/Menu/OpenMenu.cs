using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public bool isPaused = false;
    private bool isDialogueOpen = false;
    CurrentMenuScript currentMenu;
    public GameObject menu;
    private float startPhysicsStep;

    // Singleton!
    public static OpenMenu openMenu;

    private void Awake()
    {
        openMenu = this;
    }

    private void Start()
    {
        startPhysicsStep = Time.fixedDeltaTime;
        Pause(false);
        menu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (!isPaused && !isDialogueOpen)
                Pause();
            else
            {
                currentMenu?.BackButton.onClick.Invoke();
            }
        }
    }
    public void Pause(bool showMenu = true)
    {
        isPaused = true;
        menu.SetActive(showMenu);
        isDialogueOpen = !showMenu;
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }
    public void Resume()
    {
        isPaused = false;
        isDialogueOpen = false;
        currentMenu = null;
        menu.SetActive(false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = startPhysicsStep;
    }

    public void SetCurrentMenu(CurrentMenuScript value)
    {
        currentMenu = value;
    }
}

