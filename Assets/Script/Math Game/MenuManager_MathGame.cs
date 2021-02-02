using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager_MathGame : MonoBehaviour
{
    public GameObject mathPause;
    [SerializeField] private TMP_Text mathText;

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void BackMath()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseMath()
    {
        mathPause.SetActive(true);
        mathText.gameObject.SetActive(false);
    }

    public void ContinueMath()
    {
        mathPause.SetActive(false);
        mathText.gameObject.SetActive(true);
    }

    public void DebugResetAllRecords()
    {
        int recordCount = PlayerPrefs.GetInt("mathProblemRecordCount", 0);
        for (int i = 0; i < recordCount; i++)
        {
            PlayerPrefs.DeleteKey("mathProblemRecord_" + i.ToString());
        }
        PlayerPrefs.SetInt("mathProblemRecordCount", 0);
        Debug.Log("All Record has deleted!");
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
