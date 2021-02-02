using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager_MathProblem : MonoBehaviour
{
    //universal
    public GameObject mathEndScreen;
    public GameObject mathPauseScreen;

    public AudioSource yesSFX;
    public AudioSource noSFX;

    public static GameManager_MathProblem instance;
    [SerializeField] private int playerMathScore = 0;
    [SerializeField] private TMP_Text scoreText;

    public MathQuestion[] mathQuestion;
    private MathQuestion currentQuestion;
    private static List<MathQuestion> unansweredQuestions;

    [SerializeField] private TMP_Text mathText;
    [SerializeField] private float timeBetweenQuestions;

    //math game
    int mathSymbol;
    int firstNumber;
    int secondNumber;
    float answerNumber;

    //timer
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
    [SerializeField] private TMP_Text scoreResultText;
    [SerializeField] private TMP_Text recordFirstPlaceText;
    [SerializeField] private TMP_Text recordSecondPlaceText;
    [SerializeField] private TMP_Text recordThirdPlaceText;
    [SerializeField] private TMP_Text recordFourthPlaceText;
    [SerializeField] private Slider sliderCorrectWrongRatio;

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
    }

    void Update()
    {
        //time slider
        if(mathPauseScreen.activeSelf == false)
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

        //check time ends
        if (timeRemaining == 0)
        {
            if(mathEndScreen.activeSelf == false)
            {
                ShowRecord();
                mathEndScreen.SetActive(true);
            }
        }
    }

    //randomize math question
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

    //set current math answer
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

    //check for two possible answer
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

    //check comma for division question
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

    //display question on screen
    void DisplayMathQuestion()
    {
        currentQuestion.mathProblem = firstNumber.ToString() + " ▢ " + secondNumber.ToString() + " = " + System.Convert.ToInt32(answerNumber).ToString();

        mathText.text = currentQuestion.mathProblem;
        scoreText.text = playerMathScore.ToString();
    }

    //button click plus answer
    public void UserSelectPlus()
    {
        if (currentQuestion.answerPlus)
        {
            Debug.Log("Correct");
            yesSFX.Play();
            playerMathScore += 10;
            totalCorrect++;
            if (totalCorrect % 5 == 0 && totalCorrect > 0)
            {
                AddBonusTime();
            }
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            noSFX.Play();
            timeRemaining--;
            totalWrong++;
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //button click minus answer
    public void UserSelectMinus()
    {
        if (currentQuestion.answerMinus)
        {
            Debug.Log("Correct");
            yesSFX.Play();
            playerMathScore += 10;
            totalCorrect++;
            if (totalCorrect % 5 == 0 && totalCorrect > 0)
            {
                AddBonusTime();
            }
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            noSFX.Play();
            timeRemaining--;
            totalWrong++;
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //button click times answer
    public void UserSelectTimes()
    {
        if (currentQuestion.answerTimes)
        {
            Debug.Log("Correct");
            yesSFX.Play();
            playerMathScore += 10;
            totalCorrect++;
            if (totalCorrect % 5 == 0 && totalCorrect > 0)
            {
                AddBonusTime();
            }
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            noSFX.Play();
            timeRemaining--;
            totalWrong++;
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //button click divide answer
    public void UserSelectDivide()
    {
        if (currentQuestion.answerDivide)
        {
            Debug.Log("Correct");
            yesSFX.Play();
            playerMathScore += 10;
            totalCorrect++;
            if (totalCorrect % 5 == 0 && totalCorrect > 0)
            {
                AddBonusTime();
            }
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            Debug.Log("Wrong");
            noSFX.Play();
            timeRemaining--;
            totalWrong++;
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //add bonus time for 5 correct answer
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

    //transition to the next question (0s)
    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);

        yield return new WaitForSeconds(timeBetweenQuestions);

        Start();
    }

    //math statistic endscreen
    public void FinalMathScore()
    {
        if (playerMathScore >= 180)
        {
            scoreResult = "A+";
        }
        else if (playerMathScore >= 150)
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
        scoreResultText.text = scoreResult;
        int totalAnsweredQuestion = totalCorrect + totalWrong;
        float correctWrongRatio = ((float)totalCorrect / (float)totalAnsweredQuestion) * 100f;
        sliderCorrectWrongRatio.value = correctWrongRatio;
        correctText.text = totalCorrect.ToString();
        wrongText.text = totalWrong.ToString();
        totalText.text = playerMathScore.ToString();
    }

    //show record on endscreen
    void ShowRecord()
    {
        //save current session score
        int recordCount = PlayerPrefs.GetInt("mathProblemRecordCount", 0);
        PlayerPrefs.SetInt("mathProblemRecord_" + recordCount.ToString(), playerMathScore);
        recordCount++;
        PlayerPrefs.SetInt("mathProblemRecordCount", recordCount);

        //load all score
        List<int> recordList = new List<int>();
        for (int i = 0; i < recordCount; i++)
        {
            recordList.Add(PlayerPrefs.GetInt("mathProblemRecord_" + i.ToString()));
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
