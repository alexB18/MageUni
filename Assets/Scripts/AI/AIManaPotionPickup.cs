using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManaPotionPickup : Interactable
{
    public override void Interact(GameObject actor)
    {
        AIPlayer pc = actor.GetComponent<AIPlayer>();
        pc.manaPotionCount += 1;
        gameObject.SetActive(false);
    }
}
