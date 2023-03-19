using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static string directory = "SaveData";
    public static string fileName = "MySave.txt";

    public static void Save(SaveObject so)
    {
        if (!DirectoryExists()) {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + directory);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetFullPath());
        bf.Serialize(file, so);
        file.Close();
    }

    public static SaveObject Load()
    {
        if (SaveExists()) {
            try {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(GetFullPath(), FileMode.Open);
                SaveObject so = (SaveObject)bf.Deserialize(file);
                file.Close();
                return so;
            } catch (SerializationException){
                Debug.Log("Failed to load file");
            }
        }
        return null;
    }

    private static bool SaveExists()
    {
        return File.Exists(GetFullPath());
    }

    private static bool DirectoryExists()
    {
        return Directory.Exists(Application.persistentDataPath + "/" + directory);
    }

    private static string GetFullPath()
    {
        return Application.persistentDataPath + "/" + directory + "/" + fileName;
    }
}