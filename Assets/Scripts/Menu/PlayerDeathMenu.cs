using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathMenu : MonoBehaviour
{
    public PlayerHolder playerHolder;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder.player.GetComponent<StatScript>().SubscribeToOnDeath(OnDeath);
    }

    private void OnDeath(Object[] obj)
    {
        StartCoroutine("DieAndRestart");
    }
}
