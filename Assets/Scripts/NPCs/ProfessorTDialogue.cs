using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessorTDialogue : NPC
{
    protected override void Start()
    {
        base.Start();
        dialogueLines = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<string>() },
            { QuestStage.QuestStages.RatLecture + "1", new List<string>()
            {
                "Professor T: Welcome to the first day of the Intro to Spelling course. Today, we will be learning the basics. To craft a spell, first you will need to know some glyphs. Glyphs come in three types: shape glyphs, effect glyphs, and modifier glyphs."
            } },
            { QuestStage.QuestStages.RatLecture + "2", new List<string>()
            {
                "Professor T: Shape glyphs dictate the shape of the spell, as you might imagine. They are the “base” of the spell. The shape glyph contains the other glyphs of the spell; if you try to cast a spell with only a shape glyph, nothing will happen. Some examples of shape glyphs include “bolt”, “ball”, and “spray”, though there are several more which you will learn in other classes."
            } },
            { QuestStage.QuestStages.RatLecture + "3", new List<string>()
            {
                "Professor T: Effect glyphs determine what effect the spell has when the spell triggers. For many spell shapes, such as bolt, this is when it hits something. Two common effects for spells are fire and healing. Healing will, as you might expect, heal injuries either on the target hit by the spell, or on all targets in the area covered by the spell, depending on the other glyphs. Fire does some damage initially, and has a chance of setting the target alight for a short time, causing additional harm."
            } },
            { QuestStage.QuestStages.RatLecture + "4", new List<string>()
            {
                "Professor T: Modifier glyphs affect the movement, magnitude, and targeting of a spell. For example, the “fast” glyph will cause the spell to travel faster. Most of the time, modifiers change the situations in which you will use the spell, and as such, for general use, not using them may be beneficial."
            } },
            { QuestStage.QuestStages.RatLecture + "5", new List<string>()
            {
                "Professor T: Now, to actually create a spell, you must first open up your spell creation menu. From there, you can create a spell by adding first a shape, such as “bolt”, and an effect, such as “fire”. From there, you can add to the spell with modifiers, and in a future class, you will learn that you can add even more interesting effects to your spells."
            } },
            { QuestStage.QuestStages.RatLecture + "6", new List<string>()
            {
                "Janitor: Rats in the dungeon, Professor! Rats in the dungeon!"
            } },
            { QuestStage.QuestStages.RatLecture + "7", new List<string>()
            {
                "Professor T: Well, well. In that case, this lecture is over. Rats seem like a great target for you to test your spell creation abilities on. You can also head to the practice hall to test out spell creation. <Player Name>, why don’t you go take care of our rats?"
            } },
            { QuestStage.QuestStages.RatLecture + "8", new List<string>()
            {
                "Professor T: I, err, need to water my cat"
            } },
            { QuestStage.QuestStages.RatLecture + "9", new List<string>()
            {
                "Professor T: If you do this little excursion I will count it as your midterm grade. Does that sound fair?"
            } },
            { QuestStage.QuestStages.RatLecture + "10", new List<string>()
            {
                "Professor T: Great! Now, this will be graded, so be sure to come back to me and I will let you know how you did "
            } },
            { QuestStage.QuestStages.RatStart + "1", new List<string>() },
            { QuestStage.QuestStages.RatFinished + "1", new List<string>()
            {
                "Welcome back, {1}. I see you have finished your quest. Now, let’s see here. "
            } },
            { QuestStage.QuestStages.RatFinished + "2", new List<string>()
            {
                "A pacifist, are you? Well done. It’s rare to see one unwilling to destroy life. I believe those rats learned their lesson. {1}!"
            } },
            { QuestStage.QuestStages.RatFinished + "3", new List<string>()
            {
                "Well now, you were very thorough! You deserve an {1} for this assignment. Well done! "
            } },
            { QuestStage.QuestStages.RatFinished + "4", new List<string>()
            {
                "Yes, I think I’ll give you a {1} for this assignment. Good work."
            } },
            { QuestStage.QuestStages.RatFinished + "5", new List<string>()
            {
                "Hmm, I expected a little bit more. You’ll be getting a {1} this time around. I hope you perform better in the future."
            } },
            { QuestStage.QuestStages.RatFinished + "6", new List<string>()
            {
                "Well since there is still the infestation, I believe you will be able to. Why don’t you go back and try again?"
            } },
            { QuestStage.QuestStages.RatFinished + "7", new List<string>()
            {
                "Now, do you have any questions for me?"
            } },
            { QuestStage.QuestStages.RatFinished + "8", new List<string>()
            {
                "Rats go with dungeons like mages and mana potions"
            } },
            { QuestStage.QuestStages.RatFinished + "9", new List<string>()
            {
                "A huge rat? Rats are not normally organised. Strange. Well, if you got rid of the alpha rat, I imagine there won’t be many rats left in that old dungeon."
            } },
            { QuestStage.QuestStages.RatFinished + "10", new List<string>()
            {
                "Ah, you encountered a brute. These types of enemies cannot be harmed unless you use the stun spell. I believe I have the stun spell tome around here somewhere…"
            } },
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
        playerResponses = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<string>() },
            { QuestStage.QuestStages.RatLecture + "7", new List<string>()
            {
                "Why don’t you?",
                "Fine, I’ll do it then"
            } },
            { QuestStage.QuestStages.RatLecture + "8", new List<string>()
            {
                "What will I get out of it?",
                "Fine, I’ll do it then"
            } },
            { QuestStage.QuestStages.RatLecture + "9", new List<string>()
            {
                "Fine, I’ll do it then"
            } },
            { QuestStage.QuestStages.RatStart + "1", new List<string>() },
            { QuestStage.QuestStages.RatFinished + "5", new List<string>()
            {
                "Can I make up this assignment?",
                "All right…"
            } },
            { QuestStage.QuestStages.RatFinished + "7", new List<string>()
            {
                "Why were there rats in the dungeon?",
                "There was this huge rat ",
                "Immune rat"
            } },
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
    }
    public override string GetNextLine()
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerDialogueChoice(int playerChoice)
    {
        throw new System.NotImplementedException();
    }

    public override void ResetDialogue()
    {
        throw new System.NotImplementedException();
    }
}
