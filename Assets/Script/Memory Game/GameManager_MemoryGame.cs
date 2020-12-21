using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_MemoryGame : MonoBehaviour
{
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
    
    private int totalCorrect = 0;
    private int totalWrong = 0;
    private int currentCorrectAnswer;
    private int currentWrongAnswer;
    
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

    void SetLevelUI()
    {
        levelText.text = "Lv. " + levelCount.ToString();
        correctTileCountText.text = currentCorrectAnswer.ToString() + " / " + correctTileCount.ToString();
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
            gridLayoutGroup.cellSize = new Vector2(250, 250);
        }
        else if (gridSize == 4)
        {
            gridLayoutGroup.cellSize = new Vector2(200, 200);
        }
        else if (gridSize == 5)
        {
            gridLayoutGroup.cellSize = new Vector2(160, 160);
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
            playerMemoryScore += 10;

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
            currentCorrectAnswer++;
            totalCorrect++;

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
            totalWrong++;
            currentWrongAnswer++;
            var colors = currentButton.GetComponent<Button>().colors;
            colors.normalColor = Color.red;
            colors.pressedColor = Color.red;
            colors.disabledColor = Color.red;
            currentButton.GetComponent<Button>().colors = colors;

            CheckLevelFailed();
        }
    }
}
