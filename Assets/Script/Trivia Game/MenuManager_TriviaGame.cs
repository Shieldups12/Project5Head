using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_TriviaGame : MonoBehaviour
{
    public void BackTrivia()
    {
        PlayerPrefs.DeleteKey("Trivia_Score");
        SceneManager.LoadScene(0);
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
