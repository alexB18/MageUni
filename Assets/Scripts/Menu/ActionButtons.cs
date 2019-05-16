using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour
{
    public static int selected;
    public GameObject player;
    private Inventory playerInv;

    void Start()
    {
        playerInv = player.GetComponent<Inventory>();
    }
    
    public void Drop()
    {
        playerInv.Drop(selected);
    }

    public void Use()
    {
        // TODO
    }
}
