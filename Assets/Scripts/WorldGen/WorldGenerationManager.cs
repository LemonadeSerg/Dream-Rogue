﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationManager : MonoBehaviour
{
    public BiomeGen biomeGen;
    public BoardTraitGen traitGen;
    public WorldMapActions biomeActions;
    public WallGeneration newWallGen;
    public WorldSaver worldSaver;

    public Vector2Int mapSize = new Vector2Int(100, 100);
    public BoardData[,] map;

    public int biomeCount;

    private bool biomeGrown = false;
    private bool traitsMarked = false;
    private bool ouWallsGenerated = false;
    private int seed;

    // Start is called before the first frame update
    public void gen(int seed)
    {
        Random.InitState(seed);
        this.seed = seed;
        biomeGen = new BiomeGen();
        traitGen = new BoardTraitGen();
        biomeActions = new WorldMapActions();
        newWallGen = new WallGeneration();
        worldSaver = new WorldSaver();
        biomeGrown = false;
        traitsMarked = false;
        ouWallsGenerated = false;

        init();
    }

    // Update is called once per frame
    private void doGeneration()
    {
        while (!biomeGrown)
        {
            biomeGen.growBiome(map);
            if (biomeGen.cleanSpaceCount(map) == 0)
                biomeGrown = true;
        }
        while (!ouWallsGenerated)
        {
            newWallGen.wallOffBiomes(map);
            ouWallsGenerated = true;
        }
        while (!newWallGen.wallGened)
        {
            newWallGen.buildWall(map, 60);
        }
        while (!traitsMarked)
        {
            traitGen.traitMarkBiome(map);
            traitsMarked = true;
        }
        worldSaver.saveRoomInd(map, seed.ToString(), traitGen.spawnPoint);
        ScenePersistantData.worldName = seed.ToString();
    }

    private void init()
    {
        map = new BoardData[mapSize.x, mapSize.y];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                map[x, y] = new BoardData();
            }
        }
        biomeGen.init(biomeCount);
        biomeActions.init();
        biomeGen.placeBiomeStarts(map);
        doGeneration();
    }
}