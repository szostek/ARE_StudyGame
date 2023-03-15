using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeButtonList : MonoBehaviour
{
    [SerializeField] TimeButton timeButtonPrefab;
    [SerializeField] Button playButton;
    [SerializeField] RectTransform content;
    private GameManager gameManager;

    private int chosenIntervalIndex = -1;
    private string[] intervalNames = new string[]{"Quickie", "Sprint", "Session", "Test"};
    private string[] intervalDetails = new string[]{
        "6 Minutes\n3 Questions",
        "12 Minutes\n6 Questions",
        "24 Minutes\n12 Questions",
        "60 Minutes\n30 Questions",
    };
    private float[] timeIntervals = new float[]{6f, 12f, 24f, 60f};

    private void Start() 
    {
        gameManager = GetComponent<GameManager>();
        PopulateButtons();    
    }

    private void Update() 
    {
        if (gameManager.isChallengeMode) {
            playButton.interactable = chosenIntervalIndex > -1;    
        } else {
            playButton.interactable = true;
            chosenIntervalIndex = -1;
        }
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
    }

    public void PlayButton()
    {
        // send the chosen time interval (in minutes) to the GameManager:
        if (gameManager.isChallengeMode) {
            gameManager.selectedTimeInterval = timeIntervals[chosenIntervalIndex];
        }
        gameManager.StartGame();
    }
}
