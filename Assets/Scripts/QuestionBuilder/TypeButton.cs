using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeButton : MonoBehaviour
{
    public TMP_Text typeText;
    public Image icon;
    [HideInInspector] public TypeChooser typeChooser;

    public int buttonId;

    public void Select()
    {
        typeChooser.ChooseButton(buttonId);
    }

}
