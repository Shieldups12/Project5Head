using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TriviaGame : MonoBehaviour
{
    [SerializeField] private QuizUI quizUI;
    [SerializeField] private QuizData quizData;
    private List<TriviaQuestion> questions;
    private TriviaQuestion selectedQuestion;

    void Start()
    {
        questions = quizData.triviaQuestions;
        SelectTriviaQuestion();
    }

    void SelectTriviaQuestion()
    {
        int i = Random.Range(0, questions.Count);
        selectedQuestion = questions[i];
        quizUI.SetTriviaQuestion(selectedQuestion);
    }

    public bool Answer(string answered)
    {
        bool correctAnswer = false;

        if(answered == selectedQuestion.correctAns)
        {
            correctAnswer = true;
        }
        else
        {

        }

        Invoke("SelectTriviaQuestion", 0.25f);

        return correctAnswer;
    }
}

[System.Serializable]
public class TriviaQuestion
{
    public string questionInfo;
    public QuestionType questionType;
    public Sprite questionImage;
    public AudioClip questionAudio;
    public UnityEngine.Video.VideoClip questionVideo;
    public List<string> options;
    public string correctAns;
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    VIDEO,
    AUDIO
}