﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidden_key : MonoBehaviour
{
    private int currentActiveTriggers = 0;
    public int totalTriggers = 2;
    GameObject hiddenKey;

    private void Start()
    {
        hiddenKey = this.transform.GetChild(0).gameObject;
        hiddenKey.SetActive(false);
        GetComponentInParent<ColorChanger>().changeColor(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentActiveTriggers == totalTriggers)
        {
            GetComponent<ColorChanger>().changeColor(Color.green);
            hiddenKey = this.transform.GetChild(0).gameObject;
            hiddenKey.SetActive(true);
        }        
    }

    public void setTriggerActive()
    {
        this.currentActiveTriggers += 1;
    }
}
