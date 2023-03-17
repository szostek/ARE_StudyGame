using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    public void SaveChallengeStats(int timeInterval, string numScore, string percScore, string timeScore)
    {
        Debug.Log($"Saving stats for interval: {timeInterval}... numScore: {numScore}, percScore: {percScore}, timeScore: {timeScore}");
        if (timeInterval == 6) {

        } else if (timeInterval == 12) {

        } else if (timeInterval == 24) {

        } else if (timeInterval == 60) {
            
        }
    }

    public void AddQuestionToStruggleList(int questionIndex)
    {
        Debug.Log("Added " + questionIndex + " to struggle list!");
    }
}
