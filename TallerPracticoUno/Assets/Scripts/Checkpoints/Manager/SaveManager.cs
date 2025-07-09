using UnityEngine;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private SaveData saveData;
    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json"); // Path to save file
        LoadGame();
    }

    private void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("{SaveManager}: Partida cargada desde: " + saveFilePath);
        }
        else
        {
            saveData = new SaveData(); // Create a new save data if file doesn't exist
            Debug.Log("{SaveManager}: No se encontró el archivo de guardado, se creó uno nuevo.");
        }
    }

    public void SaveGame() 
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("{SaveManager}: Partida guardada en: " + saveFilePath);
    }

    public void UpdateLastCheckpoint(string checkPointID) 
    {
        saveData.lastCheckpointID = checkPointID;
        SaveGame();
    }

    public string GetLastCheckpointID() 
    {
        return saveData.lastCheckpointID;
    }
}
