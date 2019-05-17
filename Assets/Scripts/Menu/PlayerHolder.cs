using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    public GameObject player;

    public GameObject Player { get => player; set => player = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject nPlayer =  GameObject.FindGameObjectWithTag("Player");
        if (nPlayer != null)
            player = nPlayer;
    }
}
