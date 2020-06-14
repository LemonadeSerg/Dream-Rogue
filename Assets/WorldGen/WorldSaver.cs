using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldSaver
{
    // Start is called before the first frame update
    private string WorldPath = Application.dataPath + "/Saves/";

    public void saveRoomInd(BoardData[,] roomTemp, string name, Vector2 playerPos)
    {
        WorldPath = Application.dataPath + "/Saves/";
        if (!Directory.Exists(WorldPath))
            Directory.CreateDirectory(WorldPath);
        if (!Directory.Exists(WorldPath + name.ToString() + "/"))
            Directory.CreateDirectory(WorldPath + name.ToString() + "/");
        for (int x = 0; x < roomTemp.GetLength(0); x++)
        {
            for (int y = 0; y < roomTemp.GetLength(1); y++)
            {
                using (StreamWriter stream = new StreamWriter(WorldPath + name.ToString() + "/" + "World - " + name.ToString() + " Part X-" + x.ToString() + " Y-" + y.ToString() + ".json"))
                {
                    string json = JsonUtility.ToJson(roomTemp[x, y]);
                    stream.Write(json);
                }
            }
        }
        WorldInfo worldInfo = new WorldInfo();
        worldInfo.width = roomTemp.GetLength(0);
        worldInfo.height = roomTemp.GetLength(1);
        worldInfo.playerPos = playerPos;
        using (StreamWriter stream = new StreamWriter(WorldPath + "World - " + name.ToString() + ".json"))
        {
            string json = JsonUtility.ToJson(worldInfo);
            stream.Write(json);
        }
    }
}