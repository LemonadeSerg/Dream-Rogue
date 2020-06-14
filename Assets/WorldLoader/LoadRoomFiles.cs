using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadRoomFiles
{
    public string WorldPath = Application.dataPath + "/Rooms/";

    public List<RoomData> rooms;


    public void loadRooms()
    {
        if (!Directory.Exists(WorldPath))
            Directory.CreateDirectory(WorldPath);
        rooms = new List<RoomData>();
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
            rooms.Add(JsonUtility.FromJson<RoomData>(json));
        }
    }
}
