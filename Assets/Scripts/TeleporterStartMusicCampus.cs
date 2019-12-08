using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterStartMusicCampus : MonoBehaviour
{
    private bool doOnce = true;
    private void OnTriggerEnter(Collider other)
    {
        bool demonQuest = QuestStage.QS == QuestStage.QuestStages.DemonStart;
        if (other.CompareTag("Player") && (demonQuest && doOnce))
        {
            doOnce = false;
            BackgroundMusic.music.SwitchBackground();
        }
    }
}
