using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCreationStart : MonoBehaviour
{
    public void OnEnable()
    {
        if (!QuestStage.DidSpellCreation)
        {
            QuestStage.DidSpellCreation = true;
            OpenMenu.openMenu.SpellCreationTutorial();
        }
    }
}
