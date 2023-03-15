using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public RectTransform canvas;
    public GameObject mainMenu;
    public GameObject inGameMenu;
    public GameObject challengeCompletePanel;
    public GameObject practiceCompletePanel;
    public MultiCardController multChoicePrefab;
    private GameObject currentCard;
    [HideInInspector] public List<int> selectedCategories;
    [HideInInspector] public bool isChallengeMode = false;
    [HideInInspector] public float selectedTimeInterval;
    [SerializeField] TMP_Text numScoreText;
    [SerializeField] TMP_Text percentScoreText;
    [SerializeField] TMP_Text encouragementText;
    private string[] encouragements = new string[] {
        "Progress over perfection!\nYou're doing great.",
        "Keep practicing!\nYou're killin' it!",
        "Nice job!\nYou're almost ready for the big test.",
    };

    [Header("Questions:")]
    public List<QuestionSO> pcmQuestions;
    public List<QuestionSO> pjmQuestions;
    public List<QuestionSO> paQuestions;
    public List<QuestionSO> ppdQuestions;
    public List<QuestionSO> pddQuestions;
    public List<QuestionSO> ceQuestions;    
    private List<QuestionSO> questionList = new List<QuestionSO>();

    public TMP_Text timerText;
    private float timer;
    private bool readyForChallenge = false;

    private float totalPoints;

    public void StartGame()
    {
        mainMenu.SetActive(false);
        inGameMenu.SetActive(true);
        AddQuestionsToList();
        CreateQuestion();
        if (isChallengeMode) {
            timerText.gameObject.SetActive(true);
            timer = selectedTimeInterval * 60;
            readyForChallenge = true;
        } else {
            timerText.gameObject.SetActive(false);
        }
    }

    private void Update() 
    {
        if (isChallengeMode && readyForChallenge) {
            timer -= Time.deltaTime;
            HandleTimer();
            if (timer <= 1) {
                StopGame();
            }
        }
    }

    private void HandleTimer()
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopGame()
    {
        if (currentCard) Destroy(currentCard);
        inGameMenu.SetActive(false);
        if (isChallengeMode) {
            numScoreText.text = $"{totalPoints}/{(int)selectedTimeInterval / 2}";
            float calcPercScore = Mathf.Round((totalPoints / (selectedTimeInterval / 2)) * 100);
            percentScoreText.text = $"{calcPercScore}%";
            encouragementText.text = encouragements[Random.Range(0, encouragements.Length)];
            challengeCompletePanel.SetActive(true);
            readyForChallenge = false;
        } else {
            practiceCompletePanel.SetActive(true);
        }
        Reset();
    }

    public void HomeButton()
    {
        if (currentCard) Destroy(currentCard);
        inGameMenu.SetActive(false);
        Reset();
    }

    public void Reset()
    {
        questionList.Clear();
        mainMenu.SetActive(true);
        totalPoints = 0;
    }

    private void AddQuestionsToList()
    {
        foreach (int index in selectedCategories) {
            if (index == 0) {
                foreach (QuestionSO q in pcmQuestions) {
                    questionList.Add(q);
                }
            } else if (index == 1) {
                foreach (QuestionSO q in pjmQuestions) {
                    questionList.Add(q);
                }
            } else if (index == 2) {
                foreach (QuestionSO q in paQuestions) {
                    questionList.Add(q);
                }
            } else if (index == 3) {
                foreach (QuestionSO q in ppdQuestions) {
                    questionList.Add(q);
                }
            } else if (index == 4) {
                foreach (QuestionSO q in pddQuestions) {
                    questionList.Add(q);
                }
            } else if (index == 5) {
                foreach (QuestionSO q in ceQuestions) {
                    questionList.Add(q);
                }
            }
        }
        if (isChallengeMode) {
            RandomizeAndTrimList();
        }
    }

    private void RandomizeAndTrimList()
    {
        for (int i = 0; i < questionList.Count; i++) {
            QuestionSO temp = questionList[i];
            int randomIndex = Random.Range(i, questionList.Count);
            questionList[i] = questionList[randomIndex];
            questionList[randomIndex] = temp;
        }
        for (int i = (int)selectedTimeInterval / 2; i < questionList.Count; i++) {
            questionList.Remove(questionList[i]);
        }
    }

    public void CreateQuestion()
    {
        if (questionList.Count > 0) {
            QuestionSO q = questionList[Random.Range(0, questionList.Count)];
            MultiCardController questionCard = Instantiate(multChoicePrefab, canvas);
            questionCard.questionType = q.GetQuestionType();
            questionCard.question = q.question;
            questionCard.description = q.description;
            questionCard.instructions = q.GetInstruction();
            questionCard.answers = q.answers;
            questionCard.correctAnswersList = q.correctAnswerIndicies;
            questionCard.answerAmount = q.answers.Length;
            if (q.imageRef != null) {
                questionCard.imageRef = q.imageRef;
            }
            if (q.correctAnswerIndicies.Count > 1) {
                questionCard.hasMultAnswers = true;
            }
            if (q.GetQuestionType() == "isTapOnImage") {
                questionCard.imageToTapPrefab = q.imageToTap;
            }
            questionList.Remove(q);
            currentCard = questionCard.gameObject;
        } else {
            Debug.Log("No more questions available!");
            inGameMenu.SetActive(false);
            mainMenu.SetActive(true);
            StopGame();
            
        }
    }

    public void AddPoints()
    {
        totalPoints += 1;
        Debug.Log("Total Points: " + totalPoints);
    }


}
