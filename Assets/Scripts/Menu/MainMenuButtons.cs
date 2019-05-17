using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //For SceneManager and reloading the scene after death

public class MainMenuButtons : MonoBehaviour
{
    public PlayerHolder playerHolder;
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Resume()
    {
        OpenMenu.openMenu.Resume();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Load(string scene)
    {
        playerHolder.player.GetComponent<PlayerSceneChange>().LoadScene(scene);
    }
}
