using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QConverter : MonoBehaviour
{
    // How to use:
    // Copy all question .txt files from the mobile device or computer persistant data path, and drag them into the editor in the folder below.
    // Hit the convert button, and all image file paths should be updated

    public void CovertFiles()
    {
        string questionsFolderPath = $"{Application.dataPath}/Questions/NewSystem/Questions/";
        string newImageFolderPath = $"{Application.dataPath}/Questions/NewSystem/Images/";
        DirectoryInfo directoryInfo = new DirectoryInfo(questionsFolderPath);
        FileInfo[] savedQuestions = directoryInfo.GetFiles("*.txt");
        foreach (FileInfo question in savedQuestions) {
            string saveString = File.ReadAllText(question.FullName);
            SaveQuestionObject saveObject = JsonUtility.FromJson<SaveQuestionObject>(saveString);
            if (!string.IsNullOrEmpty(saveObject.refImageFilePath)) {
                string fileName = Path.GetFileName(saveObject.refImageFilePath);
                saveObject.refImageFilePath = newImageFolderPath + fileName;
            }
            if (saveObject.imageAnswerFilePaths.Count > 0) {
                List<string> newImgAnswerPaths = new List<string>();
                foreach (string imgAnswerPath in saveObject.imageAnswerFilePaths) {
                    string fileName = Path.GetFileName(imgAnswerPath);
                    newImgAnswerPaths.Add(newImageFolderPath + fileName);
                }
                saveObject.imageAnswerFilePaths = newImgAnswerPaths;
            }
            if (!string.IsNullOrEmpty(saveObject.tapImageFilePath)) {
                string fileName = Path.GetFileName(saveObject.tapImageFilePath);
                saveObject.tapImageFilePath = newImageFolderPath + fileName;
            }
            string json = JsonUtility.ToJson(saveObject);
            File.WriteAllText(questionsFolderPath + question.Name, json);
        }
        Debug.Log("Finished converting question files!");
    }

}
