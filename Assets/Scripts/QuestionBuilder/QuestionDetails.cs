using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionDetails : MonoBehaviour
{
    [SerializeField] TMP_InputField questionText;
    [SerializeField] TMP_InputField questionDetailsText;
    [SerializeField] RawImage previewImage;
    [SerializeField] Button nextButton;

    private QBuilderManager builderManager;
    private CameraManager cameraManager;

    [SerializeField] TMP_Text imagePath;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();
        cameraManager = GetComponent<CameraManager>();
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

            List<string> refimagePaths = cameraManager.SavePictures();
            if (refimagePaths.Count > 0) {
                builderManager.refImageFilePath = refimagePaths[0];
            }
        }
    }

    public void TakePhotoButton()
    {
        cameraManager.TakePicture(512, previewImage, 666);
    }
}
