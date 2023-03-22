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
    [SerializeField] TMP_Text timeScoreText;
    [SerializeField] TMP_Text encouragementText;
    private string[] encouragements = new string[] {
        "Progress over perfection!\nYou're doing great.",
        "Keep practicing!\nYou're killin' it!",
        "Nice job!\nYou're almost ready for the big test.",
    };

    [Header("Questions:")]
    public List<SaveQuestionObject> pcmQuestions;
    public List<SaveQuestionObject> pjmQuestions;
    public List<SaveQuestionObject> paQuestions;
    public List<SaveQuestionObject> ppdQuestions;
    public List<SaveQuestionObject> pddQuestions;
    public List<SaveQuestionObject> ceQuestions;    
    private List<SaveQuestionObject> tempQuestionList = new List<SaveQuestionObject>();
    // totalQuestionList is used to assign index for each question, used by status panel:
    private List<SaveQuestionObject> totalQuestionList = new List<SaveQuestionObject>();

    public TMP_Text timerText;
    private float timer;
    private string currentTime;
    private bool readyForChallenge = false;

    private float totalPoints;
    private StatusController statusController;
    private CameraManager cameraManager;

    private void Awake() 
    {
        InitiateTotalQuestions();
        statusController = GetComponent<StatusController>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

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
        currentTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = currentTime;
    }

    public void StopGame()
    {
        if (currentCard) Destroy(currentCard);
        inGameMenu.SetActive(false);
        if (isChallengeMode) {
            readyForChallenge = false;
            string numScore = $"{totalPoints}/{(int)selectedTimeInterval / 2}";
            numScoreText.text = numScore;
            float calcPercScore = Mathf.Round((totalPoints / (selectedTimeInterval / 2)) * 100);
            percentScoreText.text = $"{calcPercScore}%";
            timeScoreText.text = currentTime;
            encouragementText.text = encouragements[Random.Range(0, encouragements.Length)];
            statusController.SaveChallengeStats((int)selectedTimeInterval, calcPercScore, currentTime);
            challengeCompletePanel.SetActive(true);
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
        tempQuestionList.Clear();
        mainMenu.SetActive(true);
        totalPoints = 0;
    }

    private void AddQuestionsToList()
    {
        foreach (int index in selectedCategories) {
            if (index == 0) {
                foreach (SaveQuestionObject q in pcmQuestions) {
                    tempQuestionList.Add(q);
                }
            } else if (index == 1) {
                foreach (SaveQuestionObject q in pjmQuestions) {
                    tempQuestionList.Add(q);
                }
            } else if (index == 2) {
                foreach (SaveQuestionObject q in paQuestions) {
                    tempQuestionList.Add(q);
                }
            } else if (index == 3) {
                foreach (SaveQuestionObject q in ppdQuestions) {
                    tempQuestionList.Add(q);
                }
            } else if (index == 4) {
                foreach (SaveQuestionObject q in pddQuestions) {
                    tempQuestionList.Add(q);
                }
            } else if (index == 5) {
                foreach (SaveQuestionObject q in ceQuestions) {
                    tempQuestionList.Add(q);
                }
            }
        }
        if (isChallengeMode) {
            RandomizeAndTrimList();
        }
    }

    private void RandomizeAndTrimList()
    {
        for (int i = 0; i < tempQuestionList.Count; i++) {
            SaveQuestionObject temp = tempQuestionList[i];
            int randomIndex = Random.Range(i, tempQuestionList.Count);
            tempQuestionList[i] = tempQuestionList[randomIndex];
            tempQuestionList[randomIndex] = temp;
        }
        for (int i = ((int)selectedTimeInterval / 2) - 1; i < tempQuestionList.Count; i++) {
            tempQuestionList.Remove(tempQuestionList[i]);
        }
    }

    public void CreateQuestion()
    {
        if (tempQuestionList.Count > 0) {
            SaveQuestionObject q = tempQuestionList[Random.Range(0, tempQuestionList.Count)];
            MultiCardController questionCard = Instantiate(multChoicePrefab, canvas);
            questionCard.qIndex = q.questionIndex;
            questionCard.questionType = q.type;
            questionCard.question = q.questionText;
            questionCard.description = q.questionDetailsText;
            questionCard.instructions = q.instruction;
            questionCard.hasImagesForAnswers = q.hasImageAnswers;
            if (q.hasImageAnswers) {
                Sprite[] images = new Sprite[q.imageAnswerFilePaths.Count];
                for (int i = 0; i < q.imageAnswerFilePaths.Count; i++) {
                    images[i] = cameraManager.LoadImageFromPath(q.imageAnswerFilePaths[i]);
                }
                questionCard.imgAnswers = images;
                questionCard.answerAmount = q.imageAnswerFilePaths.Count;
            } else {
                questionCard.answers = q.textAnswers;
                questionCard.answerAmount = q.textAnswers.Length;
            }
            questionCard.correctAnswersList = q.correctAnswerIds;
            if (!string.IsNullOrEmpty(q.refImageFilePath)) {
                questionCard.imageRef = cameraManager.LoadImageFromPath(q.refImageFilePath);
            }
            if (q.correctAnswerIds.Count > 1) {
                questionCard.hasMultAnswers = true;
            }
            if (q.type == "isTapOnImage") {
                // questionCard.imageToTapPrefab = q.imageToTap;
                questionCard.tapImagePath = q.tapImageFilePath;
                questionCard.correctTapLocation = q.correctTapAreaPosition;
            }
            tempQuestionList.Remove(q);
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

    public void InitiateTotalQuestions()
    {
        totalQuestionList.Clear();
        tempQuestionList.Clear();
        List<string> allQuestions = new List<string>();
        allQuestions = SaveSystem.LoadAllQuestions();
        if (allQuestions != null) {
            foreach (string qString in allQuestions) {
                SaveQuestionObject saveObject = JsonUtility.FromJson<SaveQuestionObject>(qString);
                totalQuestionList.Add(saveObject);
                if (saveObject.categoryIndex == 0) {
                    pcmQuestions.Add(saveObject);
                } else if (saveObject.categoryIndex == 1) {
                    pjmQuestions.Add(saveObject);
                } else if (saveObject.categoryIndex == 2) {
                    paQuestions.Add(saveObject);
                } else if (saveObject.categoryIndex == 3) {
                    ppdQuestions.Add(saveObject);
                } else if (saveObject.categoryIndex == 4) {
                    pddQuestions.Add(saveObject);
                } else if (saveObject.categoryIndex == 5) {
                    ceQuestions.Add(saveObject);
                }
            }
            Debug.Log(allQuestions.Count);
            for (int i = 0; i < totalQuestionList.Count; i++) {
                SaveQuestionObject q = totalQuestionList[i];
                q.questionIndex = i;
            }
        }
    }

    public int TotalQuestions()
    {
        return totalQuestionList.Count;
    }


}
