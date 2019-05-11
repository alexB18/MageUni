using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTestScript : MonoBehaviour
{

    public GameObject textWindow;
    public GameObject player;
    public float interactDistance;

    // NOTE: This script is a simple test script to make talking to people work.
    // It will likely need to be rewritten once more NPCs/interactables are added.
    // My intention was to make it a superclass so each object can behave differently.

    // Update is called once per frame
    void Update()
    {
        if (!OpenMenu.isPaused)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if ((player.transform.position - transform.position).magnitude <= interactDistance)
                {
                    Interact();
                }
            }
        }
    }

    public void Interact()
    {
        OpenMenu.isPaused = true;
        textWindow.SetActive(true);
        Time.timeScale = 0f;
    }
}
