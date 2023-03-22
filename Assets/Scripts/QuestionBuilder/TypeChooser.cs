using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeChooser : MonoBehaviour
{
    [SerializeField] TypeButton typeButtonPrefab;
    [SerializeField] RectTransform typeList;
    [SerializeField] Sprite[] icons;
    [SerializeField] TMP_Dropdown instructionDropdown;

    private QBuilderManager builderManager;

    private string[] questionTypes = new string[]{
        "Multiple Choice",
        "True / False",
        "Fill in the blank",
        "Choose location on an image",
    };
    private string[] correctedTypes = new string[] {
        "isMultiChoice",
        "isMultiChoice",
        "isFillInBlank",
        "isTapOnImage",
    };
    private string[] instructions = new string[] {
        "Choose all that apply",
        "Choose True or False",
        "Fill in the blank",
        "Tap a location on the image",
    };

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();
    }

    private void Start() 
    {
        builderManager.type = correctedTypes[0];
        for (int i = 0; i < questionTypes.Length; i++) {
            TypeButton button = Instantiate(typeButtonPrefab, typeList);
            button.buttonId = i;
            button.typeText.text = questionTypes[i];
            button.icon.sprite = icons[i];
            button.typeChooser = this;
        }       
    }

    public void ChooseButton(int id)
    {
        builderManager.type = correctedTypes[id];
        // Instruction is temp set here, needs to be adjusted in QBuilderManager if there's only one 1 in the answer index list:
        builderManager.instruction = instructions[id];
        
    }


}
