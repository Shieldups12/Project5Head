using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        }
    }
}
