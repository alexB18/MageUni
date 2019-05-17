using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPlayer : Interactable
{
    public string connectedScene = "";
    public string connectedMarker = "";

    public override void Interact(GameObject actor)
    {
        if(connectedMarker != "" && connectedScene != "")
            actor.GetComponent<PlayerSceneChange>().Transport(connectedScene, connectedMarker);
    }
}
