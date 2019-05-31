using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDormTrigger : MonoBehaviour
{
    public RoommateDialogue roommate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && QuestStage.QS == QuestStage.QuestStages.RatDorm)
            roommate.Interact(gameObject);
    }
}
