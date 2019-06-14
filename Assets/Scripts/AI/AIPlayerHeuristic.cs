using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AIPlayerHeuristic : Decision
{
    private const float normLength = 1.8f;
    private const float normWidth = 0.3f;
    private const float bossMult = 5f;
    private const float bossLength = normLength * bossMult;
    private const float bossWidth = normWidth * bossMult;
    private const float angleToCornerRad = 1.405648f;
    private const float distToCorner = 0.912414f;
    public override float[] Decide(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        /*
         * 0: Horizontal
         * 1: Vertical
         * 2: Rotation
         * 3: Firebolt
         * 4: Freeze
         */ 
        float[] ret = new float[5];
        // Go through enemies
        Vector2 movement = new Vector2();
        int target = 0;
        for(int i = 0; i < 4; ++i)
        {
            int index = 5 + 5 * i;
            // If this thing is dead, move on
            if (vectorObs[index + 4] <= 0f)
                continue;
            // in range
            if (vectorObs[index + 0] < 20f)
            {
                // -- CHECK IF WE CAN SHOOT THIS --
                // the delta here is the offset from the center of the target
                float delta = Mathf.Abs(vectorObs[index + 0] * Mathf.Tan(Mathf.Deg2Rad * vectorObs[index + 2]));
                //----- Get the potential width of the object ----
                /*
                float g = Mathf.Abs(vectorObs[index + 1]) - 90f;
                float al = normLength * Mathf.Cos(Mathf.Deg2Rad * g);
                float aw = normWidth * Mathf.Cos(Mathf.Deg2Rad * (90 - g));
                // This is a boss
                if(vectorObs[index + 3] > 0.5f)
                {
                    al *= bossMult;
                    aw *= bossMult;
                }
                // We have the projection, now we need the offset of the center from the
                float projection = al + aw;
                float b = angleToCornerRad - Mathf.Deg2Rad *(vectorObs[1] + 90f);
                float o = normWidth * Mathf.Cos(b);
                //*/
                float a = 0.3f * (vectorObs[index + 3] > 0.5f ? bossMult : 1f);
                // Check if our offset is within the bounds of the enemy and we have mana
                if (delta < a && vectorObs[2] > 10f)
                    ret[3] = 1.0f;

                // Do we run away? If the angle is between 10 and within 10 metres
                if (Mathf.Abs(vectorObs[index + 1]) < 10f && vectorObs[index] < 15f)
                {
                    float g = 180f - vectorObs[index + 1] + vectorObs[index + 2];
                    float d = 15f / Mathf.Min(5f, vectorObs[index + 0]);
                    movement.x += d * Mathf.Sin(Mathf.Deg2Rad * g);
                    movement.y += d * Mathf.Cos(Mathf.Deg2Rad * g);

                    // If we don't have a closer target, chose this as the target
                    if (target == 0)
                        target = index;
                    else if (vectorObs[target] > vectorObs[index])
                        target = index;
                }
            }
            if (vectorObs[index + 3] > 0.5f && target == 0)
            {
                // If this is the boss and we don't have a target, let's move forward
                float g = vectorObs[index + 2];
                float d = 15f / Mathf.Min(5f, vectorObs[index + 0]);
                movement.x += d * Mathf.Sin(Mathf.Deg2Rad * g);
                movement.y += d * Mathf.Cos(Mathf.Deg2Rad * g);
                target = index;
            }

        }

        // normalise our movement
        movement.Normalize();
        ret[0] = movement.x;
        ret[1] = movement.y;

        // Rotate towards target
        //*
        float rotation = vectorObs[target + 2];
        rotation /= 25f;
        if (rotation > 1f)
            ret[2] = 1f;
        else if (rotation < -1f)
            ret[2] = -1f;
        else
            ret[2] = rotation;
        //*/
        return ret;
    }

    public override List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        return new List<float>();
    }
}
