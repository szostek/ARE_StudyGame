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

    // This get's called via the AnswerFieldList, when the Save Question button is hit:
    public void SaveQuestion()
    {
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
}
