using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentMenuScript : MonoBehaviour
{
    public Button backButton;
    public Button BackButton => backButton;

    public GameObject firstSelected;
    private GameObject lastSelected;

    private EventSystem eventSystem;
    public CurrentMenuScript upperMenu;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        OpenMenu.openMenu?.SetCurrentMenu(this);
    }

    private void OnDisable()
    {
        if (upperMenu)
            upperMenu.OnEnable();
    }

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject)
        {
            if (eventSystem.currentSelectedGameObject != lastSelected)
                lastSelected = eventSystem.currentSelectedGameObject;
        }
        else if (lastSelected != null)
            eventSystem.SetSelectedGameObject(lastSelected);
        else
            eventSystem.SetSelectedGameObject(firstSelected);
    }
}
