using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadExternalRooms
{
    public string WorldPath = Application.dataPath + "/Rooms/";

    public void loadRooms()
    {
        if (!Directory.Exists(WorldPath))
            Directory.CreateDirectory(WorldPath);

        foreach (string file in System.IO.Directory.GetFiles(WorldPath))
        {
            if (file.EndsWith(".json"))
            {
                LoadJson(file);
            }
        }
    }

    private void LoadJson(string fileName)
    {
        using (System.IO.StreamReader stream = new System.IO.StreamReader(fileName))
        {
            string json = stream.ReadToEnd();
            ScenePersistantData.rooms.Add(JsonUtility.FromJson<RoomData>(json));
        }
    }
}