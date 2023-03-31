using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerTapImage : MonoBehaviour
{
    [SerializeField] TapToMark tapImage;
    [SerializeField] TMP_Text alertText;

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
        bool isValidPicture = cameraManager.TakePicture(512, imagePreview, 777);
        if (isValidPicture) {
            tapImage.hasUploadedTapImage = true;
            alertText.color = Color.black;
            alertText.text = "Great! Now tap the image to choose the target location.";
        } else {
            tapImage.hasUploadedTapImage = true;
            alertText.color = Color.red;
            alertText.text = "No image uploaded, try again.";
            StartCoroutine(HideAlertText());
        }
    }

    public void UploadTapImageFromGallery()
    {
        Image imagePreview = tapImage.GetComponent<Image>();
        bool isValidPicture = cameraManager.UploadPictureFromGallery(512, imagePreview, 777);
        if (isValidPicture) {
            tapImage.hasUploadedTapImage = true;
            alertText.color = Color.black;
            alertText.text = "Great! Now tap the image to choose the target location.";
        } else {
            tapImage.hasUploadedTapImage = true;
            alertText.color = Color.red;
            alertText.text = "No image uploaded, try again.";
            StartCoroutine(HideAlertText());
        }
    }

    IEnumerator HideAlertText()
    {
        yield return new WaitForSeconds(2f);
        alertText.text = "";
    }

    private void SaveTapImage()
    {
        List<string> imagePaths = cameraManager.SavePictures();
        foreach (string path in imagePaths) {
            if (path.Contains("_777")) {
                builderManager.tapImageFilePath = path;
            }
        }
    }

    public void PopulateTapImageEditMode()
    {
        Image imagePreview = tapImage.GetComponent<Image>();
        imagePreview.sprite = cameraManager.LoadImageFromPath(builderManager.tapImageFilePath);
        tapImage.ShowCurrentAreaMarkerEditMode(builderManager.correctTapAreaPosition);
    }

    public void ShowPreviewCard()
    {
        SaveTapImage();
        if (builderManager.correctTapAreaPosition == Vector2.zero) {
            alertText.color = Color.red;
            alertText.text = "Tap the image above to choose the target location.";
            StartCoroutine(ClearAlertText());
            return;
        }
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

    IEnumerator ClearAlertText()
    {
        yield return new WaitForSeconds(1.5f);
        alertText.text = "";
        alertText.color = Color.black;
    }

}
