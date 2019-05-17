using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneChange : MonoBehaviour
{
    private const string masterSceneName = "MasterScene";
    Scene masterScene;
    Scene currentScene;

    string targetMarker;
    bool transport = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SceneManager.sceneLoaded += sceneLoadedListener;
        masterScene = SceneManager.GetSceneByName(masterSceneName);
        currentScene = masterScene;
    }

    private void sceneLoadedListener(Scene arg0, LoadSceneMode arg1)
    {
        rb.useGravity = true;
        if (transport)
        {
            GameObject marker = GameObject.Find(targetMarker);
            if (marker != null)
            {
                transform.position = marker.transform.position;
                transform.rotation = marker.transform.rotation;
            }
            else
                transform.position = Vector3.zero;
        }
        else
        {
            // If there is a spawn platform, spawn there. Otherwise, spawn at the origin
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
            if (spawns.Length == 0)
                transform.position.Set(0, 0, 0);
            else
            {
                GameObject spawn = spawns[Random.Range(0, spawns.Length - 1)];
                transform.position = spawn.transform.position;
                transform.rotation = spawn.transform.rotation;
            }
        }
        OpenMenu.openMenu.Resume();
    }

    public void LoadScene(string scene)
    {
        if (scene == "")
            scene = masterSceneName;
        if(currentScene != masterScene)
        {
            // Safely unload the player so we don't get deleted
            SceneManager.MoveGameObjectToScene(gameObject, masterScene);
            SceneManager.UnloadSceneAsync(currentScene);
        }

        if (scene != masterScene.name)
        {
            // Load up the current scene and move the player there
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
        currentScene = SceneManager.GetSceneByName(scene);
        SceneManager.MoveGameObjectToScene(gameObject, currentScene);

        transport = false;
    }

    public void Transport(string scene, string transportMarkerName)
    {
        if (currentScene.name != scene)
            LoadScene(scene);

        targetMarker = transportMarkerName;
        transport = true;
    }

    public void Restart()
    {
        LoadScene(masterSceneName);
        SceneManager.LoadScene(masterSceneName);
    }
}
