using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QBuilderManager : MonoBehaviour
{
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
    [SerializeField] Image tapImagePreview;

    private CameraManager cameraManager;
    private GameManager gameManager;

    private void Awake() 
    {
        cameraManager = GetComponent<CameraManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SaveQuestion()
    {
        if (hasImageAnswers) {
            List<string> refimagePaths = cameraManager.SavePictures();
            if (refimagePaths.Count > 0) {
                imageAnswerFilePaths = refimagePaths;
            }
        }
        questionIndex = gameManager.TotalQuestions() + 1;
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
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.SaveQuestion(json, questionIndex);

        refImagePreview.sprite = null;
        gameManager.InitiateTotalQuestions();
        ResetAllInternalVars();
    }

    public void HideAllBuilderMenus()
    {
        refImagePreview.sprite = null;
        tapImagePreview.sprite = null;
        foreach (GameObject menu in builderMenus) {
            menu.SetActive(false);
        }
        cameraManager.RemoveAllTempImages();
        cameraManager.RemoveTempTapImageIfValid();
    }

    //Ideally if the user goes home without finishing question, the internal vars get cleared, and all text fields, etc.

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
}