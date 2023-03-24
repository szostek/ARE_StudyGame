using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerFillInBlank : MonoBehaviour
{
    [SerializeField] TMP_InputField fillInBlankField;
    [SerializeField] TMP_Text alertText;

    // Card is used for question preview. Show this before saving question:
    [SerializeField] MultiCardController previewCardPrefab;
    [SerializeField] RectTransform canvas;

    private QBuilderManager builderManager;
    private CameraManager cameraManager;
    private GameManager gameManager;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();
        cameraManager = GetComponent<CameraManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ShowPreviewCard()
    {
        if (!ValidateAnswer()) return;
        MultiCardController previewCard = Instantiate(previewCardPrefab, canvas);
        previewCard.isBuilderMode = true;
        previewCard.questionType = builderManager.type;
        previewCard.question = builderManager.questionText;
        previewCard.instructions = builderManager.instruction;
        previewCard.qIndex = gameManager.TotalQuestions() + 1;
        previewCard.description = builderManager.questionDetailsText;
        previewCard.answers = new string[1];
        previewCard.answers[0] = fillInBlankField.text.ToLower();

        builderManager.textAnswers = new string[1];
        builderManager.textAnswers[0] = fillInBlankField.text.ToLower();
        
        if (!string.IsNullOrEmpty(builderManager.refImageFilePath)) {
            previewCard.imageRef = cameraManager.LoadImageFromPath(builderManager.refImageFilePath);
        }
    }

    private bool ValidateAnswer()
    {
        if (string.IsNullOrEmpty(fillInBlankField.text)) {
            alertText.text = "Please write a valid answer above!";
            StartCoroutine(ClearAlertText());
            return false;
        }
        return true;
    }
    IEnumerator ClearAlertText()
    {
        yield return new WaitForSeconds(1.5f);
        alertText.text = "";
    }

}
