using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LectureOutsideTut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!QuestStage.DidLectureEnter)
        {
            QuestStage.DidLectureEnter = true;
            OpenMenu.openMenu.LectureEnterTutorial();
        }
    }
}
