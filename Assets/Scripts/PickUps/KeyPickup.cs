using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : Interactable
{
    public override void Interact(GameObject actor)
    {
        PlayerController pc = actor.GetComponent<PlayerController>();
        pc.numKeys += 1;
        gameObject.SetActive(false);
    }
}
