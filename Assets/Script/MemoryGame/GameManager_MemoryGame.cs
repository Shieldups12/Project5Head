using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_MemoryGame : MonoBehaviour
{
    public List<Button> puzzleButtons;
    public List<ButtonAnswer> puzzleButtonAnswers;
    public AddButtons addButton;
    public GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int levelCount;
    [SerializeField] private int correctTileCount;
    [SerializeField] private int gridSize;
    [SerializeField] private float timeShowAnswer;
    private int currentCorrectAnswer;

    void Start()
    {
        SetLevel();
        SetCellSize();
        GetButtons();
        RandomizeCorrectTile();
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
        addButton.GenerateGrid(gridSize * gridSize);
        currentCorrectAnswer = 0;
        puzzleButtons = new List<Button>();
        puzzleButtonAnswers = new List<ButtonAnswer>();
        //if (levelCount >= 1 && levelCount <= 6)
        //{
        //    gridSize = 3;
        //    if (levelCount >= 1 && levelCount <= 3)
        //    {
        //        correctTileCount = 3;
        //    }
        //    else
        //    {
        //        correctTileCount = 4;
        //    }
        //}
        //else if (levelCount >= 7 && levelCount <= 12)
        //{
        //    gridSize = 4;
        //    if (levelCount >= 7 && levelCount <= 9)
        //    {
        //        correctTileCount = 5;
        //    }
        //    else
        //    {
        //        correctTileCount = 6;
        //    }
        //}
        //else if (levelCount >= 13 && levelCount <= 18)
        //{
        //    gridSize = 5;
        //    if (levelCount >= 13 && levelCount <= 15)
        //    {
        //        correctTileCount = 7;
        //    }
        //    else
        //    {
        //        correctTileCount = 8;
        //    }
        //}
        //dst
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
            var colors = currentButton.GetComponent<Button>().colors;
            colors.normalColor = Color.green;
            colors.pressedColor = Color.green;
            colors.disabledColor = Color.green;
            currentButton.GetComponent<Button>().colors = colors;
            CheckLevelComplete();
        }
        else
        {
            var colors = currentButton.GetComponent<Button>().colors;
            colors.normalColor = Color.red;
            colors.pressedColor = Color.red;
            colors.disabledColor = Color.red;
            currentButton.GetComponent<Button>().colors = colors;
        }
    }
}
