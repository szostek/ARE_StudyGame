using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerImage : MonoBehaviour
{
    public Image previewImage;
    public Toggle correctAnswerToggle;
    public Button takePhotoButton;
    public Button uploadPhotoButton;
    public Button deleteButton;
    [HideInInspector] public int fieldId;
    [HideInInspector] public AnswerFieldList answerFieldList;
    [HideInInspector] public CameraManager cameraManager;

    public void Delete()
    {
        answerFieldList.DeleteField(fieldId);
    }

    public void TakePhotoButton()
    {
        cameraManager.TakePicture(512, previewImage, fieldId, (isValidPath) => {});

    }
    public void UploadPhotoButton()
    {
        cameraManager.UploadPictureFromGallery(512, previewImage, fieldId, (isValidPath) => {});
    }
}
