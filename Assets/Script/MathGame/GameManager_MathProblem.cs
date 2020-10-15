using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager_MathProblem : MonoBehaviour
{
    public static GameManager_MathProblem instance;

    public Question[] questions;
    private static List<Question> unansweredQuestions;

    private Question currentQuestion;

    public int playerScore = 0;

    [SerializeField]
    private TMP_Text mathProblemText;

    [SerializeField]
    private float timeBetweenQuestions = 0f;

    void Start()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        SetCurrentQuestion();

        playerScore = PlayerPrefs.GetInt("Math_Score", 0);
    }

    void Awake()
    {
        instance = this;
    }
    List<int> FindPossibleDivisor(int number)
    {
        List<int> divisorList = new List<int>();
        for (int i = 1; i <= number; i++)
        {
            if (number % i == 0)
            {
                divisorList.Add(i);
            }
        }
        return divisorList;
    }
    void SetCurrentQuestion()
    {
        //int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        //currentQuestion = unansweredQuestions[randomQuestionIndex];
        
        int mathSymbol = Random.Range(1, 5);
        int firstNumber = Random.Range(1, 21);
        int secondNumber = Random.Range(1, 21);
        float answerNumber = 0f;
        currentQuestion = new Question();
        Debug.Log(mathSymbol);
        switch (mathSymbol)
        {
            case 1:
                currentQuestion.answerPlus = true;
                break;
            case 2:
                currentQuestion.answerMinus = true;
                break;
            case 3:
                currentQuestion.answerDivide = true;
                break;
            case 4:
                currentQuestion.answerTimes = true;
                break;
        }

        if (currentQuestion.answerPlus)
        {
            answerNumber = firstNumber + secondNumber;
        }
        else if (currentQuestion.answerMinus)
        {
            answerNumber = firstNumber - secondNumber;
        }
        else if (currentQuestion.answerDivide)
        {
            List<int> possibleDivisor = new List<int>();
            System.Random rnd = new System.Random();
            possibleDivisor = FindPossibleDivisor(firstNumber);
            secondNumber = possibleDivisor[rnd.Next(0, possibleDivisor.Count)];
            answerNumber = (float)firstNumber / (float)secondNumber;
        }
        else if (currentQuestion.answerTimes)
        {
            answerNumber = firstNumber * secondNumber;
        }

        if(answerNumber == firstNumber + secondNumber)
        {
            currentQuestion.answerPlus = true;
        }
        if (answerNumber == firstNumber * secondNumber)
        {
            currentQuestion.answerTimes = true;
        }
        if (answerNumber == firstNumber - secondNumber)
        {
            currentQuestion.answerMinus = true;
        }
        if (answerNumber == (float)firstNumber / (float)secondNumber)
        {
            currentQuestion.answerDivide = true;
        }

        currentQuestion.mathProblem = firstNumber.ToString() + " ▢ " + secondNumber.ToString() + " = " + System.Convert.ToInt32(answerNumber).ToString();

        mathProblemText.text = currentQuestion.mathProblem;
    }

    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);

        yield return new WaitForSeconds(timeBetweenQuestions);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UserSelectPlus()
    {
        if (currentQuestion.answerPlus)
        {
            Debug.Log("Correct");
            playerScore += 10;
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        } else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    public void UserSelectMinus()
    {
        if (currentQuestion.answerMinus)
        {
            Debug.Log("Correct");
            playerScore += 10;
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }
    public void UserSelectTimes()
    {
        if (currentQuestion.answerTimes)
        {
            Debug.Log("Correct");
            playerScore += 10;
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }
    public void UserSelectDivide()
    {
        if (currentQuestion.answerDivide)
        {
            Debug.Log("Correct");
            playerScore += 10;
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Math_Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }
}
