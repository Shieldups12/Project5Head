using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager_TriviaGame : MonoBehaviour
{
    public GameObject triviaPause;
    public TMP_Text questionText;

    public void Restart()
    {
        PlayerPrefs.SetInt("IsTriviaGameStart", 0);
        SceneManager.LoadScene(3);
    }

    public void BackTrivia()
    {
        PlayerPrefs.SetInt("IsTriviaGameStart", 0);
        SceneManager.LoadScene(0);
    }

    public void PauseMemory()
    {
        triviaPause.SetActive(true);
        questionText.gameObject.SetActive(false);
    }

    public void ContinueMemory()
    {
        triviaPause.SetActive(false);
        questionText.gameObject.SetActive(true);
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
