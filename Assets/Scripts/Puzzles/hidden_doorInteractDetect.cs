using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidden_doorInteractDetect : Interactable
{

    public override void Interact(GameObject actor)
    {
        gameObject.GetComponentInParent<hidden_door>().setTriggerActive();
    }
}
