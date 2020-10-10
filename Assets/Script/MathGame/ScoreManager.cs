using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;

    void Update()
    {
        scoreText.text = PlayerPrefs.GetInt("Math_Score").ToString();
    }
}
