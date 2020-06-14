using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomData
{
    public string roomName;
    public string[] backgroundTiles;
    public string[] collisionTiles;
    public int roomSize;
    public int biomeID;
    public BoardData.RoomType roomType;
    public BoardData.OrientationType orientationType;

    public string ToString()
    {
        return roomName;
    }
}