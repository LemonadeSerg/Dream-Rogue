﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapActions
{
    public List<int[]> connections;

    public void init()
    {
        connections = new List<int[]>();
    }

    private void connecteBiome(int currentBiomeID, int connectingBiomeID, BoardData[,] map)
    {
        List<Vector2> potentialSpotsF = new List<Vector2>();
        List<Vector2> potentialSpotsB = new List<Vector2>();
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y].ConnectedToOther && map[x, y].BiomeID == currentBiomeID && !areBiomesConnected(currentBiomeID, connectingBiomeID))
                {
                    if (x > 0)
                        if (map[x - 1, y].BiomeID == connectingBiomeID)
                        {
                            potentialSpotsF.Add(new Vector2(x, y));
                            potentialSpotsB.Add(new Vector2(x - 1, y));
                        }
                    if (y > 0)
                        if (map[x, y - 1].BiomeID == connectingBiomeID)
                        {
                            potentialSpotsF.Add(new Vector2(x, y));
                            potentialSpotsB.Add(new Vector2(x, y - 1));
                        }
                    if (x < map.GetLength(0) - 1)
                        if (map[x + 1, y].BiomeID == connectingBiomeID)
                        {
                            potentialSpotsF.Add(new Vector2(x, y));
                            potentialSpotsB.Add(new Vector2(x + 1, y));
                        }
                    if (y < map.GetLength(1) - 1)
                        if (map[x, y + 1].BiomeID == connectingBiomeID)
                        {
                            potentialSpotsF.Add(new Vector2(x, y));
                            potentialSpotsB.Add(new Vector2(x, y + 1));
                        }
                }
            }
        }

        if (potentialSpotsB.Count > 0)
        {
            int rand = Random.Range(0, potentialSpotsB.Count);
            map[(int)potentialSpotsF[rand].x, (int)potentialSpotsF[rand].y].DoorWay = true;
            map[(int)potentialSpotsB[rand].x, (int)potentialSpotsB[rand].y].DoorWay = true;
            if ((int)potentialSpotsF[rand].x > (int)potentialSpotsB[rand].x)
            {
                map[(int)potentialSpotsF[rand].x, (int)potentialSpotsF[rand].y].LeftWall = false;
                map[(int)potentialSpotsB[rand].x, (int)potentialSpotsB[rand].y].RightWall = false;
            }
            if ((int)potentialSpotsF[rand].x < (int)potentialSpotsB[rand].x)
            {
                map[(int)potentialSpotsF[rand].x, (int)potentialSpotsF[rand].y].RightWall = false;
                map[(int)potentialSpotsB[rand].x, (int)potentialSpotsB[rand].y].LeftWall = false;
            }
            if ((int)potentialSpotsF[rand].y > (int)potentialSpotsB[rand].y)
            {
                map[(int)potentialSpotsF[rand].x, (int)potentialSpotsF[rand].y].TopWall = false;
                map[(int)potentialSpotsB[rand].x, (int)potentialSpotsB[rand].y].BottomWall = false;
            }
            if ((int)potentialSpotsF[rand].y < (int)potentialSpotsB[rand].y)
            {
                map[(int)potentialSpotsF[rand].x, (int)potentialSpotsF[rand].y].BottomWall = false;
                map[(int)potentialSpotsB[rand].x, (int)potentialSpotsB[rand].y].TopWall = false;
            }
            int[] con1 = new int[2];
            int[] con2 = new int[2];
            con1[0] = map[(int)potentialSpotsF[rand].x, (int)potentialSpotsF[rand].y].BiomeID;
            con1[1] = map[(int)potentialSpotsB[rand].x, (int)potentialSpotsB[rand].y].BiomeID;

            connections.Add(con1);
            con2[0] = map[(int)potentialSpotsB[rand].x, (int)potentialSpotsB[rand].y].BiomeID;
            con2[1] = map[(int)potentialSpotsF[rand].x, (int)potentialSpotsF[rand].y].BiomeID;
            connections.Add(con2);
        }
    }

    private bool areBiomesConnected(int currentBiomeID, int connectingBiomeID)
    {
        foreach (int[] con in connections)
        {
            if (con[0] == currentBiomeID && con[1] == connectingBiomeID)
                return true;
        }
        return false;
    }
}