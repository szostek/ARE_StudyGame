using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerFieldList : MonoBehaviour
{
    [SerializeField] AnswerField answerFieldPrefab;
    [SerializeField] AnswerImage answerImagePrefab;
    [SerializeField] GameObject addFieldButtonPrefab;
    private GameObject currentAddFieldButton;
    [SerializeField] Switch answerTypeSwitch;

    public RectTransform answerFieldList;
    [HideInInspector] public List<AnswerField> textFieldsList = new List<AnswerField>();
    [HideInInspector] public List<AnswerImage> imageFieldsList = new List<AnswerImage>();
    private string[] textAnswers;
    private string[] imageAnswerfilePaths;
    private int[] correctAnswerIds;

    private bool toggled = false;
    private bool hasImageAnswers = false;

    private QBuilderManager builderManager;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();    
    }

    private void Update() 
    {
        if (!answerTypeSwitch.isOn && !toggled) {
            imageFieldsList.Clear();
            AddTextFieldAnswers();
            toggled = true;
            hasImageAnswers = false;
        }
        if (answerTypeSwitch.isOn && toggled) {
            textFieldsList.Clear();
            AddImageAnswers();
            toggled = false;
            hasImageAnswers = true;
        }

    }

    private void AddTextFieldAnswers()
    {        
        RemoveListItems();
        for (int i = 0; i < 2; i++) {
            AddTextFieldItem();
        }
        AddNewAnswerButton();
    }
    private void AddImageAnswers()
    {
        RemoveListItems();
        for (int i = 0; i < 2; i++) {
            AddImageFieldItem();
        }
        AddNewAnswerButton();
    }

    private void AddTextFieldItem()
    {
        AnswerField field = Instantiate(answerFieldPrefab, answerFieldList);
        field.answerFieldList = this;
        textFieldsList.Add(field);
        ApplyFieldIds();
    }
    private void AddImageFieldItem()
    {
        AnswerImage field = Instantiate(answerImagePrefab, answerFieldList);
        field.answerFieldList = this;
        imageFieldsList.Add(field);
        ApplyFieldIds();    
    }

    private void AddNewAnswerButton() //Adds the Add Answer button to end of list
    {
        GameObject addFieldButton = Instantiate(addFieldButtonPrefab, answerFieldList);
        Button button = addFieldButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => {
            Destroy(addFieldButton.gameObject);
            if (hasImageAnswers) {
                AddImageFieldItem();
            } else {
                AddTextFieldItem();
            }
            AddNewAnswerButton();
        });
        currentAddFieldButton = addFieldButton;
    }

    public void ApplyFieldIds() //Re-sorts the ids of all current buttons
    {
        if (hasImageAnswers) {
            if (imageFieldsList.Count > 0) {
                for (int i = 0; i < imageFieldsList.Count; i++) {
                    imageFieldsList[i].fieldId = i;
                }
            }
        } else {
            if (textFieldsList.Count > 0) {
                for (int i = 0; i < textFieldsList.Count; i++) {
                    textFieldsList[i].fieldId = i;
                }
            }
        }
    }

    public void DeleteField(int fieldId)
    {
        Debug.Log("Deleting field id: " + fieldId);
        if (hasImageAnswers) {
            Destroy(imageFieldsList[fieldId].gameObject);
            imageFieldsList.RemoveAt(fieldId);
        } else {
            Destroy(textFieldsList[fieldId].gameObject);
            textFieldsList.RemoveAt(fieldId);
        }
        ApplyFieldIds();
    }

    private void RemoveListItems()
    {
        foreach (RectTransform item in answerFieldList) {
            Destroy(item.gameObject);
        }
    }

    public void SaveQuestionButton()
    {
        if (hasImageAnswers) {
            builderManager.hasImageAnswers = true;

        } else {
            builderManager.hasImageAnswers = false;
            textAnswers = new string[textFieldsList.Count];
            correctAnswerIds = new int[textFieldsList.Count];
            for (int i = 0; i < textFieldsList.Count; i++) {
                textAnswers[i] = textFieldsList[i].answerTextField.text;
                correctAnswerIds[i] = textFieldsList[i].correctAnswerToggle.isOn ? 1 : 0;
            }
            builderManager.textAnswers = textAnswers;
            builderManager.correctAnswerIds = correctAnswerIds;
            builderManager.SaveQuestion();
        }
    }

}
