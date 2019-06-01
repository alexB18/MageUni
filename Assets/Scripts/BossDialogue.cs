using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDialogue : MonoBehaviour
{
    public DemonLordDialogue dialogue;
    private void OnTriggerEnter(Collider other)
    {
        if(QuestStage.QS == QuestStage.QuestStages.HellStart)
        {
            QuestStage.QS = QuestStage.QuestStages.HellBoss;
            dialogue.Interact(gameObject);
        }
    }
}
