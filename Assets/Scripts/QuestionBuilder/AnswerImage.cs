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
    [HideInInspector] public GameManager gameManager;

    public void Delete()
    {
        answerFieldList.DeleteField(fieldId);
    }

    public void TakePhotoButton()
    {

    }
}
