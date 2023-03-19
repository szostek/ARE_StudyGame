using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerField : MonoBehaviour
{
    public TMP_InputField answerTextField;
    public Toggle correctAnswerToggle;
    public Button deleteButton;
    [HideInInspector] public int fieldId;
    [HideInInspector] public AnswerFieldList answerFieldList;

    public void Delete()
    {
        answerFieldList.DeleteField(fieldId);
    }
}
