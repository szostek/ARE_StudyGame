using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryChooser : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] bCategoryButton categoryButtonPrefab;
    [SerializeField] RectTransform categoryChooserList;
    public RectTransform content;
    public Color[] backgroundColors;
    public Button nextButton;
    private int selectedId = -1;

    private QBuilderManager builderManager;

    private string[] acronyms = new string[] {"PcM", "PjM", "PA", "PPD", "PDD", "CE"};
    private string[] titles = new string[] 
    {
        "Practice Management",
        "Project Management",
        "Programming & Analysis",
        "Project Planning & Designs",
        "Project Development & Doc",
        "Construction & Evaluation",
    };

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();    
    }

    private void Start() 
    {
        for (int i = 0; i < titles.Length; i++) {
            bCategoryButton button = Instantiate(categoryButtonPrefab, content);
            button.categoryChooser = this;
            button.backgroundColor = backgroundColors[i];
            button.titleText.text = titles[i];
            button.acronymText.text = acronyms[i];
            button.buttonId = i;
        }
    }

    private void Update() 
    {
        nextButton.interactable = selectedId > -1;   
    }

    public void SelectButtonId(int id)
    {
        selectedId = id;
        Debug.Log(selectedId);
    }

    public void NextButton()
    {
        builderManager.categoryIndex = selectedId;
    }

}
