using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessorBDialogue : NPC
{
    protected override void Start()
    {
        base.Start();
        dialogueLines = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<string>() },
            { QuestStage.QuestStages.RatLecture + "1", new List<string>() },
            { QuestStage.QuestStages.RatStart + "1", new List<string>() },
            { QuestStage.QuestStages.RatFinished + "1", new List<string>() },
            { QuestStage.QuestStages.RatGraded + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeDorm + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeLecture + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeStart + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeFinished + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeGraded + "1", new List<string>() },
            { QuestStage.QuestStages.BoneDorm + "1", new List<string>() },
            { QuestStage.QuestStages.BoneLecture + "1", new List<string>()
            {
                "Professor B: Well hey there class, welcome to my lecture on BONES! Hoohoohoo… yes, this lecture is about the “Resurrection” effect glyph and its wonderful uses. Resurrection, upon targeting a dead creature, will resurrect that creature! Hoohoo… this even works if the creature barely has a body left! For example, if its flesh is all gone, and only its bones are left, you can still resurrect it! How wonderful is that, class? Hoohoohoohoo…"
            } },
            { QuestStage.QuestStages.BoneLecture + "2", new List<string>()
            {
                "Dean of Students: Skeletons! Skeletons in the cemetery! Help!"
            } },
            { QuestStage.QuestStages.BoneLecture + "3", new List<string>()
            {
                "Professor B: Well, of course there are skeletons in the cemetery, it’s a cemetery. It’s the bone zone, where the bones go."
            } },
            { QuestStage.QuestStages.BoneLecture + "4", new List<string>()
            {
                "Dean of Students: Well, yes, but these are WALKING skeletons! The aggressive kind! Someone do something!"
            } },
            { QuestStage.QuestStages.BoneStart + "1", new List<string>() },
            { QuestStage.QuestStages.BoneQuest + "1", new List<string>() },
            { QuestStage.QuestStages.BoneRetrieved + "1", new List<string>() },
            { QuestStage.QuestStages.BoneReturned + "1", new List<string>() },
            { QuestStage.QuestStages.BoneFinished + "1", new List<string>() },
            { QuestStage.QuestStages.BoneGraded + "1", new List<string>() },
            { QuestStage.QuestStages.DemonStart + "1", new List<string>() },
            { QuestStage.QuestStages.DemonFinished + "1", new List<string>() },
            { QuestStage.QuestStages.HellStart + "1", new List<string>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<string>() },
            { QuestStage.QuestStages.HellFinished + "1", new List<string>() },
            { QuestStage.QuestStages.GameFinished + "1", new List<string>() },
        };
        playerResponses = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<string>() },
            { QuestStage.QuestStages.RatLecture + "1", new List<string>() },
            { QuestStage.QuestStages.RatStart + "1", new List<string>() },
            { QuestStage.QuestStages.RatFinished + "1", new List<string>() },
            { QuestStage.QuestStages.RatGraded + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeDorm + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeLecture + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeStart + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeFinished + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeGraded + "1", new List<string>() },
            { QuestStage.QuestStages.BoneDorm + "1", new List<string>() },
            { QuestStage.QuestStages.BoneLecture + "1", new List<string>() },
            { QuestStage.QuestStages.BoneStart + "1", new List<string>() },
            { QuestStage.QuestStages.BoneQuest + "1", new List<string>() },
            { QuestStage.QuestStages.BoneRetrieved + "1", new List<string>() },
            { QuestStage.QuestStages.BoneReturned + "1", new List<string>() },
            { QuestStage.QuestStages.BoneFinished + "1", new List<string>() },
            { QuestStage.QuestStages.BoneGraded + "1", new List<string>() },
            { QuestStage.QuestStages.DemonStart + "1", new List<string>() },
            { QuestStage.QuestStages.DemonFinished + "1", new List<string>() },
            { QuestStage.QuestStages.HellStart + "1", new List<string>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<string>() },
            { QuestStage.QuestStages.HellFinished + "1", new List<string>() },
            { QuestStage.QuestStages.GameFinished + "1", new List<string>() },
        };
        playerResponsesAction = new Dictionary<string, List<Fragment>>
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<Fragment>() },
            { QuestStage.QuestStages.RatLecture + "1", new List<Fragment>() },
            { QuestStage.QuestStages.RatStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.RatFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.RatGraded + "1", new List<Fragment>() },
            { QuestStage.QuestStages.SlimeDorm + "1", new List<Fragment>() },
            { QuestStage.QuestStages.SlimeLecture + "1", new List<Fragment>() },
            { QuestStage.QuestStages.SlimeStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.SlimeFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.SlimeGraded + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneDorm + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneLecture + "1", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.BoneLecture + "2"; }
            } },
            { QuestStage.QuestStages.BoneLecture + "2", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.BoneLecture + "3"; }
            } },
            { QuestStage.QuestStages.BoneLecture + "3", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.BoneLecture + "4"; }
            } },
            { QuestStage.QuestStages.BoneLecture + "4", new List<Fragment>()
            {
                () => { QuestStage.QS = QuestStage.QuestStages.BoneStart;
                        Exit(); }
            } },
            { QuestStage.QuestStages.BoneStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneQuest + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneRetrieved + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneReturned + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneGraded + "1", new List<Fragment>() },
            { QuestStage.QuestStages.DemonStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.DemonFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.GameFinished + "1", new List<Fragment>() },
        };
    }
}
