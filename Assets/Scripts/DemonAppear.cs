using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAppear : MonoBehaviour
{
    public Renderer mesh;
    public EnemyAI ai;
    public Collider coll;
    // Update is called once per frame
    void Update()
    {
        if(QuestStage.QS == QuestStage.QuestStages.DemonStart || QuestStage.QS == QuestStage.QuestStages.DemonFinished)
        {
            if (!mesh.enabled)
            {
                mesh.enabled = true;
                ai.enabled = true;
                coll.gameObject.layer = 31;
            }
        }
        else
        {
            if (mesh.enabled)
            {
                mesh.enabled = false;
                ai.enabled = false;
                coll.gameObject.layer = 28;
            }
        }
    }
}
