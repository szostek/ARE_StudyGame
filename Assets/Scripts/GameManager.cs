using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public RectTransform canvas;
    public GameObject mainMenu;
    public GameObject levelCompletePanel;
    public MultiCardController multChoicePrefab;
    [HideInInspector] public List<int> selectedCategories;
    [HideInInspector] public bool isChallengeMode = false;
    [HideInInspector] public int selectedTimeInterval;

    [Header("Questions:")]
    public List<QuestionSO> pcmQuestions;
    public List<QuestionSO> pjmQuestions;
    public List<QuestionSO> paQuestions;
    public List<QuestionSO> ppdQuestions;
    public List<QuestionSO> pddQuestions;
    public List<QuestionSO> ceQuestions;    
    private List<QuestionSO> questionList = new List<QuestionSO>();

    private float countdown;

    private float totalPoints;

    void Start()
    {
        // CreateQuestion();
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        AddQuestionsToList();
        CreateQuestion();
    }

    public void Reset()
    {
        questionList.Clear();
        mainMenu.SetActive(true);
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
            if (q.correctAnswerIndicies.Count > 1) {
                questionCard.hasMultAnswers = true;
            }
            if (q.GetQuestionType() == "isTapOnImage") {
                questionCard.imageToTapPrefab = q.imageToTap;
            }
            questionList.Remove(q);
        } else {
            Debug.Log("No more questions available!");
            mainMenu.SetActive(true);
            levelCompletePanel.SetActive(true);
            // Show score if in challenge mode...
        }
    }

    public void AddPoints()
    {
        totalPoints += 1;
        Debug.Log("Total Points: " + totalPoints);
    }


}
