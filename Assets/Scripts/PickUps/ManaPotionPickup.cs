using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionPickup : Interactable
{
    public override void Interact(GameObject actor)
    {
        PlayerController pc = actor.GetComponent<PlayerController>();
        pc.manaPotionCount += 1;
        gameObject.SetActive(false);
    }
}
