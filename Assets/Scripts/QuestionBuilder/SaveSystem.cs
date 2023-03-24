using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/SavedQuestions/";
    public static readonly string questionsFolderPath = $"{Application.dataPath}/Questions/NewSystem/Questions/";

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
        // This is the master list of both built-in questions, and user-created questions:
        List<string> questionList = new List<string>();

        // Load all user-created questions from their device:
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] savedQuestions = directoryInfo.GetFiles("*.txt");
        foreach (FileInfo fileInfo in savedQuestions) {
            string saveString = File.ReadAllText(fileInfo.FullName);
            questionList.Add(saveString);
        }

        // Load all built-in questions here:
        DirectoryInfo builtInQuestionDir = new DirectoryInfo(questionsFolderPath);
        FileInfo[] builtInQuestions = builtInQuestionDir.GetFiles("*.txt");
        foreach (FileInfo qInfo in builtInQuestions) {
            string qString = File.ReadAllText(qInfo.FullName);
            questionList.Add(qString);
        }

        if (questionList.Count > 0) {
            return questionList;
        } else {
            return null;
        }
    }
}
