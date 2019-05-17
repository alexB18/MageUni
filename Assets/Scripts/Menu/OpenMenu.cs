using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public bool isPaused = false;
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
        Pause();
        menu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (!isPaused)
                Pause();
        }
    }
    public void Pause()
    {
        isPaused = true;
        menu.SetActive(true);
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }
    public void Resume()
    {
        isPaused = false;
        menu.SetActive(false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = startPhysicsStep;
    }
}
