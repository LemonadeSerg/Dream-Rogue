using System.Collections;
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

    public GUIStyle debugStyle;

    public int biomeCount = 6;

    private int bio1;

    private bool biomeGrown = false;
    private bool heatmapGenerated = false;
    private bool traitsMarked = false;
    private bool ouWallsGenerated = false;
    private bool inWallsGenerated = false;
    private int seed;

    // Start is called before the first frame update
    public void gen(int seed, int biomeCount)
    {
        Random.InitState(seed);
        this.biomeCount = biomeCount;
        this.seed = seed;
        biomeGen = new BiomeGen();
        traitGen = new BoardTraitGen();
        biomeActions = new WorldMapActions();
        newWallGen = new WallGeneration();
        worldSaver = new WorldSaver();
        biomeGrown = false;
        heatmapGenerated = false;
        traitsMarked = false;
        ouWallsGenerated = false;
        inWallsGenerated = false;

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
        while (!inWallsGenerated)
        {
            newWallGen.addFloatingWall(map, 10);
            if (newWallGen.noMore)
            {
                newWallGen.addPerimeterBreakoffs(map);
                inWallsGenerated = true;
            }
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
        worldSaver.saveRoomInd(map, seed.ToString(), new Vector2(2, 2));
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
                map[x, y].Init();
            }
        }
        biomeGen.init(biomeCount + 1);
        biomeActions.init();
        biomeGen.placeBiomeStarts(map);
        doGeneration();
    }
}