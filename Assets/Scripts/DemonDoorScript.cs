using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDoorScript : Interactable
{
    public override void Interact(GameObject actor)
    {
        if (QuestStage.QS == QuestStage.QuestStages.DemonStart)
        {
            gameObject.SetActive(false);
            QuestStage.QS = QuestStage.QuestStages.DemonFinished;
        }
    }
}
