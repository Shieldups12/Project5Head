using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private GameManager_TriviaGame gameManager_TriviaGame;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Image questionImage;
    [SerializeField] private UnityEngine.Video.VideoPlayer questionVideo;
    [SerializeField] private AudioSource questionAudio;
    [SerializeField] private List<Button> options;
    [SerializeField] private Color correctColor, wrongColor, normalColor;

    private TriviaQuestion triviaQuestion;
    private bool answered;
    private float audioLength;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < options.Count; i++)
        {
            Button localButton = options[i];
            localButton.onClick.AddListener(() => OnClick(localButton));
        }
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
                StartCoroutine(PlayAudio());
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

    IEnumerator PlayAudio()
    {
        if(triviaQuestion.questionType == QuestionType.AUDIO)
        {
            questionAudio.PlayOneShot(triviaQuestion.questionAudio);
            yield return new WaitForSeconds(audioLength + 0.5f);
            StartCoroutine(PlayAudio()); 
        }
        else
        {
            StopCoroutine(PlayAudio());
            yield return null;
        }
    }

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
                button.image.color = correctColor;
            }
            else
            {
                button.image.color = wrongColor;
            }
        }
    }
}
