using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CategoryButton : MonoBehaviour
{
    public Image backgroundImage;
    public Color backgroundColor;
    public Image innerDotImage;
    public TMP_Text titleText;
    public TMP_Text acronymText;

    public int buttonId;

    [HideInInspector] public CategoryList categoryList;

    private bool isOn;

    private void Start() 
    {
        backgroundImage.color = backgroundColor;
    }

    public void Select()
    {
        if (!isOn) {
            innerDotImage.gameObject.SetActive(true);
            categoryList.AddSelectedToList(buttonId);
            isOn = true;
        } else {
            innerDotImage.gameObject.SetActive(false);
            categoryList.RemoveSelectedFromList(buttonId);
            isOn = false;
        }
    }
}
