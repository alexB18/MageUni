using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingAI : SlimeAI
{
    protected override void OnDeath(Object[] obj)
    {
        base.OnDeath(obj);
        QuestStage.Pacifist = false;
        QuestStage.QS = QuestStage.QuestStages.SlimeFinished;
    }
}
