using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPlayer : Interactable
{
    public string connectedScene = "";
    public string connectedMarker = "";

    public bool isLocked;
    public bool questLocked = false;
    public QuestStage.QuestStages stage;

    public override void Interact(GameObject actor)
    {
        // Check to make sure door is linked
        if (connectedMarker != "" && connectedScene != "")
        {
            if ((questLocked && stage == QuestStage.QS) || !questLocked)
            {
                // Check to see if door is locked
                if (isLocked)
                {

                    PlayerController pc = actor.GetComponent<PlayerController>();
                    // Check if player has any keys
                    // If they do, use one on the door
                    if (pc.NumKeys > 0)
                    {
                        pc.NumKeys -= 1;
                        isLocked = false;
                        actor.GetComponent<PlayerSceneChange>().Transport(connectedScene, connectedMarker);

                    }

                }
                else
                {
                    actor.GetComponent<PlayerSceneChange>().Transport(connectedScene, connectedMarker);
                }
            }
        }
    }
}
