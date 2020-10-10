using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_MainMenu : MonoBehaviour
{
    public void PlayMath()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayMemory()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
