using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomQButton : MonoBehaviour
{
    public TMP_Text abbrevQuestionText;
    public TMP_Text categoryText;
    public TMP_Text qIndexText;
    public Image itemBackground;

    [HideInInspector] public int questionId;
    [HideInInspector] public CustomQList customQList;

    public void EditButton()
    {
        customQList.EditQuestion(questionId);
    }

    public void DeleteButton()
    {
        customQList.ConfirmDeleteQuestion(questionId);
    }
}
