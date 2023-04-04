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
    [SerializeField] TMP_Dropdown categoryFilterDropdown;
    private int filterIndex = 0;

    [Header("Builder UI Refs:")]
    [SerializeField] GameObject questionDetailsPanel;
    private QuestionDetails questionDetails;
    [SerializeField] GameObject deleteQuestionPanel;
    [SerializeField] Button confirmDeleteButton;
    [SerializeField] Button nevermindButton;
    [SerializeField] TMP_Text alertQuestionDeletedText;

    [HideInInspector] public List<SaveQuestionObject> allQuestions = new List<SaveQuestionObject>();
    public GameManager gameManager;
    private CameraManager cameraManager;
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
        cameraManager = GetComponent<CameraManager>();
    }

    private void Start() 
    {
        categoryFilterDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(categoryFilterDropdown); });
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
                    if (filterIndex == 0) {
                        CustomQButton item = Instantiate(customQButtonPrefab, customQuestionList);
                        item.questionId = question.questionIndex;
                        item.abbrevQuestionText.text = question.questionText.Length <= 32 ? question.questionText : question.questionText.Substring(0, 32) + "..";
                        item.categoryText.text = "Category: " + categoryChooser.acronyms[question.categoryIndex];
                        item.qIndexText.text = "Q# " + question.questionIndex;
                        item.itemBackground.color = categoryChooser.backgroundColors[question.categoryIndex];
                        item.customQList = this;
                    } else if (question.categoryIndex == filterIndex - 1) {
                        CustomQButton item = Instantiate(customQButtonPrefab, customQuestionList);
                        item.questionId = question.questionIndex;
                        item.abbrevQuestionText.text = question.questionText.Length <= 32 ? question.questionText : question.questionText.Substring(0, 32) + "..";
                        item.categoryText.text = "Category: " + categoryChooser.acronyms[filterIndex - 1];
                        item.qIndexText.text = "Q# " + question.questionIndex;
                        item.itemBackground.color = categoryChooser.backgroundColors[filterIndex - 1];
                        item.customQList = this;
                    }
                }
            }
        } else {
            Debug.Log("There's no user-created questions yet...");
        }
    }

    public void EditQuestion(int qIndex)
    {
        builderManager.isEditMode = true;
        foreach (SaveQuestionObject q in allQuestions) {
            if (q.questionIndex == qIndex) {
                PopulateQuestionInfo(q);
            }
        }
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

    public void ConfirmDeleteQuestion(int qIndex)
    {
        deleteQuestionPanel.SetActive(true);
        confirmDeleteButton.onClick.AddListener(() => {
            confirmDeleteButton.interactable = false;
            nevermindButton.interactable = false;
            DeleteQuestion(qIndex);
        });
    }

    public void DeleteQuestion(int qIndex)
    {
        SaveSystem.DeleteQuestion(qIndex);
        cameraManager.DeleteQuestionImages(qIndex);
        gameManager.InitiateTotalQuestions();
        Debug.Log("Deleting question: " + qIndex);
        alertQuestionDeletedText.gameObject.SetActive(true);
        StartCoroutine(HideDeletePanel());
    }

    IEnumerator HideDeletePanel()
    {
        yield return new WaitForSeconds(1.5f);
        confirmDeleteButton.interactable = true;
        nevermindButton.interactable = true;
        alertQuestionDeletedText.gameObject.SetActive(false);
        deleteQuestionPanel.SetActive(false);
    }

    private void DropdownValueChanged(TMP_Dropdown changedDropdown)
    {
        if (changedDropdown.value > 0) {
            changedDropdown.captionText.color = categoryChooser.backgroundColors[changedDropdown.value - 1];
        } else {
            changedDropdown.captionText.color = Color.black;
        }
        filterIndex = changedDropdown.value;
        PopulateCustomQs();
        Debug.Log(changedDropdown.value);
    }

}
