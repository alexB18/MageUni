using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : Interactable
{
    public bool isLocked;

    public override void Interact(GameObject actor)
    {
        if (isLocked)
        {
            PlayerController pc = actor.GetComponent<PlayerController>();
            // Check if player has any keys
            if (pc.numKeys > 0)
            {
                pc.numKeys-= 1;
                gameObject.SetActive(false);
            }

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
