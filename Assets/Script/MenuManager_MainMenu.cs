using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager_MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text recordMathFirstPlaceText;
    [SerializeField] private TMP_Text recordMathSecondPlaceText;
    [SerializeField] private TMP_Text recordMathThirdPlaceText;
    [SerializeField] private TMP_Text recordMathFourthPlaceText;

    public void PlayMath()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayMemory()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayTrivia()
    {
        SceneManager.LoadScene(3);
    }

    private void Start()
    {
        LoadMathRecord();
        LoadMemoryRecord();
        LoadTriviaRecord();
    }

    void LoadMathRecord()
    {
        int recordCount = PlayerPrefs.GetInt("mathGameRecordCount", 0);
        List<int> recordList = new List<int>();
        for (int i = 0; i < recordCount; i++)
        {
            recordList.Add(PlayerPrefs.GetInt("mathGameRecord_" + i.ToString()));
        }
        if (recordList.Count > 0)
        {
            recordList.Sort();
            recordList.Reverse();
            if (recordList.Count >= 4)
            {
                recordMathFirstPlaceText.text = recordList[0].ToString();
                recordMathSecondPlaceText.text = recordList[1].ToString();
                recordMathThirdPlaceText.text = recordList[2].ToString();
                recordMathFourthPlaceText.text = recordList[3].ToString();
            }
            else if (recordList.Count == 3)
            {
                recordMathFirstPlaceText.text = recordList[0].ToString();
                recordMathSecondPlaceText.text = recordList[1].ToString();
                recordMathThirdPlaceText.text = recordList[2].ToString();
                recordMathFourthPlaceText.text = "";
            }
            else if (recordList.Count == 2)
            {
                recordMathFirstPlaceText.text = recordList[0].ToString();
                recordMathSecondPlaceText.text = recordList[1].ToString();
                recordMathThirdPlaceText.text = "";
                recordMathFourthPlaceText.text = "";
            }
            else if (recordList.Count == 1)
            {
                recordMathFirstPlaceText.text = recordList[0].ToString();
                recordMathSecondPlaceText.text = "";
                recordMathThirdPlaceText.text = "";
                recordMathFourthPlaceText.text = "";
            }
            else
            {
                recordMathFirstPlaceText.text = "";
                recordMathSecondPlaceText.text = "";
                recordMathThirdPlaceText.text = "";
                recordMathFourthPlaceText.text = "";
            }
        }
        else
        {
            recordMathFirstPlaceText.text = "";
            recordMathSecondPlaceText.text = "";
            recordMathThirdPlaceText.text = "";
            recordMathFourthPlaceText.text = "";
        }
    }

    void LoadMemoryRecord()
    {

    }

    void LoadTriviaRecord()
    {

    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
