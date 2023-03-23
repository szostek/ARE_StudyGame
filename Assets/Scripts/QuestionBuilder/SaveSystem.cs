using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/SavedQuestions/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void SaveQuestion(string saveString, int questionIndex)
    {
        File.WriteAllText(SAVE_FOLDER + $"/Question_{questionIndex}.txt", saveString);
    }

    public static List<string> LoadAllQuestions()
    {
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] savedQuestions = directoryInfo.GetFiles("*.txt");
        
        List<string> questionList = new List<string>();
        foreach (FileInfo fileInfo in savedQuestions) {
            string saveString = File.ReadAllText(fileInfo.FullName);
            questionList.Add(saveString);
        }
        if (questionList.Count > 0) {
            return questionList;
        } else {
            return null;
        }

        // Load in all built-in questions here
    }
}
