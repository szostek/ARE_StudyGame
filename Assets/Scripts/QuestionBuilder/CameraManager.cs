using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraManager : MonoBehaviour
{
    private QBuilderManager builderManager;
    [HideInInspector] public List<string> tempImagePaths = new List<string>();
    private string tempTapImagePath;

    private void Awake() 
    {
        builderManager = GetComponent<QBuilderManager>();    
    }

    public void TakePicture(int maxSize, Image image, int index)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            if( path != null )
            {
                int curQuestionNum = builderManager.questionIndex;
                int fileExt = index;
                string directory = Application.persistentDataPath + "/TempImages";
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }
                string tempPath = $"{directory}/image_{curQuestionNum}_{fileExt}.jpg";
                FileInfo newFile = new FileInfo(tempPath);
                // while (newFile.Exists) {
                //     fileExt++;
                //     tempPath = $"{directory}/image_{curQuestionNum}_{fileExt}.jpg";
                //     newFile = new FileInfo(tempPath);
                // }
                if (newFile.Exists) {
                    File.Delete(tempPath);
                }
                File.Copy(path, tempPath);
                tempImagePaths.Add(tempPath);
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath( tempPath, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + tempPath );
                    return;
                }
                Sprite imageSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                image.sprite = imageSprite;
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }

    public List<string> SavePictures()
    {
        string directory = Application.persistentDataPath + "/CustomImages";
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        List<string> newPaths = new List<string>();
        foreach (string path in tempImagePaths) {
            FileInfo file = new FileInfo(path);
            string newPath = $"{directory}/{file.Name}";

            // if a file with same name exists, delete and replace it:
            FileInfo newFile = new FileInfo(newPath);
            if (newFile.Exists) {
                File.Delete(newPath);
            }
            
            newPaths.Add(newPath);
            File.Copy(path, newPath);

            // If the saved file is a tap-image, save it as temp so it can be deleted if user goes home...
            if (file.Name.Contains("777")) {
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
        Texture2D texture = NativeCamera.LoadImageAtPath( path, 512 );
        if( texture == null )
        {
            Debug.Log( "Couldn't load texture from " + path );
            return null;
        }
        Sprite image = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        return image;
    }

    //Make a function to delete all temp photos if user goes home
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
            if (file.Name.Contains("666")) {
                File.Delete(file.FullName);
                return true;
            }
        }
        return false;
    }

    public void RemoveTempTapImageIfValid()
    {
        if (string.IsNullOrEmpty(tempTapImagePath)) return;
        File.Delete(tempTapImagePath);
    }

}
