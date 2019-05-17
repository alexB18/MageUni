using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : Interactable
{
    public float sqrInteractDistance;
    public string description;
    public string itemName;
    public Sprite sprite;
    public Rigidbody thisRigidbody;
    public GameObject player;

    protected Inventory playerInv;


    // Start is called before the first frame update
    void Start()
    {
        playerInv = player.GetComponent<Inventory>();
        thisRigidbody = GetComponent<Rigidbody>();
    }
    
    public abstract void Use();

    override public void Interact(GameObject actor)
    {
        playerInv.inventory.Add(this);
        // teleports item to space to hide it from the player
        transform.position = new Vector3(1000, 1000, 1000);

        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = false;
        }
    }
}
