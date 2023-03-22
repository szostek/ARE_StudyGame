using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerTapImage : MonoBehaviour
{
    [SerializeField] TapToMark tapImage;

    private CameraManager cameraManager;
    private QBuilderManager builderManager;
    private GameManager gameManager;

    // Card is used for question preview. Show this before saving question:
    [SerializeField] MultiCardController previewCardPrefab;
    [SerializeField] RectTransform canvas;

    private void Awake() 
    {
        cameraManager = GetComponent<CameraManager>();
        builderManager = GetComponent<QBuilderManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start() 
    {
        tapImage.isBuilder = true;    
    }

    public void UploadTapImage()
    {
        Image imagePreview = tapImage.GetComponent<Image>();
        cameraManager.TakePicture(512, imagePreview, 777);
        tapImage.hasUploadedTapImage = true;
        SaveTapImage();
    }

    public void SaveTapImage()
    {
        List<string> refimagePaths = cameraManager.SavePictures();
        if (refimagePaths.Count > 0) {
            builderManager.tapImageFilePath = refimagePaths[0];
        }
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
        previewCard.correctTapLocation = builderManager.correctTapAreaPosition;
        tapImage.isBuilder = false;
        previewCard.imageToTapPrefab = tapImage;
        if (!string.IsNullOrEmpty(builderManager.refImageFilePath)) {
            previewCard.imageRef = cameraManager.LoadImageFromPath(builderManager.refImageFilePath);
        }

    }
}
