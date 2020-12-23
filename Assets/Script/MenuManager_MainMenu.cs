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

    [SerializeField] private TMP_Text recordMemoryFirstPlaceText;
    [SerializeField] private TMP_Text recordMemorySecondPlaceText;
    [SerializeField] private TMP_Text recordMemoryThirdPlaceText;
    [SerializeField] private TMP_Text recordMemoryFourthPlaceText;

    [SerializeField] private TMP_Text recordTriviaFirstPlaceText;
    [SerializeField] private TMP_Text recordTriviaSecondPlaceText;
    [SerializeField] private TMP_Text recordTriviaThirdPlaceText;
    [SerializeField] private TMP_Text recordTriviaFourthPlaceText;

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
        int recordCount = PlayerPrefs.GetInt("mathProblemRecordCount", 0);
        List<int> recordList = new List<int>();
        for (int i = 0; i < recordCount; i++)
        {
            recordList.Add(PlayerPrefs.GetInt("mathProblemRecord_" + i.ToString()));
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
        int recordCount = PlayerPrefs.GetInt("memoryGameRecordCount", 0);
        List<int> recordList = new List<int>();
        for (int i = 0; i < recordCount; i++)
        {
            recordList.Add(PlayerPrefs.GetInt("memoryGameRecord_" + i.ToString()));
        }
        if (recordList.Count > 0)
        {
            recordList.Sort();
            recordList.Reverse();
            if (recordList.Count >= 4)
            {
                recordMemoryFirstPlaceText.text = recordList[0].ToString();
                recordMemorySecondPlaceText.text = recordList[1].ToString();
                recordMemoryThirdPlaceText.text = recordList[2].ToString();
                recordMemoryFourthPlaceText.text = recordList[3].ToString();
            }
            else if (recordList.Count == 3)
            {
                recordMemoryFirstPlaceText.text = recordList[0].ToString();
                recordMemorySecondPlaceText.text = recordList[1].ToString();
                recordMemoryThirdPlaceText.text = recordList[2].ToString();
                recordMemoryFourthPlaceText.text = "";
            }
            else if (recordList.Count == 2)
            {
                recordMemoryFirstPlaceText.text = recordList[0].ToString();
                recordMemorySecondPlaceText.text = recordList[1].ToString();
                recordMemoryThirdPlaceText.text = "";
                recordMemoryFourthPlaceText.text = "";
            }
            else if (recordList.Count == 1)
            {
                recordMemoryFirstPlaceText.text = recordList[0].ToString();
                recordMemorySecondPlaceText.text = "";
                recordMemoryThirdPlaceText.text = "";
                recordMemoryFourthPlaceText.text = "";
            }
            else
            {
                recordMemoryFirstPlaceText.text = "";
                recordMemorySecondPlaceText.text = "";
                recordMemoryThirdPlaceText.text = "";
                recordMemoryFourthPlaceText.text = "";
            }
        }
        else
        {
            recordMemoryFirstPlaceText.text = "";
            recordMemorySecondPlaceText.text = "";
            recordMemoryThirdPlaceText.text = "";
            recordMemoryFourthPlaceText.text = "";
        }
    }

    void LoadTriviaRecord()
    {
        int recordCount = PlayerPrefs.GetInt("triviaProblemRecordCount", 0);
        List<int> recordList = new List<int>();
        for (int i = 0; i < recordCount; i++)
        {
            recordList.Add(PlayerPrefs.GetInt("triviaProblemRecord_" + i.ToString()));
        }
        if (recordList.Count > 0)
        {
            recordList.Sort();
            recordList.Reverse();
            if (recordList.Count >= 4)
            {
                recordTriviaFirstPlaceText.text = recordList[0].ToString();
                recordTriviaSecondPlaceText.text = recordList[1].ToString();
                recordTriviaThirdPlaceText.text = recordList[2].ToString();
                recordTriviaFourthPlaceText.text = recordList[3].ToString();
            }
            else if (recordList.Count == 3)
            {
                recordTriviaFirstPlaceText.text = recordList[0].ToString();
                recordTriviaSecondPlaceText.text = recordList[1].ToString();
                recordTriviaThirdPlaceText.text = recordList[2].ToString();
                recordTriviaFourthPlaceText.text = "";
            }
            else if (recordList.Count == 2)
            {
                recordTriviaFirstPlaceText.text = recordList[0].ToString();
                recordTriviaSecondPlaceText.text = recordList[1].ToString();
                recordTriviaThirdPlaceText.text = "";
                recordTriviaFourthPlaceText.text = "";
            }
            else if (recordList.Count == 1)
            {
                recordTriviaFirstPlaceText.text = recordList[0].ToString();
                recordTriviaSecondPlaceText.text = "";
                recordTriviaThirdPlaceText.text = "";
                recordTriviaFourthPlaceText.text = "";
            }
            else
            {
                recordTriviaFirstPlaceText.text = "";
                recordTriviaSecondPlaceText.text = "";
                recordTriviaThirdPlaceText.text = "";
                recordTriviaFourthPlaceText.text = "";
            }
        }
        else
        {
            recordTriviaFirstPlaceText.text = "";
            recordTriviaSecondPlaceText.text = "";
            recordTriviaThirdPlaceText.text = "";
            recordTriviaFourthPlaceText.text = "";
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
