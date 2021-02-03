using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_MemoryGame : MonoBehaviour
{
    public GameObject memoryPause;

    public void Restart()
    {
        PlayerPrefs.SetInt("IsMemoryGameStart", 0);
        SceneManager.LoadScene(2);
    }

    public void BackMemory()
    {
        PlayerPrefs.SetInt("IsMemoryGameStart", 0);
        SceneManager.LoadScene(0);
    }

    public void PauseMemory()
    {
        memoryPause.SetActive(true);
    }

    public void ContinueMemory()
    {
        memoryPause.SetActive(false);
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
