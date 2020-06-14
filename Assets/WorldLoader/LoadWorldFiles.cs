using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWorldFiles
{
    public string worldName;
    public WorldInfo worldinfo;
    public BoardData[,] map;
    public GameObject[,] gMap;
    public string WorldPath = Application.dataPath + "/Saves/";

    public int roomSize = 20;

    public LoadWorldFiles(string worldName)
    {
        using (System.IO.StreamReader stream = new System.IO.StreamReader(WorldPath + "World - " + worldName + ".json"))
        {
            string json = stream.ReadToEnd();
            worldinfo = JsonUtility.FromJson<WorldInfo>(json);
        }
        map = new BoardData[worldinfo.width, worldinfo.height];
        gMap = new GameObject[worldinfo.width, worldinfo.height];
        for (int x = 0; x < worldinfo.width; x++)
        {
            for (int y = 0; y < worldinfo.width; y++)
            {
                using (System.IO.StreamReader stream = new System.IO.StreamReader(WorldPath + worldName + "/" + "World - " + worldName + " Part X-" + x.ToString() + " Y-" + y.ToString() + ".json"))
                {
                    string json = stream.ReadToEnd();
                    map[x, y] = JsonUtility.FromJson<BoardData>(json);
                }
                gMap[x, y] = new GameObject("X:" + x.ToString() + " Y:" + y.ToString());
                gMap[x, y].transform.position = new Vector2(x * roomSize, y * roomSize);
                gMap[x, y].AddComponent<RoomManager>();
                gMap[x, y].GetComponent<RoomManager>().boardData = map[x, y];
            }
        }
    }
}