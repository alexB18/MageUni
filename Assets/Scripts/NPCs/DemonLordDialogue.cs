using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonLordDialogue : NPC
{
    protected override void Start()
    {
        base.Start();
        dialogueLines = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.HellStart + "1", new List<string>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<string>()
            {
                "FOOLISH MORTAL. YOU MAY HAVE BESTED THE LESSER DEMONS, BUT I AM THE KING OF HELL! I WILL DESTROY YOUR CAMPUS’ PETUNIAS!!"
            } },
            { QuestStage.QuestStages.HellFinished + "1", new List<string>()
            {
                "IMPOSSIBLE, HOW COULD I HAVE BEEN DEFEATED?"
            } },
            { QuestStage.QuestStages.GameFinished + "1", new List<string>() },
        };
        playerResponses = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.HellStart + "1", new List<string>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<string>() },
            { QuestStage.QuestStages.HellFinished + "1", new List<string>() },
            { QuestStage.QuestStages.GameFinished + "1", new List<string>() },
        };
        playerResponsesAction = new Dictionary<string, List<Fragment>>()
        {
            { QuestStage.QuestStages.HellBoss + "1", new List<Fragment>()
            {
                () => {
                    BackgroundMusic.music.SwitchBackground();
                    Exit();
                }
            } },
            { QuestStage.QuestStages.HellFinished + "1", new List<Fragment>()
            {
                () => {
                    BackgroundMusic.music.Pause();
                    Exit();
                }
            } },
        };
    }
}
