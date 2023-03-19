using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [HideInInspector] public SaveObject so;
    [SerializeField] RectTransform scoreList;
    [SerializeField] ScoreItem scoreItemPrefab;

    private void Awake() 
    {
        if (SaveManager.Load() == null) {
            SaveManager.Save(so);
        }
        so = SaveManager.Load();
    }

    private void Start() 
    {
        LoadChallengeStats();    
    }

    public void LoadChallengeStats()
    {
        foreach (Transform item in scoreList) {
            Destroy(item.gameObject);
        }
        if (so.scores.Count > 0) {
            foreach (KeyValuePair<int, ScoreTemplate> score in so.scores) {
                ScoreItem item = Instantiate(scoreItemPrefab, scoreList);
                item.titleText.text = score.Value.challengeType;
                item.timeText.text = score.Value.timeScore;
                item.scoreText.text = $"{score.Value.percScore}%";
            }
        }
    }

    public void SaveChallengeStats(int timeInterval, float percScore, string timeScore)
    {
        Debug.Log($"Saving stats for interval: {timeInterval}... percScore: {percScore}, timeScore: {timeScore}");

        // Validate if score is better than previous:
        bool isBetterScore = false;
        if (so.scores.ContainsKey(timeInterval)) {
            isBetterScore = float.Parse(so.scores[timeInterval].percScore) < percScore;
        } else {
            isBetterScore = true;
        }

        if (timeInterval == 6) {
            if (isBetterScore) {
                SaveStats("Quickie", timeInterval, percScore, timeScore);
            }
        } else if (timeInterval == 12) {
            if (isBetterScore) {
                SaveStats("Sprint", timeInterval, percScore, timeScore);
            }
        } else if (timeInterval == 24) {
            if (isBetterScore) {
                SaveStats("Session", timeInterval, percScore, timeScore);
            }
        } else if (timeInterval == 60) {
            if (isBetterScore) {
                SaveStats("Test", timeInterval, percScore, timeScore);
            }
        }
        LoadChallengeStats();
    }

    private void SaveStats(string type, int timeInterval, float percScore, string timeScore)
    {
        ScoreTemplate score = new ScoreTemplate();
        score.challengeType = type;
        score.percScore = percScore.ToString();
        score.timeScore = timeScore;
        so.scores[timeInterval] = score;
        SaveManager.Save(so);
    }

    public void AddQuestionToStruggleList(int questionIndex)
    {
        Debug.Log("Added " + questionIndex + " to struggle list!");
    }
}
