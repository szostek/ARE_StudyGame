using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AnswerFieldList : MonoBehaviour
{
    [SerializeField] AnswerField answerFieldPrefab;
    [SerializeField] AnswerImage answerImagePrefab;
    [SerializeField] GameObject addFieldButtonPrefab;
    private GameObject currentAddFieldButton;
    [SerializeField] Switch answerTypeSwitch;
    public RectTransform answerFieldList;

    // Card is used for question preview. Show this before saving question:
    [SerializeField] MultiCardController previewCardPrefab;
    [SerializeField] RectTransform canvas;

    [HideInInspector] public List<AnswerField> textFieldsList = new List<AnswerField>();
    [HideInInspector] public List<AnswerImage> imageFieldsList = new List<AnswerImage>();
    private string[] textAnswers;
    private string[] imageAnswerfilePaths;
    private List<int> correctAnswerIds;

    private bool toggled = false;
    private bool hasImageAnswers = false;

    private QBuilderManager builderManager;
    private CameraManager cameraManager;
    private GameManager gameManager;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();
        cameraManager = GetComponent<CameraManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() 
    {
        if (!answerTypeSwitch.isOn && !toggled) {
            hasImageAnswers = false;
            imageFieldsList.Clear();
            cameraManager.RemoveAllTempImages();
            AddTextFieldAnswers();
            toggled = true;
        }
        if (answerTypeSwitch.isOn && toggled) {
            hasImageAnswers = true;
            textFieldsList.Clear();
            AddImageAnswers();
            toggled = false;
        }

    }

    private void AddTextFieldAnswers()
    {        
        RemoveListItems();
        for (int i = 0; i < 2; i++) {
            AddTextFieldItem();
        }
        ApplyFieldIds();
        AddNewAnswerButton();
    }
    private void AddImageAnswers()
    {
        RemoveListItems();
        for (int i = 0; i < 2; i++) {
            AddImageFieldItem();
        }
        ApplyFieldIds();
        AddNewAnswerButton();
    }

    private void AddTextFieldItem()
    {
        AnswerField field = Instantiate(answerFieldPrefab, answerFieldList);
        field.answerFieldList = this;
        textFieldsList.Add(field);
        
    }
    private void AddImageFieldItem()
    {
        AnswerImage field = Instantiate(answerImagePrefab, answerFieldList);
        field.answerFieldList = this;
        field.cameraManager = cameraManager;
        field.gameManager = gameManager;
        imageFieldsList.Add(field);  
    }

    private void AddNewAnswerButton() //Adds the Add Answer button to end of list
    {
        GameObject addFieldButton = Instantiate(addFieldButtonPrefab, answerFieldList);
        Button button = addFieldButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => {
            Destroy(addFieldButton.gameObject);
            if (hasImageAnswers) {
                AddImageFieldItem();
            } else {
                AddTextFieldItem();
            }
            AddNewAnswerButton();
        });
        currentAddFieldButton = addFieldButton;
        ApplyFieldIds();
    }

    public void ApplyFieldIds() //Re-sorts the ids of all current buttons
    {
        if (hasImageAnswers) {
            for (int i = 0; i < imageFieldsList.Count; i++) {
                imageFieldsList[i].fieldId = i;
                Debug.Log(i);
            }
        } else {
            for (int i = 0; i < textFieldsList.Count; i++) {
                textFieldsList[i].fieldId = i;
            }
        }
    }

    public void DeleteField(int fieldId)
    {
        Debug.Log("Deleting field id: " + fieldId);
        if (hasImageAnswers) {
            Destroy(imageFieldsList[fieldId].gameObject);
            imageFieldsList.RemoveAt(fieldId);
        } else {
            Destroy(textFieldsList[fieldId].gameObject);
            textFieldsList.RemoveAt(fieldId);
        }
        ApplyFieldIds();
    }

    private void RemoveListItems()
    {
        foreach (RectTransform item in answerFieldList) {
            Destroy(item.gameObject);
        }
    }

    // PUT THESE TWO FUNCTIONS IN THE QBuilderManager SCRIPT:
    public void ShowPreviewCard()
    {
        MultiCardController previewCard = Instantiate(previewCardPrefab, canvas);
        previewCard.isBuilderMode = true;
        previewCard.questionType = builderManager.type;
        previewCard.hasImagesForAnswers = hasImageAnswers;
        previewCard.qIndex = gameManager.TotalQuestions() + 1;
        previewCard.question = builderManager.questionText;
        previewCard.instructions = builderManager.instruction;
        if (hasImageAnswers) {
            correctAnswerIds = new List<int>();
            Sprite[] images = new Sprite[cameraManager.tempImagePaths.Count];
            for (int i = 0; i < cameraManager.tempImagePaths.Count; i++) {
                images[i] = cameraManager.LoadImageFromPath(cameraManager.tempImagePaths[i]);
            }
            for (int i = 0; i < imageFieldsList.Count; i++) {
                // correctAnswerIds.Add(imageFieldsList[i].correctAnswerToggle.isOn ? 1: 0);
                if (imageFieldsList[i].correctAnswerToggle.isOn) {
                    correctAnswerIds.Add(imageFieldsList[i].fieldId);
                }
            }
            builderManager.correctAnswerIds = correctAnswerIds;
            builderManager.hasImageAnswers = true;
            previewCard.imgAnswers = images;
            previewCard.answerAmount = images.Length;
            previewCard.correctAnswersList = correctAnswerIds;
            previewCard.hasMultAnswers = correctAnswerIds.Count > 1;
        } else {
            textAnswers = new string[textFieldsList.Count];
            correctAnswerIds = new List<int>();
            for (int i = 0; i < textFieldsList.Count; i++) {
                textAnswers[i] = textFieldsList[i].answerTextField.text;
                if (textFieldsList[i].correctAnswerToggle.isOn) {
                    correctAnswerIds.Add(textFieldsList[i].fieldId);
                }
            }
            builderManager.textAnswers = textAnswers;
            builderManager.correctAnswerIds = correctAnswerIds;
            previewCard.answers = textAnswers;
            previewCard.answerAmount = textAnswers.Length;
            previewCard.correctAnswersList = correctAnswerIds;
            previewCard.hasMultAnswers = correctAnswerIds.Count > 1;
        }
        if (!string.IsNullOrEmpty(builderManager.refImageFilePath)) {
            previewCard.imageRef = cameraManager.LoadImageFromPath(builderManager.refImageFilePath);
        }
        // Adjust instruction text if there's only 1 correct answer:
        if (correctAnswerIds.Count == 1) {
            builderManager.instruction = "Choose only one Answer";
            previewCard.instructions = builderManager.instruction;
        }
    }


}
