using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic music;

    public AudioClip[] main;
    public AudioClip[] campus;
    public AudioClip[] rat;
    public AudioClip[] ratBoss;
    public AudioClip[] slime;
    public AudioClip[] slimeBoss;
    public AudioClip[] demonCampus;
    public AudioClip[] hell;
    public AudioClip[] hellBoss;

    public AudioSource source;

    private Dictionary<QuestStage.QuestStages, AudioClip[]> dict;

    // Start is called before the first frame update
    void Start()
    {
        dict = new Dictionary<QuestStage.QuestStages, AudioClip[]>()
        {
            // Dungeons
            {QuestStage.QuestStages.RatStart, rat},
            {QuestStage.QuestStages.SlimeStart, slime},
            {QuestStage.QuestStages.DemonStart, demonCampus},
            {QuestStage.QuestStages.DemonFinished, demonCampus},
            {QuestStage.QuestStages.HellStart, hell},
            {QuestStage.QuestStages.HellBoss, hellBoss},
            // Campus
            {QuestStage.QuestStages.RatDorm, campus},
            {QuestStage.QuestStages.RatFinished, campus},
            {QuestStage.QuestStages.SlimeDorm, campus},
            {QuestStage.QuestStages.SlimeFinished, campus}
        };
        music = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwitchBackground(scene.name == "MasterScene");
    }

    public void SwitchBackground(bool title = false)
    {
        source.Pause();
        StopAllCoroutines();
        StartCoroutine(PlayLoop(title));
    }

    public void Pause()
    {
        source.Pause();
    }

    private IEnumerator PlayLoop(bool title = false)
    {
        AudioClip[] selection = title ? main : dict[QuestStage.QS];
        if(QuestStage.QS != QuestStage.QuestStages.HellBoss)
            Shuffle(selection);
        foreach (AudioClip clip in selection)
        {
            source.clip = clip;
            source.Play();
            yield return new WaitForSeconds(clip.length);
            source.Pause();
        }
        SwitchBackground(title);
    }

    public static void Shuffle(AudioClip[] list)
    {
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0,n);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
