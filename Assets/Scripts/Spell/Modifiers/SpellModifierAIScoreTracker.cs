using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class SpellModifierAIScoreTracker : SpellModifier
{
    public Agent agent;
    public static float missScore = -2f;
    public static float hitScore = 10f;

    public override void Start(SpellScript self)
    {
        self.StopAllCoroutines();
        self.StartCoroutine(self.AIDecay(agent, 2f));
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        if (other.CompareTag("GoodGuy") || other.CompareTag("BadGuy") || other.CompareTag("Player"))
            agent.AddReward(hitScore);
        else
            agent.AddReward(missScore);
        return false;
    }
}
