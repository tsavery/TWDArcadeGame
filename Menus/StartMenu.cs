using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    public void quitGame()
    {
        Application.Quit();
    }

    public void loadArena(string arena)
    {
        SceneManager.LoadScene(arena);
    }
}
