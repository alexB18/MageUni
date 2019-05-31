using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBonedBenDialogue : NPC
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
            { QuestStage.QuestStages.BoneStart + "1", new List<string>()
            {
                "Big-Boned Ben: Ah, someone has sent for help. Can you help me find my bones, young one?"
            } },
            { QuestStage.QuestStages.BoneStart + "2", new List<string>()
            {
                "Big-Boned Ben: Yes, my bones! I lost my bones somewhere in the cemetery, and now I can’t find them. Could you help?"
            } },
            { QuestStage.QuestStages.BoneStart + "3", new List<string>()
            {
                "Big-Boned Ben: No, no, my dress bones! You’ve got dress clothes, right? I need my dress bones, and I can’t find them!"
            } },
            { QuestStage.QuestStages.BoneStart + "4", new List<string>()
            {
                "Big-Boned Ben: Bless you, young one! I think they’re scattered all over, because right after I dropped my bones, these creepy skeletons came and grabbed them and started flinging them to and fro. Good luck finding my bones!"
            } },
            { QuestStage.QuestStages.BoneQuest + "1", new List<string>()
            {
                "Big-Boned Ben: Have you found all of my bones? No? Please, young one, I need my bones! I think I saw some of the skeletons run down, underground, with my bones. Please find my bones…"
            } },
            { QuestStage.QuestStages.BoneRetrieved + "1", new List<string>()
            {
                "Big-Boned Ben: Thank you, young one! You found my bones! Now I can go to the big party…"
            } },
            { QuestStage.QuestStages.BoneRetrieved + "2", new List<string>()
            {
                "Big-Boned Ben: But I can’t be having someone running around talking about how Big-Boned Ben lost his bones. I have a reputation to maintain, you know, otherwise I wouldn’t do this. Sorry, but I must destroy you now."
            } },
            { QuestStage.QuestStages.BoneReturned + "1", new List<string>() },
            { QuestStage.QuestStages.BoneFinished + "1", new List<string>()
            {
                "Big-Boned Ben: But...my bones are so big! How could you defeat me? What are you??"
            } },
            { QuestStage.QuestStages.BoneFinished + "2", new List<string>()
            {
                "<Well, that was...certainly an interesting experience.>"
            } },
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
            { QuestStage.QuestStages.BoneStart + "1", new List<string>()
            {
                "Your...bones?",
                "Looks like you’ve got plenty…",
                "Sure, I can help."
            } },
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
    }
}
