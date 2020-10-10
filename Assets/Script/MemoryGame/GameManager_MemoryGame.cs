using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_MemoryGame : MonoBehaviour
{
    public List<Button> puzzleButtons = new List<Button>();
    public List<ButtonAnswer> puzzleButtonAnswers = new List<ButtonAnswer>();

    void Start()
    {
        GetButtons();
        RandomizeCorrectTile();
        ShowCorrectTileColor();
        StartCoroutine(RevertTileColor());
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
        int correctTileCount = 0;
        correctTileCount = System.Convert.ToInt32(System.Math.Sqrt(System.Convert.ToDouble(puzzleButtons.Count)));
        for(int i=0; i<puzzleButtons.Count; i++)
        {
            puzzleButtonAnswers.Add(
                new ButtonAnswer
                {
                    isCorrectAnswer = false
                }
            );
            if (correctTileCount>0)
            {
                if (puzzleButtons.Count - i <= correctTileCount)
                {
                    puzzleButtonAnswers[i].isCorrectAnswer = true;
                    correctTileCount -= 1;
                }
                else
                {
                    if (Random.value > 0.5f)
                    {
                        puzzleButtonAnswers[i].isCorrectAnswer = true;
                        correctTileCount -= 1;
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
        yield return new WaitForSeconds(3f);

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

    void UserSelectTile(bool isCorrectAnswer, Button currentButton)
    {
        currentButton.interactable = false;
        if (isCorrectAnswer)
        {
            var colors = currentButton.GetComponent<Button>().colors;
            colors.normalColor = Color.green;
            colors.pressedColor = Color.green;
            colors.disabledColor = Color.green;
            currentButton.GetComponent<Button>().colors = colors;
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
