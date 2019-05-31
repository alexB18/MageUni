using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidden_keyInteractDetect : Interactable
{
    //private bool inPlace = false;
    Rigidbody otherRb;

    public override void Interact(GameObject actor)
    {
        gameObject.GetComponentInParent<hidden_key>().setTriggerActive();
    }
}
