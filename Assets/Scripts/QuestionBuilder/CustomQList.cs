using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomQList : MonoBehaviour
{
    [Header("Custom Q Refs:")]
    [SerializeField] CustomQButton customQButtonPrefab;
    [SerializeField] RectTransform customQuestionList;

    [Header("Builder UI Refs:")]
    [SerializeField] GameObject questionDetailsPanel;
    private QuestionDetails questionDetails;

    [HideInInspector] public List<SaveQuestionObject> allQuestions = new List<SaveQuestionObject>();
    private QBuilderManager builderManager;
    private CategoryChooser categoryChooser;
    private AnswerFieldList answerFieldList;
    private AnswerFillInBlank answerFillInBlank;
    private AnswerTapImage answerTapImage;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();
        categoryChooser = GetComponent<CategoryChooser>();
        questionDetails = GetComponent<QuestionDetails>();
        answerFieldList = GetComponent<AnswerFieldList>();
        answerFillInBlank = GetComponent<AnswerFillInBlank>();
        answerTapImage = GetComponent<AnswerTapImage>();
    }

    // Called from gameManager when questions are loaded:
    public void PopulateCustomQs()
    {
        foreach(RectTransform item in customQuestionList) {
            Destroy(item.gameObject);
        }

        if (allQuestions.Count > 0) {
            foreach (SaveQuestionObject question in allQuestions) {
                if (question.isUserCreated) {
                    CustomQButton item = Instantiate(customQButtonPrefab, customQuestionList);
                    item.abbrevQuestionText.text = question.questionText.Length <= 32 ? question.questionText : question.questionText.Substring(0, 32) + "..";
                    item.categoryText.text = "Category: " + categoryChooser.acronyms[question.categoryIndex];
                    item.qIndexText.text = "Q# " + question.questionIndex;
                    item.itemBackground.color = categoryChooser.backgroundColors[question.categoryIndex];
                    item.customQList = this;
                }
            }
        } else {
            Debug.Log("There's no user-created questions yet...");
        }
    }

    public void EditQuestion(int qIndex)
    {
        builderManager.isEditMode = true;
        PopulateQuestionInfo(allQuestions[qIndex]);
        Debug.Log("Editing question: " + qIndex);
    }

    private void PopulateQuestionInfo(SaveQuestionObject q)
    {
        builderManager.questionIndex = q.questionIndex;
        builderManager.categoryIndex = q.categoryIndex;
        builderManager.type = q.type;
        builderManager.questionText = q.questionText;
        builderManager.instruction = q.instruction;
        builderManager.questionDetailsText = q.questionDetailsText;
        builderManager.refImageFilePath = q.refImageFilePath;
        builderManager.hasImageAnswers = q.hasImageAnswers;
        builderManager.textAnswers = q.textAnswers;
        builderManager.imageAnswerFilePaths = q.imageAnswerFilePaths;
        builderManager.correctAnswerIds = q.correctAnswerIds;
        builderManager.correctTapAreaPosition = q.correctTapAreaPosition;
        builderManager.tapImageFilePath = q.tapImageFilePath;

        questionDetails.PopulateDetailsEditMode();
        questionDetailsPanel.SetActive(true);

        // Need to check question type here, and populate correct answer panels:
        if (q.type == "isMultiChoice") {
            answerFieldList.PopulateAnswersEditMode();
        } else if (q.type == "isFillInBlank") {
            answerFillInBlank.PopulateAnswerEditMode();
        } else if (q.type == "isTapOnImage") {
            answerTapImage.PopulateTapImageEditMode();
        }
    }

    public void DeleteQuestion(int qIndex)
    {
        Debug.Log("Deleting question: " + qIndex);
    }
}
