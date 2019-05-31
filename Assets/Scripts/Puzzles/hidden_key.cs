using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidden_key : MonoBehaviour
{
    private int currentActiveTriggers = 0;
    public int totalTriggers = 2;
    GameObject hiddenKey;

    private void Start()
    {
        hiddenKey = transform.GetChild(0).gameObject;
        hiddenKey.SetActive(false);
    }

    public void setTriggerActive()
    {
        currentActiveTriggers += 1;
        if (currentActiveTriggers == totalTriggers)
        {
            hiddenKey = transform.GetChild(0).gameObject;
            hiddenKey.SetActive(true);
        }
    }
}
