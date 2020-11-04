using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreMathText;

    void Update()
    {
        scoreMathText.text = PlayerPrefs.GetInt("Math_Score").ToString();
    }
}
