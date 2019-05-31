using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLectureTrigger : MonoBehaviour
{
    public ProfessorTDialogue professorTDialogue;
    public Transform professorTHolding;
    public ProfessorHDialogue professorHDialogue;
    public Transform professorHHolding;
    public ProfessorBDialogue professorBDialogue;
    public Transform professorBHolding;
    public Transform podium;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            professorTDialogue.transform.position = professorTHolding.position;
            professorTDialogue.transform.rotation = professorTHolding.rotation;

            professorHDialogue.transform.position = professorHHolding.position;
            professorHDialogue.transform.rotation = professorHHolding.rotation;

            professorBDialogue.transform.position = professorBHolding.position;
            professorBDialogue.transform.rotation = professorBHolding.rotation;

            switch (QuestStage.Quest)
            {
                case QuestStage.Quests.Rat:
                    professorTDialogue.transform.position = podium.position;
                    professorTDialogue.transform.rotation = podium.rotation;
                    if(QuestStage.QS == QuestStage.QuestStages.RatLecture) professorTDialogue.Interact(other.gameObject);
                    break;
                case QuestStage.Quests.Slime:
                    professorHDialogue.transform.position = podium.position;
                    professorHDialogue.transform.rotation = podium.rotation;
                    if (QuestStage.QS == QuestStage.QuestStages.SlimeLecture) professorHDialogue.Interact(other.gameObject);
                    break;
                case QuestStage.Quests.Bone:
                    professorBDialogue.transform.position = podium.position;
                    professorBDialogue.transform.rotation = podium.rotation;
                    if (QuestStage.QS == QuestStage.QuestStages.BoneLecture) professorBDialogue.Interact(other.gameObject);
                    break;
            }
        }
    }
}
