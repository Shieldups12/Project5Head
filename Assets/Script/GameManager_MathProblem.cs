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

        playerScore = PlayerPrefs.GetInt("Player Score", 0);
    }

    void Awake()
    {
        instance = this;
    }

    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        ////1 = +
        ////2 = -
        ////3 = /
        ////4 = X
        //int mathSymbol = Random.Range(1, 5);
        //int firstNumber = Random.Range(1, 20);
        //int secondNumber = Random.Range(1, 20);
        //float answerNumber = 0f;
        //currentQuestion = new Question();
        //Debug.Log(mathSymbol);
        //switch (mathSymbol)
        //{
        //    case 1:
        //        currentQuestion.answerPlus = true;
        //        break;
        //    case 2:
        //        currentQuestion.answerMinus = true;
        //        break;
        //    case 3:
        //        currentQuestion.answerDivide = true;
        //        break;
        //    case 4:
        //        currentQuestion.answerTimes = true;
        //        break;
        //}

        //if (currentQuestion.answerPlus)
        //{
        //    answerNumber = firstNumber + secondNumber;
        //}
        //else if (currentQuestion.answerMinus)
        //{
        //    answerNumber = firstNumber - secondNumber;
        //}
        //else if (currentQuestion.answerDivide)
        //{
        //    answerNumber = (float)firstNumber / (float)secondNumber;
        //}
        //else if (currentQuestion.answerTimes)
        //{
        //    answerNumber = firstNumber * secondNumber;
        //}

        //if (currentQuestion.answerDivide)
        //{
        //    currentQuestion.mathProblem = firstNumber.ToString() + " ▢ " + secondNumber.ToString() + " = " + System.Math.Round(answerNumber, 2);
        //}
        //else
        //{
        //    currentQuestion.mathProblem = firstNumber.ToString() + " ▢ " + secondNumber.ToString() + " = " + System.Convert.ToInt32(answerNumber).ToString();
        //}

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
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        } else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    public void UserSelectMinus()
    {
        if (currentQuestion.answerMinus)
        {
            Debug.Log("Correct");
            playerScore += 10;
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }
    public void UserSelectTimes()
    {
        if (currentQuestion.answerTimes)
        {
            Debug.Log("Correct");
            playerScore += 10;
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }
    public void UserSelectDivide()
    {
        if (currentQuestion.answerDivide)
        {
            Debug.Log("Correct");
            playerScore += 10;
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            PlayerPrefs.SetInt("Player Score", playerScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }
}
