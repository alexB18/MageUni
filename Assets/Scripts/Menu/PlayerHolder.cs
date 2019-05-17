using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    public GameObject player;

    public GameObject Player { get => player; set => player = value; }
    
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }
}
