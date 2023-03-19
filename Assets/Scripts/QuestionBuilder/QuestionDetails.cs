using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionDetails : MonoBehaviour
{
    [SerializeField] TMP_InputField questionText;
    [SerializeField] TMP_InputField questionDetailsText;
    [SerializeField] Image previewImage;
    [SerializeField] Button nextButton;

    private QBuilderManager builderManager;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();    
    }

    private void Update() 
    {
        nextButton.interactable = questionText.text.Length > 0;
    }

    public void NextButton()
    {
        if (!string.IsNullOrEmpty(questionText.text)) {
            builderManager.questionText = questionText.text;
            builderManager.questionDetailsText = questionDetailsText.text;
            builderManager.refImageFilePath = "Need to save path to image here";
        }
    }
}
