﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizUI : MonoBehaviour
{
    //screen
    public GameObject triviaEndScreen;
    public GameObject triviaPauseScreen;
    public GameObject triviaCountdownScreen;

    //audio
    public AudioSource yesSFX;
    public AudioSource noSFX;

    //countdown timer
    float countdownCurrentTime = 0f;
    float countdownStartTime = 3f;
    [SerializeField] private TMP_Text countdownText;

    //trivia game
    [SerializeField] private GameManager_TriviaGame gameManager_TriviaGame;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Image questionImage;
    [SerializeField] private UnityEngine.Video.VideoPlayer questionVideo;
    [SerializeField] private AudioSource questionAudio;
    [SerializeField] private List<Button> options;
    [SerializeField] private Color correctColor, wrongColor, normalColor;
    [SerializeField] private TMP_Text scoreText;
    public int playerTriviaScore = 0;

    private TriviaQuestion triviaQuestion;
    private bool answered;
    private float audioLength;

    //timer variable
    private float timeRemaining = 40f;
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

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("IsTriviaGameStart", 0) == 0)
        {
            CountdownTrigger();
        }

        for (int i = 0; i < options.Count; i++)
        {
            Button localButton = options[i];
            localButton.onClick.AddListener(() => OnClick(localButton));
        }
        PlayerPrefs.SetInt("Trivia_Score", 0);
    }

    void Update()
    {
        DoCountdown();

        if (triviaPauseScreen.activeSelf || triviaCountdownScreen.activeSelf == false)
        {
            sliderRight.value = timeRemaining;
            sliderLeft.value = timeRemaining;

            if (timeRemaining <= 6.66)
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
            if(triviaEndScreen.activeSelf == false)
            {
                FinalTriviaScore();
                ShowRecord();
                triviaEndScreen.SetActive(true);
            }
        }

        scoreText.text = PlayerPrefs.GetInt("Trivia_Score").ToString();
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
        triviaCountdownScreen.SetActive(true);
        yield return new WaitForSeconds(3);
        startSliderTime = Time.time;
        PlayerPrefs.SetInt("IsTriviaGameStart", 1);
        triviaCountdownScreen.SetActive(false);
        questionText.gameObject.SetActive(true);
    }

    public void SetTriviaQuestion(TriviaQuestion triviaQuestion)
    {
        this.triviaQuestion = triviaQuestion;

        switch (triviaQuestion.questionType)
        {
            case QuestionType.TEXT:
                questionImage.transform.parent.gameObject.SetActive(false);
                break;

            case QuestionType.IMAGE:
                ImageHolder();
                questionImage.transform.gameObject.SetActive(true);
                questionImage.sprite = triviaQuestion.questionImage;
                break;

            case QuestionType.VIDEO:
                questionVideo.transform.gameObject.SetActive(true);
                questionVideo.clip = triviaQuestion.questionVideo;
                questionVideo.Play();
                ImageHolder();
                break;

            case QuestionType.AUDIO:
                ImageHolder();
                questionAudio.transform.gameObject.SetActive(true);
                audioLength = triviaQuestion.questionAudio.length;
                //StartCoroutine(PlayAudio());
                break;
        }
        questionText.text = triviaQuestion.questionInfo;

        List<string> answerList = ShuffleList.ShuffleListItems<string>(triviaQuestion.options);

        for (int i = 0; i < options.Count; i++)
        {
            options[i].GetComponentInChildren<TMP_Text>().text = answerList[i];
            options[i].name = answerList[i];
            options[i].image.color = normalColor;
        }
        answered = false;
    }

    //IEnumerator PlayAudio()
    //{
    //    if(triviaQuestion.questionType == QuestionType.AUDIO)
    //    {
    //        questionAudio.PlayOneShot(triviaQuestion.questionAudio);
    //        yield return new WaitForSeconds(audioLength + 0.5f);
    //        StartCoroutine(PlayAudio()); 
    //    }
    //    else
    //    {
    //        StopCoroutine(PlayAudio());
    //        yield return null;
    //    }
    //}

    void ImageHolder()
    {
        questionImage.transform.parent.gameObject.SetActive(true);
        questionImage.transform.gameObject.SetActive(false);
        questionVideo.transform.gameObject.SetActive(false);
        questionAudio.transform.gameObject.SetActive(false);
    }

    private void OnClick(Button button)
    {
        if (!answered)
        {
            answered = true;
            bool i = gameManager_TriviaGame.Answer(button.name);
            if (i)
            {
                yesSFX.Play();
                //button.image.color = correctColor;
                Debug.Log("Correct");
                playerTriviaScore += 10;
                totalCorrect++;
                if (totalCorrect % 5 == 0 && totalCorrect > 0)
                {
                    AddBonusTime();
                }
                PlayerPrefs.SetInt("Trivia_Score", playerTriviaScore);
            }
            else
            {
                noSFX.Play();
                //button.image.color = wrongColor;
                Debug.Log("Wrong");
                totalWrong++;
                timeRemaining--;
                PlayerPrefs.SetInt("Trivia_Score", playerTriviaScore);
            }
        }
    }

    void AddBonusTime()
    {
        if (timeRemaining >= 40f)
        {
            timeRemaining = 40f;
        }
        else
        {
            timeRemaining++;
        }
        sliderRight.value = timeRemaining;
        sliderLeft.value = timeRemaining;
    }

    public void FinalTriviaScore()
    {
        Debug.Log("Hello");
        if (playerTriviaScore >= 150)
        {
            scoreResult = "A+";
        }
        else if (playerTriviaScore >= 120)
        {
            scoreResult = "A";
        }
        else if (playerTriviaScore >= 90)
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
        totalText.text = playerTriviaScore.ToString();
    }

    void ShowRecord()
    {
        //Save Current Session Score
        int recordCount = PlayerPrefs.GetInt("triviaProblemRecordCount", 0);
        PlayerPrefs.SetInt("triviaProblemRecord_" + recordCount.ToString(), playerTriviaScore);
        recordCount++;
        PlayerPrefs.SetInt("triviaProblemRecordCount", recordCount);
        //Load All Score
        List<int> recordList = new List<int>();
        for (int i = 0; i < recordCount; i++)
        {
            recordList.Add(PlayerPrefs.GetInt("triviaProblemRecord_" + i.ToString()));
        }
        //Display Top 4 Score
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
