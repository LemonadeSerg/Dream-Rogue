using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//None unique part of a board this is the template that a room follows when building
//For unique changes boarddata deals with this
public class RoomPreload : MonoBehaviour
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
    public string[] EntityName;
    public Vector2[] EntityPos;
    public EntityUniqueData[] entityUniqueDatas;
}