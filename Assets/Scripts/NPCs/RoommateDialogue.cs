using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoommateDialogue : NPC
{
    protected override void Start()
    {
        base.Start();
        dialogueLines = new Dictionary<string, List<string>>
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<string>()
                { "Roommate: Dude, wake up, you don’t want to miss your first class."}
            },
            { QuestStage.QuestStages.RatDorm + "2", new List<string>()
                { "Roommate: Yeah, man, what did you think you’d be doing at Mage University? It’s a uni, of course you’re going to have class!"}
            },
            { QuestStage.QuestStages.RatDorm + "3", new List<string>()
                { "Roommate: Late enough, how hard did you party last night? Doesn’t matter, you have to run if you’re going to get there on time, I bet."}
            },
            { QuestStage.QuestStages.RatDorm + "4", new List<string>()
                { "Roommate: Nah man, remember? I’m a junior. I don’t have to take the intro class that all the freshmen have to take. But you’re a freshman, so you gotta get to class quick! "}
            },
            { QuestStage.QuestStages.RatDorm + "5", new List<string>()
                { "Roommate: You know where the lecture halls are, right? The giant floating hat, yeah. Down the hall, out onto the field then take a left. Good luck!"}
            },
            { QuestStage.QuestStages.RatLecture + "1", new List<string>() },
            { QuestStage.QuestStages.RatStart + "1", new List<string>() },
            { QuestStage.QuestStages.RatFinished + "1", new List<string>() },
            { QuestStage.QuestStages.RatGraded + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeDorm + "1", new List<string>()
                { "Roommate: Yo, dawg, you okay? You look terrible! What happened in that intro class?"}
            },
            { QuestStage.QuestStages.SlimeDorm + "2", new List<string>()
                { "Dude, no way! They had you clear out rats from the dungeon? When I took that class, they just put me in the practice hall. That must have been pretty sweet!"}
            },
            { QuestStage.QuestStages.SlimeDorm + "3", new List<string>()
                { "Roommate: Yeah? Don’t want to talk about it any more? I gotcha."}
            },
            { QuestStage.QuestStages.SlimeDorm + "4", new List<string>()
                { "Roommate: So, what now? Got any plans for the rest of today?"}
            },
            { QuestStage.QuestStages.SlimeDorm + "5", new List<string>()
                { "Roommate: That’s fair. There’s plenty to see on campus, I’m sure you know. "}
            },
            { QuestStage.QuestStages.SlimeDorm + "6", new List<string>()
                { "Roommate: Haha, yeah, I gotcha. I’ll try to be quiet then. Hope you make it to class tomorrow!"}
            },
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
            { QuestStage.QuestStages.DemonDorm + "1",new List<string>()
                { "Roommate: Dude, you’re not gonna believe this. There are demons all over campus! Nobody’s doing anything about it, all of the professors are holed up in the administrative building talking about having to “protect the garden” or something. I’ve never been in that building, so I don’t know what’s up, but shouldn’t they be protecting the students first?",
                "Look, I see you getting ready to go out there, man, but...it’s demons. That’s scary, I think I’ll stay in here. If you’re gonna do something...best of luck."}
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
            { QuestStage.QuestStages.RatDorm + "1", new List<string>()
            {
                "Huh, class?",
                "You coming too? ",
                "I’d better get going.",
            }},
            { QuestStage.QuestStages.RatLecture + "1", new List<string>() },
            { QuestStage.QuestStages.RatStart + "1", new List<string>() },
            { QuestStage.QuestStages.RatFinished + "1", new List<string>() },
            { QuestStage.QuestStages.RatGraded + "1", new List<string>() },
            { QuestStage.QuestStages.SlimeDorm + "1", new List<string>()
            {
                "Well…",
                "It went alright.",
            }},
            { QuestStage.QuestStages.SlimeDorm + "4", new List<string>()
            {
                "Not really.",
                "Sleep, mostly.",
            }},
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
        playerResponsesAction = new Dictionary<string, List<Fragment>>()
        {
            { QuestStage.QuestStages.RatDorm + "1", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.RatDorm + "2"; },
                () => { currentState = QuestStage.QuestStages.RatDorm + "4"; },
                () => { currentState = QuestStage.QuestStages.RatDorm + "5"; }
            }},
            { QuestStage.QuestStages.RatDorm + "2", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.RatDorm + "1"; }
            }},
            { QuestStage.QuestStages.RatDorm + "3", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.RatDorm + "1"; }
            }},
            { QuestStage.QuestStages.RatDorm + "4", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.RatDorm + "1"; }
            }},
            { QuestStage.QuestStages.RatDorm + "5", new List<Fragment>()
            {
                () => { QuestStage.QS = QuestStage.QuestStages.RatLecture;
                        Exit(); }
            }},
            { QuestStage.QuestStages.RatLecture + "1", new List<Fragment>() },
            { QuestStage.QuestStages.RatStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.RatFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.RatGraded + "1", new List<Fragment>() },
            { QuestStage.QuestStages.SlimeDorm + "1", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.SlimeDorm + "2"; },
                () => { currentState = QuestStage.QuestStages.SlimeDorm + "3"; }
            }},
            { QuestStage.QuestStages.SlimeDorm + "2", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.SlimeDorm + "4"; }
            }},
            { QuestStage.QuestStages.SlimeDorm + "3", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.SlimeDorm + "4"; }
            }},
            { QuestStage.QuestStages.SlimeDorm + "4", new List<Fragment>()
            {
                () => { currentState = QuestStage.QuestStages.SlimeDorm + "5"; },
                () => { currentState = QuestStage.QuestStages.SlimeDorm + "6"; }
            }},
            { QuestStage.QuestStages.SlimeDorm + "5", new List<Fragment>()
            {
                () => { QuestStage.QS = QuestStage.QuestStages.SlimeLecture;
                        Exit(); }
            }},
            { QuestStage.QuestStages.SlimeDorm + "6", new List<Fragment>()
            {
                () => { QuestStage.QS = QuestStage.QuestStages.SlimeLecture;
                        Exit(); }
            }},
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
            { QuestStage.QuestStages.DemonDorm + "1", new List<Fragment>()
            {
                () => { QuestStage.QS = QuestStage.QuestStages.DemonStart;
                        Exit(); }
            } },
            { QuestStage.QuestStages.DemonStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.DemonFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellStart + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellBoss + "1", new List<Fragment>() },
            { QuestStage.QuestStages.HellFinished + "1", new List<Fragment>() },
            { QuestStage.QuestStages.GameFinished + "1", new List<Fragment>() },
        };
    }
}
