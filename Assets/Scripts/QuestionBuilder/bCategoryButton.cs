using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bCategoryButton : MonoBehaviour
{
    public Image backgroundImage;
    public Color backgroundColor;
    public TMP_Text titleText;
    public TMP_Text acronymText;

    public int buttonId;

    [HideInInspector] public CategoryChooser categoryChooser;

    private void Start() 
    {
        backgroundImage.color = backgroundColor;
    }

    public void Select()
    {
        categoryChooser.SelectButtonId(buttonId);
    }
}
