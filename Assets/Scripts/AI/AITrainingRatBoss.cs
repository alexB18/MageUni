using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrainingRatBoss : AITrainingRat
{
    protected override void Start()
    {
        base.Start();
        isBoss = true;
    }
}
