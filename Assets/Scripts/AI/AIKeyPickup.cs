using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKeyPickup : Interactable
{
    public override void Interact(GameObject actor)
    {
        AIPlayer pc = actor.GetComponent<AIPlayer>();
        pc.keyCount += 1;
        gameObject.SetActive(false);
    }
}
