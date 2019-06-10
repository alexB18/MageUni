using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentLectureAppear : MonoBehaviour
{
    private void Start()
    {
        bool appear = QuestStage.QS == QuestStage.QuestStages.RatDorm;
        appear = appear || QuestStage.QS == QuestStage.QuestStages.RatLecture;
        appear = appear || QuestStage.QS == QuestStage.QuestStages.SlimeDorm;
        appear = appear || QuestStage.QS == QuestStage.QuestStages.SlimeLecture;
        gameObject.SetActive(appear);
    }
}
