using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_MemoryGame : MonoBehaviour
{
    //universal variable
    public GameObject memoryEndScreen;
    public GameObject memoryPauseScreen;

    public AudioSource yesSFX;
    public AudioSource noSFX;

    public List<Button> puzzleButtons;
    public List<ButtonAnswer> puzzleButtonAnswers;
    public AddButtons addButton;
    public GridLayoutGroup gridLayoutGroup;
    public TMP_Text levelText;
    public TMP_Text correctTileCountText;
    public TMP_Text scoreText;

    //timer
    private float timeRemaining = 30f;
    public Slider sliderRight;
    public Slider sliderLeft;

    //memory game
    [SerializeField] private int levelCount;
    [SerializeField] private int correctTileCount;
    [SerializeField] private int gridSize;
    [SerializeField] private float timeShowAnswer;
    [SerializeField] private int playerMemoryScore;
    
    //endscreen
    private int totalCorrect = 0;
    private int totalWrong = 0;
    private int currentCorrectAnswer;
    private int currentWrongAnswer;
    [SerializeField] private TMP_Text correctText;
    [SerializeField] private TMP_Text wrongText;
    [SerializeField] private TMP_Text totalText;
    private string scoreResult;
    [SerializeField] private TMP_Text scoreResultText;
    [SerializeField] private TMP_Text recordFirstPlaceText;
    [SerializeField] private TMP_Text recordSecondPlaceText;
    [SerializeField] private TMP_Text recordThirdPlaceText;
    [SerializeField] private TMP_Text recordFourthPlaceText;
    [SerializeField] private Slider sliderCorrectWrongRatio;

    void Start()
    {
        SetLevel();
        SetCellSize();
        GetButtons();
        RandomizeCorrectTile();
        SetLevelUI();
        ShowCorrectTileColor();
        StartCoroutine(RevertTileColor());
    }

    void Update()
    {
        if (memoryPauseScreen.activeSelf == false)
        {
            sliderRight.value = timeRemaining;
            sliderLeft.value = timeRemaining;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
            }
            else if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
        }

        if (timeRemaining == 0)
        {
            if (memoryEndScreen.activeSelf == false)
            {
                FinalMemoryScore();
                ShowRecord();
                memoryEndScreen.SetActive(true);
            }
        }
    }

    void SetLevel()
    {
        if(levelCount % 6 == 0)
        {
            gridSize = levelCount / 6 + 3 - 1;
        }
        else
        {
            gridSize = levelCount / 6 + 3;
        }

        if (levelCount % 3 == 0)
        {
            correctTileCount = levelCount / 3 + 3 - 1;
        }
        else
        {
            correctTileCount = levelCount / 3 + 3;
        }

        if(levelCount >= 18)
        {
            gridSize = 5;
            correctTileCount = 9;
        }

        addButton.GenerateGrid(gridSize * gridSize);
        currentCorrectAnswer = 0;
        currentWrongAnswer = 0;
        puzzleButtons = new List<Button>();
        puzzleButtonAnswers = new List<ButtonAnswer>();
    }

    void SetCellSize()
    {
        Debug.Log(Screen.currentResolution);
        if (gridSize == 3)
        {
            gridLayoutGroup.cellSize = new Vector2(255, 255);
        }
        else if (gridSize == 4)
        {
            gridLayoutGroup.cellSize = new Vector2(185, 185);
        }
        else if (gridSize == 5)
        {
            gridLayoutGroup.cellSize = new Vector2(145, 145);
        }
        //dst
    }

    void GetButtons()
    {
        
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton_Tag");
        for (int i = 0; i < objects.Length; i++)
        {
            puzzleButtons.Add(objects[i].GetComponent<Button>());
        }
    }
    void RandomizeCorrectTile()
    {
        int tempCorrectTileCount = correctTileCount;

        for(int i=0; i<puzzleButtons.Count; i++)
        {
            puzzleButtonAnswers.Add(
                new ButtonAnswer
                {
                    isCorrectAnswer = false
                }
            );
            if (tempCorrectTileCount > 0)
            {
                if (puzzleButtons.Count - i <= tempCorrectTileCount)
                {
                    puzzleButtonAnswers[i].isCorrectAnswer = true;
                    tempCorrectTileCount -= 1;
                }
                else
                {
                    if (Random.value > 0.5f)
                    {
                        puzzleButtonAnswers[i].isCorrectAnswer = true;
                        tempCorrectTileCount -= 1;
                    }
                }
            }
        }
    }

    void SetLevelUI()
    {
        levelText.text = "Lv. " + levelCount.ToString();
        correctTileCountText.text = currentCorrectAnswer.ToString() + " / " + correctTileCount.ToString();
    }

    void ShowCorrectTileColor()
    {      
        for (int i = 0; i < puzzleButtons.Count; i++)
        {
            if (puzzleButtonAnswers[i].isCorrectAnswer)
            {
                var colors = puzzleButtons[i].GetComponent<Button>().colors;
                colors.normalColor = Color.green;
                puzzleButtons[i].GetComponent<Button>().colors = colors;
            }
        }
    }

    IEnumerator RevertTileColor()
    {
        yield return new WaitForSeconds(timeShowAnswer);

        for (int i = 0; i < puzzleButtons.Count; i++)
        {
            var colors = puzzleButtons[i].GetComponent<Button>().colors;
            colors.normalColor = Color.white;
            puzzleButtons[i].GetComponent<Button>().colors = colors;
            puzzleButtons[i].interactable = true;
        }
        AddTileJudgement();
    }

    void AddTileJudgement()
    {
        for(int i=0; i<puzzleButtons.Count; i++)
        {
            bool isCorrectAnswer = puzzleButtonAnswers[i].isCorrectAnswer;
            Button currentButton = puzzleButtons[i];
            puzzleButtons[i].onClick.AddListener(
                delegate 
                {
                    UserSelectTile(isCorrectAnswer, currentButton);
                }
            );
        }
    }

    void CheckLevelComplete()
    {
        Debug.Log("Correct Answer: "+correctTileCount.ToString() + "Current Answer: "+ currentCorrectAnswer.ToString());
        if(correctTileCount == currentCorrectAnswer)
        {
            levelCount++;
            totalCorrect++;
            playerMemoryScore += 10;

            if (totalCorrect % 5 == 0 && totalCorrect > 0)
            {
                AddBonusTime();
            }

            scoreText.text = playerMemoryScore.ToString();

            Debug.Log("Destroy");
            addButton.DestroyGrid();
            Start();
        }
    }

    void CheckLevelFailed()
    {
        if(currentWrongAnswer >= 3)
        {
            totalWrong++;
            timeRemaining--;
            Debug.Log("Destroy");
            addButton.DestroyGrid();
            Start();
        }
    }

    void UserSelectTile(bool isCorrectAnswer, Button currentButton)
    {
        currentButton.interactable = false;
        if (isCorrectAnswer)
        {
            yesSFX.Play();
            currentCorrectAnswer++;
            var colors = currentButton.GetComponent<Button>().colors;
            colors.normalColor = Color.green;
            colors.pressedColor = Color.green;
            colors.disabledColor = Color.green;
            currentButton.GetComponent<Button>().colors = colors;

            correctTileCountText.text = currentCorrectAnswer.ToString() + " / " + correctTileCount.ToString();

            CheckLevelComplete();
        }
        else
        {
            noSFX.Play();
            currentWrongAnswer++;
            var colors = currentButton.GetComponent<Button>().colors;
            colors.normalColor = Color.red;
            colors.pressedColor = Color.red;
            colors.disabledColor = Color.red;
            currentButton.GetComponent<Button>().colors = colors;

            CheckLevelFailed();
        }
    }

    void AddBonusTime()
    {
        if (timeRemaining >= 30f)
        {
            timeRemaining = 30f;
        }
        else
        {
            timeRemaining++;
        }
        sliderRight.value = timeRemaining;
        sliderLeft.value = timeRemaining;
    }

    public void FinalMemoryScore()
    {
        if (playerMemoryScore >= 180)
        {
            scoreResult = "A+";
        }
        else if (playerMemoryScore >= 150)
        {
            scoreResult = "A";
        }
        else if (playerMemoryScore >= 120)
        {
            scoreResult = "B";
        }
        else
        {
            scoreResult = "C";
        }
        scoreResultText.text = scoreResult;
        int totalAnsweredQuestion = totalCorrect + totalWrong;
        float correctWrongRatio = ((float)totalCorrect / (float)totalAnsweredQuestion) * 100f;
        sliderCorrectWrongRatio.value = correctWrongRatio;
        correctText.text = totalCorrect.ToString();
        wrongText.text = totalWrong.ToString();
        totalText.text = playerMemoryScore.ToString();
    }

    void ShowRecord()
    {
        //save surrent session score
        int recordCount = PlayerPrefs.GetInt("memoryGameRecordCount", 0);
        PlayerPrefs.SetInt("memoryGameRecord_" + recordCount.ToString(), playerMemoryScore);
        recordCount++;
        PlayerPrefs.SetInt("memoryGameRecordCount", recordCount);

        //load all score
        List<int> recordList = new List<int>();
        for (int i = 0; i < recordCount; i++)
        {
            recordList.Add(PlayerPrefs.GetInt("memoryGameRecord_" + i.ToString()));
        }

        //display top 4 score
        if (recordList.Count > 0)
        {
            recordList.Sort();
            recordList.Reverse();
            if (recordList.Count >= 4)
            {
                recordFirstPlaceText.text = recordList[0].ToString();
                recordSecondPlaceText.text = recordList[1].ToString();
                recordThirdPlaceText.text = recordList[2].ToString();
                recordFourthPlaceText.text = recordList[3].ToString();
            }
            else if (recordList.Count == 3)
            {
                recordFirstPlaceText.text = recordList[0].ToString();
                recordSecondPlaceText.text = recordList[1].ToString();
                recordThirdPlaceText.text = recordList[2].ToString();
                recordFourthPlaceText.text = "";
            }
            else if (recordList.Count == 2)
            {
                recordFirstPlaceText.text = recordList[0].ToString();
                recordSecondPlaceText.text = recordList[1].ToString();
                recordThirdPlaceText.text = "";
                recordFourthPlaceText.text = "";
            }
            else if (recordList.Count == 1)
            {
                recordFirstPlaceText.text = recordList[0].ToString();
                recordSecondPlaceText.text = "";
                recordThirdPlaceText.text = "";
                recordFourthPlaceText.text = "";
            }
            else
            {
                recordFirstPlaceText.text = "";
                recordSecondPlaceText.text = "";
                recordThirdPlaceText.text = "";
                recordFourthPlaceText.text = "";
            }
        }
        else
        {
            recordFirstPlaceText.text = "";
            recordSecondPlaceText.text = "";
            recordThirdPlaceText.text = "";
            recordFourthPlaceText.text = "";
        }
    }
}
