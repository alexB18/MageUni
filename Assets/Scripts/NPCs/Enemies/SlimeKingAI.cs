using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingAI : EnemyAI
{
    protected override void OnDeath(Object[] obj)
    {
        base.OnDeath(obj);
        QuestStage.Pacifist = false;
        QuestStage.QS = QuestStage.QuestStages.SlimeFinished;
    }
    protected override void Attack(GameObject t)
    {
        throw new System.NotImplementedException();
    }
}
