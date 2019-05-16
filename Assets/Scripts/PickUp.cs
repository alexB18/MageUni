using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float sqrInteractDistance;
    public string description;
    public string itemName;
    public Sprite sprite;
    public Rigidbody thisRigidbody;
    public GameObject player;

    private Inventory playerInv;


    // Start is called before the first frame update
    void Start()
    {
        playerInv = player.GetComponent<Inventory>();
        thisRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!OpenMenu.isPaused)
        {
            if (Input.GetButtonDown("Interact"))
            {
                // using sqrMagnitude here to cheapen this calculation--if we have a lot of
                // interactables in a scene there could be big lag without it.
                if ((player.transform.position - transform.position).sqrMagnitude <= sqrInteractDistance)
                {
                    Interact();
                }
            }
        }
    }

    void Interact()
    {
        playerInv.inventory.Add(this);
        // teleports item to space to hide it from the player
        transform.position = new Vector3(1000, 1000, 1000);

        if (thisRigidbody != null) {
            thisRigidbody.useGravity = false;
        }
    }
}
