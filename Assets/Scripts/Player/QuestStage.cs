using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestStage
{
    public static QuestStages QS = QuestStages.RatDorm;
    internal class QuestDetails
    {
        internal bool pacifist;
        internal int score;
        internal int frozenEnemies;
    }
    private static Dictionary<QuestStages, Quests> QS_Q = new Dictionary<QuestStages, Quests>()
    {
        {QuestStages.RatDorm, Quests.Rat},
        {QuestStages.RatLecture, Quests.Rat},
        {QuestStages.RatStart, Quests.Rat},
        {QuestStages.RatFinished ,Quests.Rat},
        {QuestStages.RatGraded, Quests.Rat},
        {QuestStages.SlimeDorm, Quests.Slime},
        {QuestStages.SlimeLecture, Quests.Slime},
        {QuestStages.SlimeStart, Quests.Slime},
        {QuestStages.SlimeFinished, Quests.Slime},
        {QuestStages.SlimeGraded, Quests.Slime},
        {QuestStages.BoneDorm, Quests.Bone},
        {QuestStages.BoneLecture, Quests.Bone},
        {QuestStages.BoneStart, Quests.Bone},
        {QuestStages.BoneQuest, Quests.Bone},
        {QuestStages.BoneRetrieved, Quests.Bone},
        {QuestStages.BoneReturned, Quests.Bone},
        {QuestStages.BoneFinished, Quests.Bone},
        {QuestStages.BoneGraded, Quests.Bone},
        {QuestStages.DemonDorm, Quests.Demon},
        {QuestStages.DemonStart, Quests.Demon},
        {QuestStages.DemonFinished, Quests.Demon},
        {QuestStages.HellStart, Quests.Hell},
        {QuestStages.HellBoss, Quests.Hell},
        {QuestStages.HellFinished, Quests.Hell},
        {QuestStages.GameFinished, Quests.Hell}
    };
    private static Dictionary<Quests, QuestDetails> QDs = new Dictionary<Quests, QuestDetails>()
    {
        {Quests.Rat,    new QuestDetails() { pacifist = true, score = 0, frozenEnemies = 0 } },
        {Quests.Slime,  new QuestDetails() { pacifist = true, score = 0, frozenEnemies = 0 } },
        {Quests.Bone,   new QuestDetails() { pacifist = true, score = 0, frozenEnemies = 0 } },
        {Quests.Demon,  new QuestDetails() { pacifist = true, score = 0, frozenEnemies = 0 } },
        {Quests.Hell,   new QuestDetails() { pacifist = true, score = 0, frozenEnemies = 0 } },
    };
    private static QuestDetails Index => QDs[QS_Q[QS]];
    public static bool Pacifist { get => Index.pacifist; set => Index.pacifist = value; }
    public static int Score { get => Index.score; set => Index.score = value; }
    public static void Reset()
    {
        QS = QuestStages.RatStart;
        Score = 0;
        Pacifist = true;
        QS = QuestStages.SlimeStart;
        Score = 0;
        Pacifist = true;
        QS = QuestStages.BoneStart;
        Score = 0;
        Pacifist = true;
        QS = QuestStages.DemonStart;
        Score = 0;
        Pacifist = true;
        QS = QuestStages.HellStart;
        Score = 0;
        Pacifist = true;


        QS = QuestStages.RatDorm;
    }
    public static Quests Quest => QS_Q[QS];


    /**
* 0 = D+, 1 = C-, 2 = C, 3 = D+, 4 = B+, 5 = A-, 6 = A, 7 = A+
*/
    public static int Grade()
    {
        int grade = 7;
        return grade;
    }
    public enum Quests
    {
        Rat,
        Slime,
        Bone,
        Demon,
        Hell
    }
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
