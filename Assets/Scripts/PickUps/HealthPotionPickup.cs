using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionPickup : Interactable
{
    public override void Interact(GameObject actor)
    {
        PlayerController pc = actor.GetComponent<PlayerController>();
        pc.healthPotionCount += 1;
        gameObject.SetActive(false);
    }
}
