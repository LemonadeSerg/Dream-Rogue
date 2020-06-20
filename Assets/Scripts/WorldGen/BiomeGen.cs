using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGen
{
    private int biomeCount;

    public void init(int biomeCount)
    {
        this.biomeCount = biomeCount;
    }

    public void growBiomeAll(BoardData[,] map)
    {
        while (cleanSpaceCount(map) > 0)
        {
            growBiome(map);
        }
    }

    public void placeBiomeStarts(BoardData[,] map)
    {
        for (int i = 1; i < biomeCount; i++)
        {
            int Rand = Random.Range(0, map.GetLength(0) - 1);
            int Rand2 = Random.Range(0, map.GetLength(1) - 1);
            map[Rand, Rand2].BiomeID = i;
            map[Rand, Rand2].RType = BoardData.RoomType.Boss;
        }
    }

    public void growBiome(BoardData[,] map)
    {
        BoardData[,] tempMap = new BoardData[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                tempMap[x, y] = new BoardData();
                tempMap[x, y].BiomeID = map[x, y].BiomeID;
                tempMap[x, y].RType = BoardData.RoomType.Normal;
            }
        }

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (tempMap[x, y].BiomeID != 0)
                {
                    if (x > 0)
                        if (tempMap[x - 1, y].BiomeID == 0)
                            map[x - 1, y].BiomeID = expandChance(tempMap[x, y].BiomeID, 1);
                    if (y > 0)
                        if (tempMap[x, y - 1].BiomeID == 0)
                            map[x, y - 1].BiomeID = expandChance(tempMap[x, y].BiomeID, 2);
                    if (x < map.GetLength(1) - 1)
                        if (tempMap[x + 1, y].BiomeID == 0)
                            map[x + 1, y].BiomeID = expandChance(tempMap[x, y].BiomeID, 3);
                    if (y < map.GetLength(1) - 1)
                        if (tempMap[x, y + 1].BiomeID == 0)
                            map[x, y + 1].BiomeID = expandChance(tempMap[x, y].BiomeID, 4);
                }
            }
        }
    }

    public int expandChance(int biomeID, int dir)
    {
        switch (biomeID)
        {
            case 1:
                return dir == 1 ? Random.Range(0, 4) == 1 ? biomeID : 0 :
                    dir == 2 ? Random.Range(0, 4) == 1 ? biomeID : 0 :
                    dir == 3 ? Random.Range(0, 2) == 1 ? biomeID : 0 :
                    dir == 4 ? Random.Range(0, 2) == 1 ? biomeID : 0 : 0;

            case 2:
                return dir == 1 ? Random.Range(0, 2) == 1 ? biomeID : 0 :
                    dir == 2 ? Random.Range(0, 2) == 1 ? biomeID : 0 :
                    dir == 3 ? Random.Range(0, 4) == 1 ? biomeID : 0 :
                    dir == 4 ? Random.Range(0, 4) == 1 ? biomeID : 0 : 0;

            case 3:
                return dir == 1 ? Random.Range(0, 2) == 1 ? biomeID : 0 :
                    dir == 2 ? Random.Range(0, 4) == 1 ? biomeID : 0 :
                    dir == 3 ? Random.Range(0, 2) == 1 ? biomeID : 0 :
                    dir == 4 ? Random.Range(0, 4) == 1 ? biomeID : 0 : 0;

            case 4:
                return dir == 1 ? Random.Range(0, 4) == 1 ? biomeID : 0 :
                    dir == 2 ? Random.Range(0, 2) == 1 ? biomeID : 0 :
                    dir == 3 ? Random.Range(0, 4) == 1 ? biomeID : 0 :
                    dir == 4 ? Random.Range(0, 2) == 1 ? biomeID : 0 : 0;

            default:
                return Random.Range(0, 5) == 1 ? biomeID : 0;
        }
    }

    public int cleanSpaceCount(BoardData[,] map)
    {
        int count = 0;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y].BiomeID == 0)
                    count++;
            }
        }
        return count;
    }
}