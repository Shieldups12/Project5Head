using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_MathGame : MonoBehaviour
{
    public void BackMath()
    {
        SceneManager.LoadScene(0);
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
