using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour
{
    public static int selected;
    public PlayerHolder playerHolder;
    private Inventory playerInv;

    void Start()
    {
        playerInv = playerHolder.Player.GetComponent<Inventory>();
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
