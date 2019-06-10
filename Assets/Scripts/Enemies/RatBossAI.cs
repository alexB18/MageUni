using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBossAI : RatAI
{
    protected override void OnDeath(Object[] obj)
    {
        base.OnDeath(obj);
        QuestStage.Pacifist = false;
        QuestStage.QS = QuestStage.QuestStages.RatFinished;
    }
}
