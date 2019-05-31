using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidden_door : MonoBehaviour
{
    private int currentActiveTriggers = 0;
    public int totalTriggers = 2;
    GameObject hiddenDoor;

    // Make sure door is rendered on start
    private void Start()
    {
        hiddenDoor = transform.GetChild(0).gameObject;
        hiddenDoor.SetActive(true);
    }

    public void setTriggerActive()
    {
        currentActiveTriggers += 1;
        if (currentActiveTriggers == totalTriggers)
        {
            hiddenDoor = transform.GetChild(0).gameObject;
            hiddenDoor.SetActive(false);
        }
    }
}
