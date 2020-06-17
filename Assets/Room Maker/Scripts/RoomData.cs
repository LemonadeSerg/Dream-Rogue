using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//None unique part of a board this is the template that a room follows when building
//For unique changes boarddata deals with this
[Serializable]
public class RoomData
{
    public string roomName;
    public string[] decorationBTiles;
    public string[] decorationFTiles;
    public string[] backgroundTiles;
    public string[] collisionTiles;
    public int roomSize;
    public int biomeID;
    public BoardData.RoomType roomType;
    public BoardData.OrientationType orientationType;
    public string[] entityName;
    public Vector2[] entityPos;

    public string ToString()
    {
        return roomName;
    }
}