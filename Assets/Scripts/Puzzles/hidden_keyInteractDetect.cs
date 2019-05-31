using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidden_keyInteractDetect : Interactable
{
    //private bool inPlace = false;
    Rigidbody otherRb;
    private bool executed = false;

    public override void Interact(GameObject actor)
    {
        if(!executed)
        {
            gameObject.GetComponentInParent<hidden_key>().setTriggerActive();
            executed = true;
        }
        

    }
}
