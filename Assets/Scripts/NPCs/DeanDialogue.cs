using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeanDialogue : NPC
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
            { QuestStage.QuestStages.BoneLecture + "1", new List<string>() },
            { QuestStage.QuestStages.BoneStart + "1", new List<string>() },
            { QuestStage.QuestStages.BoneQuest + "1", new List<string>() },
            { QuestStage.QuestStages.BoneRetrieved + "1", new List<string>() },
            { QuestStage.QuestStages.BoneReturned + "1", new List<string>() },
            { QuestStage.QuestStages.BoneFinished + "1", new List<string>() },
            { QuestStage.QuestStages.BoneGraded + "1", new List<string>() },
            { QuestStage.QuestStages.DemonStart + "1", new List<string>() },
            { QuestStage.QuestStages.DemonFinished + "1", new List<string>()
            {
                "Dean of Students: Oh, thank goodness! Someone’s here! Wha--a student? Did--did you fight all of those demons that were outside?"
            } },
            { QuestStage.QuestStages.DemonFinished + "2", new List<string>()
            {
                "Dean of Students: It was easy? Well, then. Clearly you’re very powerful. In that case, could we ask you to deal with this problem more...permanently? That is to say...we will open a portal to Hell, and can you go through it and kill all the demons that you can find?"
            } },
            { QuestStage.QuestStages.DemonFinished + "3", new List<string>()
            {
                "Dean of Students: Ah. Well, you’re not going to like what we’re going to ask you to do next, then...can you go to Hell and kill every demon that you can find? We’re sure if you kill enough of them, they’ll stop attacking our campus."
            } },
            { QuestStage.QuestStages.DemonFinished + "4", new List<string>()
            {
                "Dean of Students: You, ah, what? How did you even manage that? I’m certain at least one of them was blocking this door…"
            } },
            { QuestStage.QuestStages.DemonFinished + "5", new List<string>()
            {
                "Dean of Students: So, will you do it? Will you go to Hell and kill six billion--I mean, kill as many demons as possible?"
            } },
            { QuestStage.QuestStages.DemonFinished + "6", new List<string>()
            {
                "Dean of Students: The game can’t continue unless you say “yes”, actually."
            } },
            { QuestStage.QuestStages.DemonFinished + "7", new List<string>()
            {
                "Dean of Students: Great. We're counting on you!"
            } },
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
            { QuestStage.QuestStages.DemonFinished + "1", new List<string>()
            {
                "Yeah, it was easy.",
                "Yeah, it was awful.",
                "No, I ran around them."
            } },
            { QuestStage.QuestStages.DemonFinished + "5", new List<string>()
            {
                "Sure, what could go wrong?",
                "Uh, I’ll pass."
            } },
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
            { QuestStage.QuestStages.BoneLecture + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneQuest + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneRetrieved + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneReturned + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.BoneGraded + "1", new List<Fragment>() },
            { QuestStage.QuestStages.DemonStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.DemonFinished + "1", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.DemonFinished + "2"; },
                () => { currentState = QuestStage.QuestStages.DemonFinished + "3"; },
                () => { currentState = QuestStage.QuestStages.DemonFinished + "4"; }
            } },
            { QuestStage.QuestStages.DemonFinished + "2", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.DemonFinished + "5"; }
            } },
            { QuestStage.QuestStages.DemonFinished + "3", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.DemonFinished + "5"; }
            } },
            { QuestStage.QuestStages.DemonFinished + "4", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.DemonFinished + "1"; }
            } },
            { QuestStage.QuestStages.DemonFinished + "5", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.DemonFinished + "7";},
                () => { currentState = QuestStage.QuestStages.DemonFinished + "6";}
            } },
            { QuestStage.QuestStages.DemonFinished + "6", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.DemonFinished + "5"; }
            } },
            { QuestStage.QuestStages.DemonFinished + "7", new List<Fragment>()
            {
                () => { QuestStage.QS = QuestStage.QuestStages.HellStart; Exit(); }
            } },
            { QuestStage.QuestStages.HellStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.GameFinished + "1", new List<Fragment>() },
        };
    }
}
