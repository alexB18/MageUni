using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBossDeath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to death
        StatScript ss = GetComponent<StatScript>();
        ss.SubscribeToOnDeath(OnDeath);
    }

    void OnDeath(Object[] obj)
    {
        //
        OpenMenu.openMenu.Pause();
        GameObject winMenu = GameObject.Find("WinMenu");
    }
}
