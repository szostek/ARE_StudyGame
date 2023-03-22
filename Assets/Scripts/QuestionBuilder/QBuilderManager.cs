using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] GameObject[] builderMenus;

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
        Debug.Log("Category index: " + categoryIndex);
        Debug.Log("Type: " + type);
        Debug.Log("instruction: " + instruction);
        Debug.Log("question text: " + questionText);
        Debug.Log("Details text: " + questionDetailsText);
        Debug.Log("Ref image file path" + refImageFilePath);
        string answerTexts = "";
        foreach(string i in textAnswers) {
            answerTexts += i + ", ";
        }
        Debug.Log("answers: " + answerTexts);
        string correctIds = "";
        foreach (int id in correctAnswerIds) {
            correctIds += id.ToString() + ", ";
        }
        Debug.Log("correct ids: " + correctIds);

        string imagePaths = "";
        foreach (string path in imageAnswerFilePaths) {
            imagePaths += path + ", ";
        }
        Debug.Log(imagePaths);
    }

    public void HideAllBuilderMenus()
    {
        foreach (GameObject menu in builderMenus) {
            menu.SetActive(false);
        }
    }


}
