using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneNameList
{
    public static Dictionary<string, string> SceneNameToEcosystemName = new Dictionary<string, string>()
    {
        //{"F1_AbandonedHouse_Rat", "AbandonedHouse_Rat"},
        //{"B1_AbandonedHouse_Rat", "AbandonedHouse_Rat"},
        //{"B2_AbandonedHouse_Rat", "AbandonedHouse_Rat"},
        {"RatHouse", "RatHouse"},
        {"SlimeCave", "SlimeCave"},
        {"BoneZone", "BoneZone"},
        {"AndrewTest", "AndrewTest"},
        {"Campus_Environment", "Campus_Environment" },
        {"Campus", "Campus" }
    };
    public static Dictionary<string, string[]> EcosystemNameToSceneName = new Dictionary<string, string[]>()
    {
        //{"AbandonedHouse_Rat", new string[] { "F1_AbandonedHouse_Rat", "B1_AbandonedHouse_Rat", "B2_AbandonedHouse_Rat" } },
        {"RatHouse", new string[] { "RatHouse" } },
        {"SlimeCave", new string[] { "SlimeCave" } },
        {"BoneZone", new string[] { "BoneZone" } },
        {"AndrewTest", new string[] { "AndrewTest" } },
        {"Campus_Environment", new string[] { "Campus_Environment" } },
        {"Campus", new string[] { "Campus" } }
    };
    
    public static string[] GetSiblingScenes(string sceneName)
    {
        return EcosystemNameToSceneName[SceneNameToEcosystemName[sceneName]];
    }

}
