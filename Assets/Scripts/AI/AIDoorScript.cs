using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDoorScript : Interactable
{
    public bool isLocked;

    public override void Interact(GameObject actor)
    {
        if (isLocked)
        {
            AIPlayer pc = actor.GetComponent<AIPlayer>();
            // Check if player has any keys
            if (pc.keyCount > 0)
            {
                pc.keyCount-= 1;
                gameObject.SetActive(false);
            }

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
