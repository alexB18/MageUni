using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestStage
{
    public static int QS = 0;
    public static bool RatStun = false;
    public static bool RatPacifist = false;
    public static bool SlimePacifist = false;
    public static bool SkeletonPacifist = false;
    public static bool DemonPacifist = false;
    public static bool HellPacifist = false;
    public enum QuestStages
    {
        RatDorm,
        RatLecture,
        RatStart,
        RatFinished,
        RatGraded,
        SlimeDorm,
        SlimeLecture,
        SlimeStart,
        SlimeFinished,
        SlimeGraded,
        BoneDorm,
        BoneLecture,
        BoneStart,
        BoneQuest,
        BoneRetrieved,
        BoneReturned,
        BoneFinished,
        BoneGraded,
        DemonDorm,
        DemonStart,
        DemonFinished,
        HellStart,
        HellBoss,
        HellFinished,
        GameFinished
    }
}
