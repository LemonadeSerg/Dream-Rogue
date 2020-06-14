using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class BoardData
{
    [SerializeField]
    private int biomeID;

    public string roomName = "";

    public bool cleared;

    public bool connectedCell;

    public int BiomeID
    {
        get { return biomeID; }
        set
        {
            biomeID = value;
        }
    }

    [SerializeField]
    private Color color;

    public Color Col
    {
        get { return color; }
        set
        {
            color = value;
        }
    }

    [SerializeField]
    private bool outerShell;

    public bool OuterShell
    {
        get { return outerShell; }
        set
        {
            outerShell = value;
        }
    }

    [SerializeField]
    private bool connectedToOther;

    public bool ConnectedToOther
    {
        get { return connectedToOther; }
        set
        {
            connectedToOther = value;
        }
    }

    [SerializeField]
    private bool doorWay;

    public bool DoorWay
    {
        get { return doorWay; }
        set
        {
            doorWay = value;
        }
    }

    [SerializeField]
    private bool topWall;

    public bool TopWall
    {
        get { return topWall; }
        set
        {
            topWall = value;
        }
    }

    [SerializeField]
    private bool bottomWall;

    public bool BottomWall
    {
        get { return bottomWall; }

        set
        {
            bottomWall = value;
        }
    }

    [SerializeField]
    private bool rightWall;

    public bool RightWall
    {
        get { return rightWall; }
        set
        {
            rightWall = value;
        }
    }

    [SerializeField]
    private bool leftWall;

    public bool LeftWall
    {
        get { return leftWall; }
        set
        {
            leftWall = value;
        }
    }

    public RoomType RType
    {
        get { return rType; }
        set
        {
            rType = value;
        }
    }

    [SerializeField]
    private RoomType rType;

    public OrientationType OrType
    {
        get { return orType; }
        set
        {
            orType = value;
        }
    }

    [SerializeField]
    private OrientationType orType;

    public enum RoomType
    {
        Normal,
        Boss,
        Spawn,
        Battle,
        Village,
        Puzzle,
    }

    public enum OrientationType
    {
        Clear,
        ClearT,
        ClearR,
        ClearB,
        ClearL,
        ClearTR,
        ClearTL,
        ClearTB,
        ClearRB,
        ClearRL,
        ClearBL,
        ClearTRB,
        ClearTRL,
        ClearRBL,
        ClearBLT,
    }

    public void Init()
    {
        biomeID = 0;
        outerShell = false;
        connectedToOther = false;
    }

    public int getWallCount()
    {
        int count = 0;
        if (topWall)
            count++;
        if (bottomWall)
            count++;
        if (rightWall)
            count++;
        if (leftWall)
            count++;
        return count;
    }

    public bool getWallRightChance()
    {
        if (biomeID == 1)
        {
            if (Random.Range(0, 10) == 1)
                return true;
        }
        else if (Random.Range(0, 2) == 1)
            return true;
        else
            return false;

        return false;
    }

    public bool getWallLeftChance()
    {
        if (biomeID == 1)
        {
            if (Random.Range(0, 10) == 1)
                return true;
        }
        else if (Random.Range(0, 2) == 1)
            return true;
        else
            return false;

        return false;
    }

    public bool getWallTopChance()
    {
        if (biomeID == 2)
        {
            if (Random.Range(0, 10) == 1)
                return true;
        }
        else if (Random.Range(0, 2) == 1)
            return true;
        else
            return false;

        return false;
    }

    public bool getWallBotChance()
    {
        if (biomeID == 2)
        {
            if (Random.Range(0, 10) == 1)
                return true;
        }
        else if (Random.Range(0, 2) == 1)
            return true;
        else
            return false;

        return false;
    }
}