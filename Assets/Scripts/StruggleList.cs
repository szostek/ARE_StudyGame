using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StruggleList : MonoBehaviour
{
    [SerializeField] StruggleButton struggleButtonPrefab;
    [SerializeField] RectTransform content;
    private GameManager gameManager;
    private CategoryList categoryList;    

    private void Awake() 
    {
        gameManager = GetComponent<GameManager>();
        categoryList = GetComponent<CategoryList>();
    }

    // Called from StatusController:
    public void PopulateStruggles(List<int> struggles)
    {
        foreach (RectTransform item in content) {
            Destroy(item.gameObject);
        }
        foreach (int struggleId in struggles) {
            StruggleButton button = Instantiate(struggleButtonPrefab, content);
            // button.buttonBackgroundImage.color = categoryList.backgroundColors[gameManager.GetStruggleButtonColorIndex(struggleId)];
            button.struggleList = this;
            button.qIndex = struggleId;
            button.buttonText.text = "Q# " + struggleId;
        }
    }

    public void GetStruggleQuestion(int qIndex)
    {
        gameManager.CreateStrugglePreviewQuestion(qIndex);
    }
}
