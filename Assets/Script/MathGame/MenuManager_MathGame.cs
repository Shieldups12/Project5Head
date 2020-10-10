using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_MathGame : MonoBehaviour
{
    public void BackMath()
    {
        PlayerPrefs.DeleteKey("Math_Score");
        SceneManager.LoadScene(0);
    }
}
