using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CameraManager : MonoBehaviour
{
    private QBuilderManager builderManager;
    [HideInInspector] public List<string> tempImagePaths = new List<string>();
    [HideInInspector] public string tempTapImagePath;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();    
    }

    public void TakePicture(int maxSize, Image image, int index, Action<bool> onCompleted)
    {
		if(NativeCamera.IsCameraBusy()) return;
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            if( path != null )
            {
                int curQuestionNum = builderManager.questionIndex;
                int fileExt = index;
                string directory = Path.Combine(Application.persistentDataPath, "TempImages");
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }
                string tempPath = Path.Combine(directory, $"image_{curQuestionNum}_{fileExt}.jpg");
                FileInfo newFile = new FileInfo(tempPath);
                if (newFile.Exists) {
                    File.Delete(tempPath);
                    tempImagePaths.Remove(tempPath);
                    Debug.Log("File was deleted and replaced.");
                }
                File.Copy(path, tempPath);
                if (!tempImagePaths.Contains(tempPath)) {
                    tempImagePaths.Add(tempPath);
                }
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath( tempPath, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + tempPath );
                    onCompleted(false);
                    return;
                }
                Sprite imageSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                image.sprite = imageSprite;
                onCompleted(true);
            }
        }, maxSize );
        Debug.Log( "Permission result: " + permission );

    }

    public void UploadPictureFromGallery(int maxSize, Image image, int index, Action<bool> onCompleted)
    {
        if(NativeGallery.IsMediaPickerBusy()) return;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if(path != null)
            {
                int curQuestionNum = builderManager.questionIndex;
                int fileExt = index;
                string directory = Path.Combine(Application.persistentDataPath, "TempImages");
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }
                string tempPath = Path.Combine(directory, $"image_{curQuestionNum}_{fileExt}.jpg");
                FileInfo newFile = new FileInfo(tempPath);
                if (newFile.Exists) {
                    File.Delete(tempPath);
                }
                File.Copy(path, tempPath);
                tempImagePaths.Add(tempPath);
                // Create a Texture2D from the captured image
                Texture2D texture = NativeGallery.LoadImageAtPath( tempPath, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + tempPath );
                    onCompleted(false);
                    return;
                }
                Sprite imageSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                image.sprite = imageSprite;
                onCompleted(true);
            }
        } );
        Debug.Log( "Permission result: " + permission );
    }

    public List<string> SavePictures()
    {
        string directory = Path.Combine(Application.persistentDataPath, "CustomImages");
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        List<string> newPaths = new List<string>();
        foreach (string path in tempImagePaths) {
            FileInfo file = new FileInfo(path);
            string newPath = Path.Combine(directory, file.Name);

            // if a file with same name exists, delete and replace it:
            FileInfo newFile = new FileInfo(newPath);
            if (newFile.Exists) {
                File.Delete(newPath);
            }
            
            newPaths.Add(newPath);
            File.Copy(path, newPath);

            // If the saved file is a tap-image, save it as temp so it can be deleted if user goes home...
            if (file.Name.Contains("_777")) {
                tempTapImagePath = newPath;
            }
        }
        foreach (string path in tempImagePaths) {
            File.Delete(path);
        }
        tempImagePaths.Clear();
        return newPaths;
    }

    public Sprite LoadImageFromPath(string path)
    {
        if (path.Contains("Resources")) {
            string fileName = Path.GetFileNameWithoutExtension(path);
            Debug.Log(fileName);
            Sprite sprite = Resources.Load<Sprite>(Path.Combine("Images", fileName));
            return sprite;
        } else {
            Texture2D texture = NativeCamera.LoadImageAtPath( path, 512 );
            if( texture == null )
            {
                Debug.Log( "Couldn't load texture from " + path );
                return null;
            }
            Sprite image = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            return image;
        }
    }

    //Deletes all temp photos if user goes home
    public void RemoveAllTempImages()
    {
        if (tempImagePaths.Count > 0) {
            foreach (string tempPath in tempImagePaths) {
                File.Delete(tempPath);
            }
            tempImagePaths.Clear();
        }
    }

    public bool RemoveTempRefImage()
    {
        foreach (string path in tempImagePaths) {
            FileInfo file = new FileInfo(path);
            if (file.Name.Contains("_666")) {
                File.Delete(file.FullName);
                return true;
            }
        }
        return false;
    }

    public void RemoveRefImage(int qIndex)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "CustomImages");
        if (Directory.Exists(folderPath))
        {
            // Get all files in the folder
            string[] files = Directory.GetFiles(folderPath);

            // Iterate through all files
            foreach (string file in files)
            {
                // Get the file name without the extension
                string fileName = Path.GetFileNameWithoutExtension(file);

                // Check if the file name contains the int value
                if (fileName.Contains($"_{qIndex}_666"))
                {
                    // Delete the file
                    File.Delete(file);
                }
            }
        }
        else
        {
            Debug.LogWarning("CustomImages folder not found.");
        }
    }

    public void RemoveTempTapImageIfValid()
    {
        if (string.IsNullOrEmpty(tempTapImagePath)) return;
        File.Delete(tempTapImagePath);
    }

    public void CopyAnswerImagesToTemp(int qIndex)
    {
        tempImagePaths.Clear();
        string customFolder = Path.Combine(Application.persistentDataPath, "CustomImages");
        string tempFolder = Path.Combine(Application.persistentDataPath, "TempImages");
        if (Directory.Exists(customFolder))
        {
            string[] files = Directory.GetFiles(customFolder);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.Contains($"_{qIndex}_"))
                {
                    string newTempPath = Path.Combine(tempFolder, Path.GetFileName(file));
                    if (File.Exists(newTempPath)) {
                        File.Delete(newTempPath);
                    }
                    File.Copy(file, newTempPath);
                    tempImagePaths.Add(newTempPath);
                }
            }
        }
    }

    public void DeleteQuestionImages(int qIndex)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "CustomImages");
        if (Directory.Exists(folderPath))
        {
            // Get all files in the folder
            string[] files = Directory.GetFiles(folderPath);

            // Iterate through all files
            foreach (string file in files)
            {
                // Get the file name without the extension
                string fileName = Path.GetFileNameWithoutExtension(file);

                // Check if the file name contains the int value
                if (fileName.Contains($"_{qIndex}_"))
                {
                    // Delete the file
                    File.Delete(file);
                }
            }
        }
        else
        {
            Debug.LogWarning("CustomImages folder not found.");
        }
    }


}
