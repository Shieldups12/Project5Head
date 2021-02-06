using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_MemoryGame : MonoBehaviour
{
    //screen
    public GameObject memoryEndScreen;
    public GameObject memoryPauseScreen;
    public GameObject memoryCountdownScreen;

    //audio
    public AudioSource yesSFX;
    public AudioSource noSFX;

    //countdown
    float countdownCurrentTime = 0f;
    float countdownStartTime = 3f;
    [SerializeField] private TMP_Text countdownText;

    //memory game
    public List<Button> puzzleButtons;
    public List<ButtonAnswer> puzzleButtonAnswers;
    public AddButtons addButton;
    public GridLayoutGroup gridLayoutGroup;
    public TMP_Text levelText;
    public TMP_Text correctTileCountText;
    public TMP_Text scoreText;
    
    [SerializeField] private int levelCount;
    [SerializeField] private int correctTileCount;
    [SerializeField] private int gridSize;
    [SerializeField] private float timeShowAnswer;
    [SerializeField] private int playerMemoryScore;

    //timer
    private float timeRemaining = 30f;
    public Slider sliderRight;
    public Slider sliderLeft;

    public Image sliderRightImage;
    public Image sliderLeftImage;
    private Color startColor = new Color(255, 255, 255, 1);
    private Color endColor = new Color(255, 255, 255, 0);
    private float startSliderTime;

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
        if (PlayerPrefs.GetInt("IsMemoryGameStart", 0) == 0)
        {
            CountdownTrigger();
        }
        SetLevel();
        SetCellSize();
        GetButtons();
        RandomizeCorrectTile();
        SetLevelUI();
        if (PlayerPrefs.GetInt("IsMemoryGameStart", 0) == 1)
        {
            ShowCorrectTileColor();
            StartCoroutine(RevertTileColor());
        }
        //startTime = Time.time;
        //isGenerated = false;
    }

    //bool isGenerated = false;
    //float startTime;
    void Update()
    {
        //float waitingTime = Time.time - startTime;
        //float t = 1 - Mathf.Pow((1 - (waitingTime)),2.0f);
        //if (waitingTime <= 1)
        //{
        //    for (int i = 0; i < puzzleButtons.Count; i++)
        //    {
        //        var colors = puzzleButtons[i].GetComponent<Button>().colors;
        //        if(colors.normalColor != Color.white)
        //        {
        //            colors.normalColor = Color.Lerp(Color.green, Color.white, t);
        //        }

        //        puzzleButtons[i].GetComponent<Button>().colors = colors;
        //        puzzleButtons[i].interactable = true;
        //    }
        //    Debug.Log("Test");
        //}
        //else
        //{
        //    if(!isGenerated)
        //    {
        //        Debug.Log("Generated");
        //        isGenerated = true;
        //        AddTileJudgement();
        //    }
        //}

        DoCountdown();
        if (memoryPauseScreen.activeSelf || memoryCountdownScreen.activeSelf == false)
        {
            sliderRight.value = timeRemaining;
            sliderLeft.value = timeRemaining;

            if (timeRemaining <= 5)
            {
                BlinkSlider();
            }
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

    void CountdownTrigger()
    {
        countdownCurrentTime = countdownStartTime;
        StartCoroutine(WaitBeforeShow());
    }
    void DoCountdown()
    {

        countdownCurrentTime -= 1 * Time.deltaTime;
        countdownText.text = countdownCurrentTime.ToString("0");

        if (countdownCurrentTime <= 0)
        {

            countdownCurrentTime = 0;
        }
    }

    void BlinkSlider()
    {
        float t = ((Mathf.Sin((Time.time - startSliderTime) * 9) / 2) + 0.5f);
        sliderLeftImage.color = Color.Lerp(startColor, endColor, t);
        sliderRightImage.color = Color.Lerp(startColor, endColor, t);
    }

    //countdown timer
    IEnumerator WaitBeforeShow()
    {
        memoryCountdownScreen.SetActive(true);
        yield return new WaitForSeconds(3);

        PlayerPrefs.SetInt("IsMemoryGameStart", 1);
        memoryCountdownScreen.SetActive(false);
        ShowCorrectTileColor();
        StartCoroutine(RevertTileColor());
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
