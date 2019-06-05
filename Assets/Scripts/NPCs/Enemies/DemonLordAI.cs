using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonLordAI : DemonAI
{
    public DemonLordDialogue dialogue;
    protected override void OnDeath(Object[] obj)
    {
        base.OnDeath(obj);
        QuestStage.QS = QuestStage.QuestStages.HellFinished;
        dialogue.Interact(gameObject);
    }
}

