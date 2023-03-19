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
        "Choose location on an image"
    };

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();
    }

    private void Start() 
    {
        for (int i = 0; i < questionTypes.Length; i++) {
            TypeButton button = Instantiate(typeButtonPrefab, typeList);
            button.buttonId = i;
            button.typeText.text = questionTypes[i];
            button.icon.sprite = icons[i];
            button.typeChooser = this;
        }
        builderManager.instructionIndex = 0;
        instructionDropdown.onValueChanged.AddListener((int value) => {
            builderManager.instructionIndex = value;
        });
    }

    public void ChooseButton(int id)
    {
        Debug.Log("Chosen type: " + questionTypes[id]);
        builderManager.typeIndex = id;
    }


}
