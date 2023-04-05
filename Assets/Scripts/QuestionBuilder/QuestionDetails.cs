using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionDetails : MonoBehaviour
{
    [Header("Question Info:")]
    [SerializeField] TMP_InputField questionText;
    [SerializeField] TMP_InputField questionDetailsText;
    [SerializeField] Image previewImage;
    [SerializeField] Button nextButton;
    [SerializeField] TMP_Text imagePath;
    [SerializeField] Button removeImageButton;
    [SerializeField] TMP_Text alertText;

    [Header("Next Panels:")]
    public GameObject a_textImageAnswerPanel;
    public GameObject b_fillInBlankPanel;
    public GameObject c_tapImagePanel;

    private QBuilderManager builderManager;
    private CameraManager cameraManager;
    public UIManager uIManager;

    private bool hasRemovePreviewImage = true;

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

            if (!hasRemovePreviewImage) {
                List<string> refimagePaths = cameraManager.SavePictures();
                if (refimagePaths.Count > 0) {
                    builderManager.refImageFilePath = refimagePaths[0];
                }
            }
        }
        if (builderManager.type == "isMultiChoice") {
            uIManager.ShowNextScreen(a_textImageAnswerPanel);
            // a_textImageAnswerPanel.SetActive(true);
        } else if (builderManager.type == "isFillInBlank") {
            uIManager.ShowNextScreen(b_fillInBlankPanel);
            // b_fillInBlankPanel.SetActive(true);
        } else if (builderManager.type == "isTapOnImage") {
            uIManager.ShowNextScreen(c_tapImagePanel);
            // c_tapImagePanel.SetActive(true);
        }
    }

    public void TakePhotoButton()
    {
        cameraManager.TakePicture(512, previewImage, 666, (isValidPath) => {
            if (isValidPath)
            {
                removeImageButton.gameObject.SetActive(true);
                hasRemovePreviewImage = false;
            }
        });
    }

    public void UploadPhotoButton()
    {
        cameraManager.UploadPictureFromGallery(512, previewImage, 666, (isValidPath) => {
            if (isValidPath) 
            {
                removeImageButton.gameObject.SetActive(true);
                hasRemovePreviewImage = false;
            }
        });
    }

    public void RemoveImageButton()
    {
        // Delete image from temp files via cameraManager
        if (cameraManager.RemoveTempRefImage()) {            
            previewImage.sprite = null;
            removeImageButton.gameObject.SetActive(false);
            hasRemovePreviewImage = true;
        } else if (builderManager.isEditMode) {
            cameraManager.RemoveRefImage(builderManager.questionIndex);
            builderManager.refImageFilePath = "";
            previewImage.sprite = null;
            removeImageButton.gameObject.SetActive(false);
            hasRemovePreviewImage = true;
        }
    }

    public void PopulateDetailsEditMode()
    {
        questionText.text = builderManager.questionText;
        questionDetailsText.text = builderManager.questionDetailsText;
        if (!string.IsNullOrEmpty(builderManager.refImageFilePath)) {
            previewImage.sprite = cameraManager.LoadImageFromPath(builderManager.refImageFilePath);
            removeImageButton.gameObject.SetActive(true);
        }        
        hasRemovePreviewImage = true; // Setting this to true, wont resave the path in the builder manager, just keep the old one
    }

}