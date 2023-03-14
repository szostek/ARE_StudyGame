using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CategoryList : MonoBehaviour
{
    private GameManager gameManager;

    [Header("References:")]
    public CategoryButton categoryButtonPrefab;
    public RectTransform content;
    public Color[] backgroundColors;
    public Button nextButton;

    private string[] acronyms = new string[] {"PcM", "PjM", "PA", "PPD", "PDD", "CE"};
    private string[] titles = new string[] 
    {
        "Practice Management\n100 Questions",
        "Project Management\n100 Questions",
        "Programming & Analysis\n100Questions",
        "Project Planning & Design\n100Questions",
        "Project Development & Doc\n100Questions",
        "Construction & Evaluation\n100Questions",
    };

    private void Start() 
    {
        gameManager = GetComponent<GameManager>();

        for (int i = 0; i < titles.Length; i++) {
            CategoryButton button = Instantiate(categoryButtonPrefab, content);
            button.categoryList = this;
            button.backgroundColor = backgroundColors[i];
            button.titleText.text = titles[i];
            button.acronymText.text = acronyms[i];
            button.buttonId = i;
        }
    }

    private void Update() 
    {
        nextButton.interactable = selectedIds.Count > 0;    
    }

    private List<int> selectedIds = new List<int>();

    public void AddSelectedToList(int id)
    {
        selectedIds.Add(id);
    }
    public void RemoveSelectedFromList(int id)
    {
        selectedIds.Remove(id);
    }

    public void NextButton()
    {
        gameManager.selectedCategories = selectedIds.OrderBy(i => i).ToList();
        // Open the Modes panel...
    }


}
