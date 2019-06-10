using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentDialogue : NPC
{
    protected override void Start()
    {
        base.Start();
        dialogueLines = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<string>()
                { "Student: Yo."}
            },
            { QuestStage.QuestStages.RatDorm + "2", new List<string>()
                { "Student: Hey."}
            },
            { QuestStage.QuestStages.RatDorm + "3", new List<string>()
                { "Student: What's up."}
            },
            { QuestStage.QuestStages.RatDorm + "4", new List<string>()
                { "Student: Nice to meet you."}
            },
            { QuestStage.QuestStages.RatLecture + "1", new List<string>()
                { "Student: Why am I here?"}
            },
            { QuestStage.QuestStages.RatLecture + "2", new List<string>()
                { "Student: I hope I don't fall asleep."}
            },
            { QuestStage.QuestStages.RatLecture + "3", new List<string>()
                { "Student: Welcome to class."}
            },
            { QuestStage.QuestStages.RatLecture + "4", new List<string>()
                { "Student: What's up?"}
            },
            { QuestStage.QuestStages.RatStart + "1", new List<string>()
                { "Student: Gross, you're going into the dungeon?"}
            },
            { QuestStage.QuestStages.RatStart + "2", new List<string>()
                { "Student: Stay safe down there. Rats can be nasty."}
            },
            { QuestStage.QuestStages.RatStart + "3", new List<string>()
                { "Student: Make sure you have a heal spell ready"}
            },
            { QuestStage.QuestStages.RatStart + "4", new List<string>()
                { "Student: I hope you have a cleaning spell for your clothes when you're done."}
            },
            { QuestStage.QuestStages.RatFinished + "1", new List<string>() },
            { QuestStage.QuestStages.RatGraded + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeDorm + "1", new List<string>()
            },
            { QuestStage.QuestStages.SlimeLecture + "1", new List<string>()
                { "Student: I think we're learning a freeze spell today. That's cool."}
            },
            { QuestStage.QuestStages.SlimeLecture + "2", new List<string>()
                { "Student: How did the rat hunting go?"}
            },
            { QuestStage.QuestStages.SlimeLecture + "3", new List<string>()
                { "Student: Why am I still coming to class?"}
            },
            { QuestStage.QuestStages.SlimeLecture + "4", new List<string>()
                { "Student: I like Professor T's clothes. They're a nice colour"}
            },
            { QuestStage.QuestStages.SlimeStart + "1", new List<string>()
                { "Student: Didn't you clear out the rats before? The professors must hate you"}
            },
            { QuestStage.QuestStages.SlimeStart + "2", new List<string>()
                { "Student: Remember to freeze the slimes. You'll do more damage when you hit them after."}
            },
            { QuestStage.QuestStages.SlimeStart + "3", new List<string>()
                { "Student: I kind of want to clear the cave. Kind of."}
            },
            { QuestStage.QuestStages.SlimeStart + "4", new List<string>()
                { "Student: Why doesn't the professor do it?"}
            },
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
            { QuestStage.QuestStages.DemonDorm + "1",new List<string>()
            },
            { QuestStage.QuestStages.DemonStart + "1", new List<string>() },
            { QuestStage.QuestStages.DemonFinished + "1", new List<string>() },
            { QuestStage.QuestStages.HellStart + "1", new List<string>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<string>() },
            { QuestStage.QuestStages.HellFinished + "1", new List<string>() },
            { QuestStage.QuestStages.GameFinished + "1", new List<string>() },
        };
        playerResponses = new Dictionary<string, List<string>>
        {
        };
        playerResponsesAction = new Dictionary<string, List<Fragment>>()
        {
        };
    }

    public override void Interact(GameObject player)
    {
        OpenMenu.openMenu.Pause(false);
        DialogueButtons.speaker = this;
        DialogueButtons.dialogueButtons.textWindow.SetActive(true);
        DialogueButtons.dialogueButtons.goodbyeButton.interactable = false;
        DialogueButtons.dialogueButtons.nextButton.interactable = true;
        int randomLine = Random.Range(1, 5);
        currentState = QuestStage.QS.ToString() + randomLine.ToString();
        DialogueButtons.dialogueButtons.dialogue.text = GetNextLine();
    }
}
