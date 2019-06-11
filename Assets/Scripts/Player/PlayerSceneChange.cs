using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneChange : MonoBehaviour
{
    private const string masterSceneName = "MasterScene";
    Scene masterScene;
    List<Scene> currentScenes = new List<Scene>();
    Scene activeScene;

    string targetMarker = "";

    private Rigidbody rb;
    private bool doTransport = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SceneManager.sceneLoaded += sceneLoadedListener;
        SceneManager.sceneUnloaded += sceneUnloadedListener;
        masterScene = SceneManager.GetSceneByName(masterSceneName);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= sceneLoadedListener;
        SceneManager.sceneUnloaded -= sceneUnloadedListener;
    }

    private void sceneLoadedListener(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0 == activeScene && doTransport)
        {
            rb.useGravity = true;
            SceneManager.MoveGameObjectToScene(gameObject, activeScene);
            
            TransportMarker();

            OpenMenu.openMenu.Resume();
        }
    }

    private void sceneUnloadedListener(Scene arg0)
    {
        currentScenes.Remove(arg0);
    }

    private void TransportMarker()
    {
        // Set the player to be in the active scene
        SceneManager.MoveGameObjectToScene(gameObject, activeScene);

        // Try to spawn at the marker
        GameObject marker = GameObject.Find(targetMarker);
        if (marker != null)
        {
            targetMarker = "";
            transform.position = marker.transform.position;
            transform.rotation = marker.transform.rotation;
        }
        else
        {
            // If there is a spawn platform, spawn there. Otherwise, spawn at the origin
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
            if (spawns.Length != 0)
            {
                GameObject spawn = spawns[Random.Range(0, spawns.Length - 1)];
                transform.position = spawn.transform.position;
                transform.rotation = spawn.transform.rotation;
            }
            else
            {
                transform.position.Set(0, 0, 0);
            }
        }
    }

    public void LoadScene(string scene)
    {
        BackgroundMusic.music.Pause();
        if (scene == "")
            scene = masterSceneName;

        // Safely unload the player so we don't get deleted
        SceneManager.MoveGameObjectToScene(gameObject, masterScene);
        foreach(Scene s in currentScenes)
        {
            SceneManager.UnloadSceneAsync(s);
        }

        if (scene != masterScene.name)
        {
            // Find all of the scenes in the selected scene's ecosystem
            string[] sceneNames = SceneNameList.GetSiblingScenes(scene);

            doTransport = true;
            // Load up the current scenes and move the player there
            foreach (string sceneName in sceneNames)
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
                currentScenes.Add(SceneManager.GetSceneByName(sceneName));
            }
            //TransportMarker();
        }
        else
        {
            BackgroundMusic.music.SwitchBackground(true);
            doTransport = false;
        }

        activeScene = SceneManager.GetSceneByName(scene);
    }

    public void Transport(string scene, string transportMarkerName)
    {
        targetMarker = transportMarkerName;
        doTransport = true;

        bool isSceneLoaded = false;
        foreach (Scene s in currentScenes)
            isSceneLoaded |= s.name == scene;

        if (!isSceneLoaded)
            LoadScene(scene);
        else
        {
            if (scene != activeScene.name)
                activeScene = SceneManager.GetSceneByName(scene);
            TransportMarker();
        }
    }

    public void Restart()
    {
        SceneManager.sceneLoaded -= sceneLoadedListener;
        SceneManager.LoadScene(masterSceneName, LoadSceneMode.Single);
    }

    public void DeathReload()
    {
        GetComponent<StatScript>().AIReset();
        PlayerController pc = GetComponent<PlayerController>();
        pc.NumKeys = 0;
        pc.HealthPotionCount = 0;
        pc.ManaPotionCount = 0;
        string sceneToLoad = "MasterScene";
        switch(QuestStage.Quest)
        {
            case QuestStage.Quests.Rat:
                sceneToLoad = "RatHouse";
                break;
            case QuestStage.Quests.Slime:
                sceneToLoad = "SlimeCave";
                break;
            case QuestStage.Quests.Demon:
                sceneToLoad = "Campus";
                break;
            case QuestStage.Quests.Hell:
                sceneToLoad = "Hell";
                break;
        }
        LoadScene(sceneToLoad);
    }

    public bool CampusScene()
    {
        bool ret = false;
        foreach(Scene s in currentScenes)
        {
            ret = ret || s.name == "Campus";
        }
        return ret;
    }
}
