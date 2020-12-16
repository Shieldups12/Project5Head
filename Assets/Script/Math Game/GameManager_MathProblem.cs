using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager_MathProblem : MonoBehaviour
{
    public GameObject mathEndScreen;

    public static GameManager_MathProblem instance;
    public int playerMathScore = 0;

    public MathQuestion[] mathQuestion;
    private MathQuestion currentQuestion;
    private static List<MathQuestion> unansweredQuestions;

    [SerializeField] private TMP_Text mathText;
    [SerializeField] private float timeBetweenQuestions;

    //math problem variable
    int mathSymbol;
    int firstNumber;
    int secondNumber;
    float answerNumber;

    //timer variable
    private float timeRemaining = 30f;
    public Slider sliderRight;
    public Slider sliderLeft;

    //endscreen
    private int totalCorrect = 0;
    private int totalWrong = 0;
    [SerializeField] private TMP_Text correctText;
    [SerializeField] private TMP_Text wrongText;
    [SerializeField] private TMP_Text totalText;
    private string scoreResult;
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = mathQuestion.ToList<MathQuestion>();
        }

        RandomizeMathQuestion();
        SetMathQuestion();
        CheckMathAnswer();
        DisplayMathQuestion();
        FinalMathScore();

        playerMathScore = PlayerPrefs.GetInt("Math_Score", 0);
    }

    //void Awake()
    //{
    //    instance = this;
    //}

    //check for possible comma answer for division
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

    //timer countdown
    void Update()
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

        if (timeRemaining == 0)
        {
            mathEndScreen.SetActive(true);
            //PlayerPrefs.DeleteKey("Math_Score");
            //SceneManager.LoadScene(0);
        }
    }

    void RandomizeMathQuestion()
    {
        mathSymbol = Random.Range(1, 5);
        firstNumber = Random.Range(1, 21);
        secondNumber = Random.Range(1, 21);
        answerNumber = 0f;

        currentQuestion = new MathQuestion();

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
                currentQuestion.answerTimes = true;
                break;
            case 4:
                currentQuestion.answerDivide = true;
                break;
        }

    }

    //set current question answer
    void SetMathQuestion()
    {
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
    }

    //check answer if there's possible way for two answer
    void CheckMathAnswer()
    {
        if (answerNumber == firstNumber + secondNumber)
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
    }

    //display question on screen
    void DisplayMathQuestion()
    {
        currentQuestion.mathProblem = firstNumber.ToString() + " ▢ " + secondNumber.ToString() + " = " + System.Convert.ToInt32(answerNumber).ToString();

        mathText.text = currentQuestion.mathProblem;
    }

    //transition to the next question (0s)
    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);

        yield return new WaitForSeconds(timeBetweenQuestions);

        Invoke("Start", 0f);
    }

    //button click plus answer
    public void UserSelectPlus()
    {
        if (currentQuestion.answerPlus)
        {
            Debug.Log("Correct");
            playerMathScore += 10;
            totalCorrect++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            totalWrong++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //button click minus answer
    public void UserSelectMinus()
    {
        if (currentQuestion.answerMinus)
        {
            Debug.Log("Correct");
            playerMathScore += 10;
            totalCorrect++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            totalWrong++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //button click times answer
    public void UserSelectTimes()
    {
        if (currentQuestion.answerTimes)
        {
            Debug.Log("Correct");
            playerMathScore += 10;
            totalCorrect++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            totalWrong++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //button click divide answer
    public void UserSelectDivide()
    {
        if (currentQuestion.answerDivide)
        {
            Debug.Log("Correct");
            playerMathScore += 10;
            totalCorrect++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            totalWrong++;
            PlayerPrefs.SetInt("Math_Score", playerMathScore);
            StartCoroutine(TransitionToNextQuestion());
        }
    }
    
    public void FinalMathScore()
    {
        if (playerMathScore >= 180)
        {
            scoreResult = "A+";
        }
        if (playerMathScore >= 150)
        {
            scoreResult = "A";
        }
        else if (playerMathScore >=120)
        {
            scoreResult = "B";
        }
        else
        {
            scoreResult = "C";
        }
        scoreText.text = scoreResult;

        correctText.text = totalCorrect.ToString();
        wrongText.text = totalWrong.ToString();
        totalText.text = playerMathScore.ToString();
    }
}
