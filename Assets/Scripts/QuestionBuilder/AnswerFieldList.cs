using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class AnswerFieldList : MonoBehaviour
{
    [SerializeField] AnswerField answerFieldPrefab;
    [SerializeField] AnswerImage answerImagePrefab;
    [SerializeField] GameObject addFieldButtonPrefab;
    private GameObject currentAddFieldButton;
    [SerializeField] Switch answerTypeSwitch;
    public RectTransform answerFieldList;
    [SerializeField] TMP_Text alertText;

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
        if (builderManager.isEditMode) {
            // answerTypeSwitch.gameObject.SetActive(false);
            return;
        } else {
            answerTypeSwitch.gameObject.SetActive(true);
        }
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

    public void PopulateAnswersEditMode()
    {
        if (builderManager.hasImageAnswers) {
            // answerTypeSwitch.SetSwitchOn(true, false);
            hasImageAnswers = true;
            imageFieldsList.Clear();
            textFieldsList.Clear();
            AddImageAnswers();
        } else {
            // answerTypeSwitch.isOn = false;
            // answerTypeSwitch.SetSwitchOn(false, false);
            hasImageAnswers = false;
            imageFieldsList.Clear();
            textFieldsList.Clear();
            cameraManager.RemoveAllTempImages();
            AddTextFieldAnswers();
        }
        answerTypeSwitch.gameObject.SetActive(false);
    }

    public void ResetAnswerListToDefault()
    {
        AddTextFieldAnswers();
    }

    private void AddTextFieldAnswers()
    {        
        RemoveListItems();
        int amount = builderManager.isEditMode ? builderManager.textAnswers.Length : 2;
        for (int i = 0; i < amount; i++) {
            AddTextFieldItem(builderManager.isEditMode ? builderManager.textAnswers[i] : "");
        }
        ApplyFieldIds();
        AddNewAnswerButton();
    }
    private void AddImageAnswers()
    {
        RemoveListItems();
        int amount = builderManager.isEditMode ? builderManager.imageAnswerFilePaths.Count : 2;
        for (int i = 0; i < amount; i++) {
            AddImageFieldItem(builderManager.isEditMode ? builderManager.imageAnswerFilePaths[i] : "");
        }
        // If edit mode, copy all custom images into TempImages folder
        if (builderManager.isEditMode) {
            cameraManager.CopyAnswerImagesToTemp(builderManager.questionIndex);
        }
        ApplyFieldIds();
        AddNewAnswerButton();
    }

    private void AddTextFieldItem(string answerText)
    {
        AnswerField field = Instantiate(answerFieldPrefab, answerFieldList);
        field.answerFieldList = this;
        field.answerTextField.text = answerText;
        textFieldsList.Add(field);
    }

    private void AddImageFieldItem(string imgPath)
    {
        AnswerImage field = Instantiate(answerImagePrefab, answerFieldList);
        field.answerFieldList = this;
        field.cameraManager = cameraManager;
        // If in edit move, load the current image for this field:
        if (!string.IsNullOrEmpty(imgPath)) {
            field.previewImage.sprite = cameraManager.LoadImageFromPath(imgPath);
        }
        imageFieldsList.Add(field);  
    }

    private void AddNewAnswerButton() //Adds the Add Answer button to end of list
    {
        GameObject addFieldButton = Instantiate(addFieldButtonPrefab, answerFieldList);
        Button button = addFieldButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => {
            Destroy(addFieldButton.gameObject);
            if (hasImageAnswers) {
                AddImageFieldItem("");
            } else {
                AddTextFieldItem("");
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
            }
            if (builderManager.isEditMode) {
                foreach (AnswerImage field in imageFieldsList) {
                    if (builderManager.correctAnswerIds.Contains(field.fieldId) && field.correctAnswerToggle.isOn == false) {
                        field.correctAnswerToggle.SetIsOnWithoutNotify(true);
                        // Debug.Log(field.fieldId + " Should be true");
                    }  
                }
            }
        } else {
            for (int i = 0; i < textFieldsList.Count; i++) {
                textFieldsList[i].fieldId = i;
            }
            if (builderManager.isEditMode) {
                foreach (AnswerField field in textFieldsList) {
                    if (builderManager.correctAnswerIds.Contains(field.fieldId) && field.correctAnswerToggle.isOn == false) {
                        field.correctAnswerToggle.SetIsOnWithoutNotify(true);
                        // Debug.Log(field.fieldId + " Should be true");
                    }
                }
            }
        }
    }

    public void DeleteField(int fieldId)
    {
        Debug.Log("Deleting field id: " + fieldId);
        if (hasImageAnswers) {
            Destroy(imageFieldsList[fieldId].gameObject);
            imageFieldsList.RemoveAt(fieldId);
            if (builderManager.isEditMode) {
                File.Delete(cameraManager.tempImagePaths[fieldId]);
                builderManager.imageAnswerFilePaths.RemoveAt(fieldId);
                cameraManager.tempImagePaths.RemoveAt(fieldId);
            }
        } else {
            Destroy(textFieldsList[fieldId].gameObject);
            textFieldsList.RemoveAt(fieldId);
            if (builderManager.isEditMode) {
                List<string> textAnswerList = new List<string>(builderManager.textAnswers);
                textAnswerList.RemoveAt(fieldId);
                builderManager.textAnswers = textAnswerList.ToArray();
            }
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
        if (!ValidateAnswers()) return;
        MultiCardController previewCard = Instantiate(previewCardPrefab, canvas);
        previewCard.isBuilderMode = true;
        previewCard.questionType = builderManager.type;
        previewCard.hasImagesForAnswers = hasImageAnswers;
        previewCard.qIndex = builderManager.isEditMode ? builderManager.questionIndex : gameManager.TotalQuestions() + 1;
        previewCard.question = builderManager.questionText;
        previewCard.instructions = builderManager.instruction;
        if (hasImageAnswers) {
            correctAnswerIds = new List<int>();
            Sprite[] images = new Sprite[cameraManager.tempImagePaths.Count];
            for (int i = 0; i < cameraManager.tempImagePaths.Count; i++) {
                images[i] = cameraManager.LoadImageFromPath(cameraManager.tempImagePaths[i]);
            }
            // int amount = builderManager.isEditMode ? builderManager.imageAnswerFilePaths.Count : cameraManager.tempImagePaths.Count;
            // for (int i = 0; i < amount; i++) {
            //     if (!builderManager.isEditMode) {
            //         images[i] = cameraManager.LoadImageFromPath(cameraManager.tempImagePaths[i]);
            //     } else {
            //         if (cameraManager.tempImagePaths.Count > 0) {
            //             foreach (string path in cameraManager.tempImagePaths) {
            //                 if (Path.GetFileName(path) == Path.GetFileName(builderManager.imageAnswerFilePaths[i])) {
            //                     images[i] = cameraManager.LoadImageFromPath(cameraManager.tempImagePaths[i]);
            //                 } else {
            //                     images[i] = cameraManager.LoadImageFromPath(builderManager.imageAnswerFilePaths[i]);
            //                 }
            //             }
            //         } else {
            //             images[i] = cameraManager.LoadImageFromPath(builderManager.imageAnswerFilePaths[i]);
            //         }
            //     }
            // }
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

    private bool ValidateAnswers()
    {
        if (hasImageAnswers) {
            if (imageFieldsList.Count == 0) {
                alertText.text = "Must have at least 1 answer!";
                StartCoroutine(ClearAlertText());
                return false;
            }
            List<int> toggleCheck = new List<int>();
            foreach (AnswerImage answer in imageFieldsList) {
                if (answer.previewImage.sprite == null) {
                    alertText.text = "Answers cannot have blank images!";
                    StartCoroutine(ClearAlertText());
                    return false;
                }
                if (answer.correctAnswerToggle.isOn) {
                    toggleCheck.Add(1);
                }
            }
            if (toggleCheck.Count == 0) {
                alertText.text = "Must specify at least 1 correct answer!";
                StartCoroutine(ClearAlertText());
                return false;
            }
        } else {
            if (textFieldsList.Count == 0) {
                alertText.text = "Must have at least 1 answer!";
                StartCoroutine(ClearAlertText());
                return false;
            }
            List<int> toggleCheck = new List<int>();
            foreach (AnswerField answer in textFieldsList) {
                if (string.IsNullOrEmpty(answer.answerTextField.text)) {
                    alertText.text = "Cannot include blank answers!";
                    StartCoroutine(ClearAlertText());
                    return false;
                }
                if (answer.correctAnswerToggle.isOn) {
                    toggleCheck.Add(1);
                }
            }
            if (toggleCheck.Count == 0) {
                alertText.text = "Must specify at least 1 correct answer!";
                StartCoroutine(ClearAlertText());
                return false;
            }
        }
        return true;
    }

    IEnumerator ClearAlertText()
    {
        yield return new WaitForSeconds(1.5f);
        alertText.text = "";
    }


}
