using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerFillInBlank : MonoBehaviour
{
    [SerializeField] TMP_InputField fillInBlankField;

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
        MultiCardController previewCard = Instantiate(previewCardPrefab, canvas);
        previewCard.isBuilderMode = true;
        previewCard.questionType = builderManager.type;
        previewCard.question = builderManager.questionText;
        previewCard.instructions = builderManager.instruction;
        previewCard.qIndex = gameManager.TotalQuestions() + 1;
        previewCard.description = builderManager.questionDetailsText;
        if (!string.IsNullOrEmpty(fillInBlankField.text)) {
            builderManager.textAnswers = new string[1];
            previewCard.answers = new string[1];
            builderManager.textAnswers[0] = fillInBlankField.text.ToLower();
            previewCard.answers[0] = fillInBlankField.text.ToLower();
        }
        if (!string.IsNullOrEmpty(builderManager.refImageFilePath)) {
            previewCard.imageRef = cameraManager.LoadImageFromPath(builderManager.refImageFilePath);
        }
    }

}
