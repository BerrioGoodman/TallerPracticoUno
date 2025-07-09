using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string lastCheckpointID;

    public SaveData() 
    {
        lastCheckpointID = "start_point";
    }
}


