using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBed : Interactable
{
    public RoommateDialogue roommate;
    public Transform roommateHoldingCell;
    public Transform roommateNormalPosition;
    public override void Interact(GameObject actor)
    {
        switch(QuestStage.QS)
        {
            case QuestStage.QuestStages.RatGraded:
                QuestStage.QS = QuestStage.QuestStages.SlimeDorm;
                roommate.Interact(actor);
                break;
            case QuestStage.QuestStages.SlimeGraded:
                QuestStage.QS = QuestStage.QuestStages.DemonDorm;
                roommate.Interact(actor);
                break;
            case QuestStage.QuestStages.BoneFinished:
                QuestStage.QS = QuestStage.QuestStages.DemonDorm;
                roommate.Interact(actor);
                break;
        }
    }
}
