using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessorHDialogue : NPC
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
            { QuestStage.QuestStages.SlimeLecture + "1", new List<string>()
            {
                "Professor H: Welcome, class. This lecture is, primarily, about combining spells, but we will, as always, be introducing new glyphs to you as well.",
            } },
            { QuestStage.QuestStages.SlimeLecture + "2", new List<string>()
            {
                "Professor H: Combining spells is a fairly simple affair. To combine a spell with another spell, simply draw a new spell shape inside the primary spell. This will, typically, result in the second spell being cast when the primary spell hits its target, or more generally, triggers. Of course, you must draw an effect for the second spell as well, or it will not do anything when the primary spell triggers.",
            } },
            { QuestStage.QuestStages.SlimeLecture + "3", new List<string>()
            {
                "Professor H: And the glyph we will be learning about today is an effect glyph: freeze. The freeze glyph is an interesting one indeed. When applied to a spell, the most common use for freeze is to stop something, typically a hostile creature. This allows the mage time to draw upon some additional mana to deal with the enemy. Of note, if you are the adventuring sort...freeze does not injure creatures on its own. It simply leaves the creature unable to act for a time, or until it has been injured in some other manner. However, against creatures with liquid or semi-liquid parts, such as--",
            } },
            { QuestStage.QuestStages.SlimeLecture + "4", new List<string>()
            {
                "Professor T: Slimes! Slimes in the mines!",
            } },
            { QuestStage.QuestStages.SlimeLecture + "5", new List<string>()
            {
                "Professor H: Such as slimes, yes, if the effect is broken, they will be injured additionally due to how deep the frost penetrates. That concludes this lecture; if any of you ARE the adventuring sort, I suspect that Professor T. will welcome your assistance in clearing the mines.",
            } },
            { QuestStage.QuestStages.SlimeStart + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeFinished + "1", new List<string>()
            {
                "Professor H: Ah yes, <Player Name>. You’re back. I suspect you’ll be wanting a grade for this assignment?"
            } },
            { QuestStage.QuestStages.SlimeFinished + "2", new List<string>()
            {
                "Professor H:  Well that’s a rare sight. A pacifist? They’re just low level monsters, but even then you wouldn’t raise your fist against them? I commend you. You deserve this {1}"
            } },
            { QuestStage.QuestStages.SlimeFinished + "3", new List<string>()
            {
                "Professor H: Excellent work. I hope you perform as diligently in the future. You get an {1}"
            } },
            { QuestStage.QuestStages.SlimeFinished + "4", new List<string>()
            {
                "Professor H: Yes, yes. You got the job done, for sure. That’s {1} level work. Good job."
            } },
            { QuestStage.QuestStages.SlimeFinished + "5", new List<string>()
            {
                "Professor H: Well now, I don’t know how well you solved the problem. You’ll be getting a <Grade> this time around. I hope you perform better in the future."
            } },
            { QuestStage.QuestStages.SlimeFinished + "6", new List<string>()
            {
                "Professor H: I’m willing to allow you to redo this assignment. Go back and thoroughly destroy those slimes!"
            } },
            { QuestStage.QuestStages.SlimeFinished + "7", new List<string>()
            {
                "Professor H: Now, do you have any questions for me?"
            } },
            { QuestStage.QuestStages.SlimeFinished + "8", new List<string>()
            {
                "Professor H: If healing spells are not sufficing, you can try making a more complex type of spell. Look for the Wall shape, the Barrier effect, the Self modifier, and maybe the Orbit modifier. If you combine all of these into a single spell, you will have a shield surrounding you. If you choose to have this spell target an entity without using Orbit, you could also trap them."
            } },
            { QuestStage.QuestStages.SlimeFinished + "9", new List<string>()
            {
                "Professor H: The Slime King still lived? Amazing. Who knew that our humble little mine would have such an entity. Yes, the Slime King was one of original slimes when the world was created. A shame that he is now gone."
            } },
            { QuestStage.QuestStages.SlimeFinished + "10", new List<string>()
            {
                "Professor H: The Slime King still lives? Amazing. Who knew that our humble little mine would have such an entity. Yes, the Slime King is one of original slimes when the world was created. Thank you for not killing him. I’ll be sure to go down and do research on the matter at some point."
            } },
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
