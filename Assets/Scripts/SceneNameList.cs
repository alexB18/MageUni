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
        {"F1_AbandonedHouse_Rat", "F1_AbandonedHouse_Rat"},
        {"B1_AbandonedHouse_Rat", "B1_AbandonedHouse_Rat"},
        {"B2_AbandonedHouse_Rat", "B2_AbandonedHouse_Rat"},
        {"AndrewTest", "AndrewTest"},
        {"Campus_Environment", "Campus_Environment" }
    };
    public static Dictionary<string, string[]> EcosystemNameToSceneName = new Dictionary<string, string[]>()
    {
        //{"AbandonedHouse_Rat", new string[] { "F1_AbandonedHouse_Rat", "B1_AbandonedHouse_Rat", "B2_AbandonedHouse_Rat" } },
        {"F1_AbandonedHouse_Rat", new string[] { "F1_AbandonedHouse_Rat" } },
        {"B1_AbandonedHouse_Rat", new string[] { "B1_AbandonedHouse_Rat" } },
        {"B2_AbandonedHouse_Rat", new string[] { "B2_AbandonedHouse_Rat" } },
        {"AndrewTest", new string[] { "AndrewTest" } },
        {"Campus_Environment", new string[] { "Campus_Environment" } }
    };
    
    public static string[] GetSiblingScenes(string sceneName)
    {
        return EcosystemNameToSceneName[SceneNameToEcosystemName[sceneName]];
    }

}
