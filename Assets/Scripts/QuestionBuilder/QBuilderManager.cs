using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class QBuilderManager : MonoBehaviour
{
    public bool isEditMode = false;
    [HideInInspector] public int questionIndex;
    [HideInInspector] public int categoryIndex;
    [HideInInspector] public string type;
    [HideInInspector] public string instruction;
    [HideInInspector] public string questionText;
    [HideInInspector] public string questionDetailsText;
    [HideInInspector] public string refImageFilePath;
    [HideInInspector] public bool hasImageAnswers;
    [HideInInspector] public string[] textAnswers;
    [HideInInspector] public List<string> imageAnswerFilePaths;
    [HideInInspector] public List<int> correctAnswerIds;
    [HideInInspector] public Vector2 correctTapAreaPosition;
    [HideInInspector] public string tapImageFilePath;

    [Header("UI References:")]
    [SerializeField] GameObject[] builderMenus;
    [SerializeField] Image refImagePreview;
    [SerializeField] TMP_InputField questionField;
    [SerializeField] TMP_InputField descriptionField;
    [SerializeField] RectTransform answerFieldListContent;
    private AnswerFieldList answerFieldList;
    [SerializeField] TMP_InputField fillInBlankField;
    [SerializeField] TapToMark tapImagePreview;
    [SerializeField] TMP_Text[] alertTexts;

    private CameraManager cameraManager;
    private GameManager gameManager;

    private void Awake() 
    {
        cameraManager = GetComponent<CameraManager>();
        answerFieldList = GetComponent<AnswerFieldList>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SaveQuestion()
    {
        if (hasImageAnswers) {
            if (isEditMode) {
                cameraManager.DeleteQuestionImages(questionIndex);
            }
            List<string> savedImagePaths = cameraManager.SavePictures();
            // if (savedImagePaths.Count > 0) {
            //     if (isEditMode) {
            //         for (int i = 0; i < imageAnswerFilePaths.Count; i++) {
            //             foreach (string path in savedImagePaths) {
            //                 if (Path.GetFileName(imageAnswerFilePaths[i]) == Path.GetFileName(path)) {
            //                     // User has updated the image, save the current path as the new path:
            //                     imageAnswerFilePaths[i] = path;
            //                 }
            //             }
            //         }
            //     } else {
            //         imageAnswerFilePaths = savedImagePaths;
            //     }
            // }
            imageAnswerFilePaths = savedImagePaths;
        }
        if (!isEditMode) {
            Debug.Log("SHOULDNT SEE THIS IF YOURE IN EDIT MODE...");
            questionIndex = gameManager.TotalQuestions() + 1;
        }
        cameraManager.tempTapImagePath = "";
        SaveQuestionObject saveObject = new SaveQuestionObject {
            questionIndex = questionIndex,
            categoryIndex = categoryIndex,
            type = type,
            instruction = instruction,
            questionText = questionText,
            questionDetailsText = questionDetailsText,
            refImageFilePath = refImageFilePath,
            hasImageAnswers = hasImageAnswers,
            textAnswers = textAnswers,
            imageAnswerFilePaths = imageAnswerFilePaths,
            correctAnswerIds = correctAnswerIds,
            correctTapAreaPosition = correctTapAreaPosition,
            tapImageFilePath = tapImageFilePath,
            isUserCreated = true,
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.SaveQuestion(json, questionIndex);

        refImagePreview.sprite = null;
        gameManager.InitiateTotalQuestions();
        // HideAllBuiderMenus function below is called from the gameManager after a couple seconds..
        
    }

    public void HideAllBuilderMenus()
    {
        refImagePreview.sprite = null;
        tapImagePreview.gameObject.GetComponent<Image>().sprite = null;
        if (tapImagePreview.gameObject.transform.GetChild(0) != null) {
            tapImagePreview.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        tapImagePreview.isBuilder = true;
        tapImagePreview.hasUploadedTapImage = false;        

        foreach (GameObject menu in builderMenus) {
            menu.SetActive(false);
        }
        questionField.text = "";
        descriptionField.text = "";
        fillInBlankField.text = "";
        foreach (RectTransform item in answerFieldListContent) {
            Destroy(item.gameObject);
        }
        answerFieldList.ResetAnswerListToDefault();
        foreach (TMP_Text alert in alertTexts) {
            alert.text = "";
        }
        cameraManager.RemoveAllTempImages();
        cameraManager.tempImagePaths.Clear();
        cameraManager.RemoveTempTapImageIfValid();
        ResetAllInternalVars();
        isEditMode = false;
        Debug.Log("Edit Mode: " + isEditMode);
    }

    public void ResetAllInternalVars()
    {
        questionIndex = 0;
        categoryIndex = 0;
        type = "";
        instruction = "";
        questionText = "";
        questionDetailsText = "";
        refImageFilePath = "";
        hasImageAnswers = false;
        textAnswers = null;
        imageAnswerFilePaths = null;
        correctAnswerIds = null;
        correctTapAreaPosition = new Vector2(0, 0);
        tapImageFilePath = "";
    }
    

}

[System.Serializable]
public class SaveQuestionObject 
{
    public int questionIndex;
    public int categoryIndex;
    public string type;
    public string instruction;
    public string questionText;
    public string questionDetailsText;
    public string refImageFilePath;
    public bool hasImageAnswers;
    public string[] textAnswers;
    public List<string> imageAnswerFilePaths;
    public List<int> correctAnswerIds;
    public Vector2 correctTapAreaPosition;
    public string tapImageFilePath;
    public bool isUserCreated;
}