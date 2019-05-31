using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject destination;
    public bool activeDuringDemon = true;

    private void OnTriggerEnter(Collider other)
    {
        bool demonQuest = QuestStage.QS == QuestStage.QuestStages.DemonStart;
        if (other.CompareTag("Player") && (!demonQuest || (demonQuest && activeDuringDemon)))
        {
            other.transform.position = destination.transform.position;
            other.transform.rotation = destination.transform.rotation;
        }
    }
}
