using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_TriviaGame : MonoBehaviour
{
    public GameObject triviaPause;

    public void Restart()
    {
        SceneManager.LoadScene(3);
    }

    public void BackTrivia()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseMemory()
    {
        triviaPause.SetActive(true);
    }

    public void ContinueMemory()
    {
        triviaPause.SetActive(false);
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerPrefs.DeleteKey("Trivia_Score");
                SceneManager.LoadScene(0);
            }
        }
    }
}
