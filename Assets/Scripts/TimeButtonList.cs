using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeButtonList : MonoBehaviour
{
    [SerializeField] TimeButton timeButtonPrefab;
    [SerializeField] RectTransform content;
    private GameManager gameManager;

    private int chosenIntervalIndex;
    private string[] intervalNames = new string[]{"Quickie", "Session", "Test"};
    private string[] intervalDetails = new string[]{
        "6 Minutes\n3 Questions",
        "20 Minutes\n10 Questions",
        "60 Minutes\n30 Questions",
    };

    private void Start() 
    {
        gameManager = GetComponent<GameManager>();
        PopulateButtons();    
    }

    public void PopulateButtons()
    {
        for (int i = 0; i < intervalNames.Length; i++) {
            TimeButton button = Instantiate(timeButtonPrefab, content);
            button.intervalNameText.text = intervalNames[i];
            button.intervalDetailsText.text = intervalDetails[i];
            button.buttonIndex = i;
            button.timeButtonList = this;
        }
    }

    public void SetIntervalIndex(int index)
    {
        chosenIntervalIndex = index;
        Debug.Log("Chosen interval: " + chosenIntervalIndex);
    }

    public void PlayButton()
    {
        gameManager.selectedTimeInterval = chosenIntervalIndex;
    }
}
