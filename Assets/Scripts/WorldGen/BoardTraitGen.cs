using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTraitGen
{
    public Vector2 spawnPoint;

    public void traitMarkBiome(BoardData[,] map)
    {
        addRandomSpawn(map);
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y].getWallCount() == 3)
                {
                    if (!map[x, y].TopWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearT;
                    }
                    if (!map[x, y].BottomWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearB;
                    }
                    if (!map[x, y].LeftWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearL;
                    }
                    if (!map[x, y].RightWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearR;
                    }
                }
                else
                if (map[x, y].getWallCount() == 2)
                {
                    if (!map[x, y].TopWall)
                    {
                        if (!map[x, y].BottomWall)
                        {
                            map[x, y].OrType = BoardData.OrientationType.ClearTB;
                        }
                        if (!map[x, y].LeftWall)
                        {
                            map[x, y].OrType = BoardData.OrientationType.ClearTL;
                        }
                        if (!map[x, y].RightWall)
                        {
                            map[x, y].OrType = BoardData.OrientationType.ClearTR;
                        }
                    }
                    if (!map[x, y].RightWall)
                    {
                        if (!map[x, y].BottomWall)
                        {
                            map[x, y].OrType = BoardData.OrientationType.ClearRB;
                        }
                        if (!map[x, y].LeftWall)
                        {
                            map[x, y].OrType = BoardData.OrientationType.ClearRL;
                        }
                    }
                    if (!map[x, y].BottomWall)
                    {
                        if (!map[x, y].LeftWall)
                        {
                            map[x, y].OrType = BoardData.OrientationType.ClearBL;
                        }
                    }
                }
                else
                if (map[x, y].getWallCount() == 1)
                {
                    if (map[x, y].TopWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearRBL;
                    }
                    if (map[x, y].RightWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearBLT;
                    }
                    if (map[x, y].BottomWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearTRL;
                    }
                    if (map[x, y].LeftWall)
                    {
                        map[x, y].OrType = BoardData.OrientationType.ClearTRB;
                    }
                }
                else
                {
                    map[x, y].OrType = BoardData.OrientationType.Clear;
                }
                /*
                if (map[x, y].RType == BoardData.RoomType.Normal)
                {
                    int Rand = Random.Range(0, 10);
                    if (Rand == 1)
                    {
                        map[x, y].RType = BoardData.RoomType.Village;
                    }
                    if (Rand == 2)
                    {
                        map[x, y].RType = BoardData.RoomType.Puzzle;
                    }
                    if (Rand == 3)
                    {
                        map[x, y].RType = BoardData.RoomType.Battle;
                    }
                }*/
            }
        }
    }

    private void addRandomSpawn(BoardData[,] map)
    {
        int Rand1 = Random.Range(0, map.GetLength(0));
        int Rand2 = Random.Range(0, map.GetLength(1));
        if (map[Rand1, Rand2].RType == BoardData.RoomType.Normal && map[Rand1, Rand2].BiomeID == 1)
        {
            map[Rand1, Rand2].RType = BoardData.RoomType.Spawn;
            spawnPoint = new Vector2((Rand1 * 32) + 16, (Rand2 * 32) + 16);
        }
        else
        {
            addRandomSpawn(map);
        }
    }
}