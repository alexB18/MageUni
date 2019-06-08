using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthPotionPickup : Interactable

{
    public override void Interact(GameObject actor)
    {
        AIPlayer pc = actor.GetComponent<AIPlayer>();
        pc.healthPotionCount += 1;
        gameObject.SetActive(false);
    }
}
