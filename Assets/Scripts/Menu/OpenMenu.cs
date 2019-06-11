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
    public GameObject movement_tut;
    public GameObject enter_tut;
    public GameObject quest_tut;
    public GameObject spell_creation_tut;
    private bool canOpen = false;

    // Singleton!
    public static OpenMenu openMenu;

    public void CanOpen(bool b)
    {
        canOpen = b;
    }

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
        if (Input.GetButtonDown("Menu") && canOpen)
        {
            if (!isPaused && !isDialogueOpen)
                Pause();
            else
            {
                if(currentMenu && currentMenu.BackButton.interactable)
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

    public void MovementTutorial()
    {
        Pause(false);
        movement_tut.SetActive(true);
    }

    public void LectureEnterTutorial()
    {
        Pause(false);
        enter_tut.SetActive(true);
    }

    public void QuestTutorial()
    {
        Pause(false);
        quest_tut.SetActive(true);
    }

    public void SpellCreationTutorial()
    {
        Pause(false);
        spell_creation_tut.SetActive(true);
    }

    public void SetCurrentMenu(CurrentMenuScript value)
    {
        currentMenu = value;
    }
}

