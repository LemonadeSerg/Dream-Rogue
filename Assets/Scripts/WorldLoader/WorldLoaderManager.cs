using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoaderManager : MonoBehaviour
{
    private LoadWorldFiles loadWorld;

    public WorldInfo worldinfo;
    public BoardData[,] map;
    public GameObject[,] gMap;
    public bool[,] loaded;
    public bool[,] loading;

    public bool[,] tiled;

    public int[,] killCount;
    public GameObject player;

    public int roomSize = 20;

    public UnityEngine.Tilemaps.Tilemap backgroundTiles;
    public UnityEngine.Tilemaps.Tilemap decorationBTiles;
    public UnityEngine.Tilemaps.Tilemap collisionTiles;
    public UnityEngine.Tilemaps.Tilemap decorationfTiles;

    public Vector2Int lastRoom;

    public bool firstRoom = true;

    public float unloadDelay = 0.1f;

    // Start is called before the first frame update
    private void Start()
    {
        loadWorld = new LoadWorldFiles(ScenePersistantData.worldName);
        worldinfo = loadWorld.worldinfo;
        map = new BoardData[worldinfo.width, worldinfo.height];
        gMap = new GameObject[worldinfo.width, worldinfo.height];
        map = loadWorld.map;
        gMap = loadWorld.gMap;
        loaded = new bool[100, 100];
        killCount = new int[100, 100];
        loading = new bool[100, 100];
        player.transform.position = new Vector3Int((int)worldinfo.playerPos.x, (int)worldinfo.playerPos.y, 0);
        RoomChange(GetBoardAtVector(worldinfo.playerPos.x, worldinfo.playerPos.y), GetBoardAtVector(worldinfo.playerPos.x, worldinfo.playerPos.y));
        firstRoom = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GetBoardAtVector(player.transform.position.x, player.transform.position.y) != lastRoom)
        {
            RoomChange(GetBoardAtVector(player.transform.position.x, player.transform.position.y), lastRoom);
        }
        lastRoom = GetBoardAtVector(player.transform.position.x, player.transform.position.y);
    }

    public void RoomChange(Vector2Int curentRoom, Vector2Int lastRoom)
    {
        print("Changing from room : " + lastRoom.ToString() + " to room : " + curentRoom.ToString());
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                object[] parms = new object[2] { new Vector2Int(x, y) + curentRoom, curentRoom - lastRoom };
                StartCoroutine("LoadRoom", parms);
            }
        }
        if (curentRoom.x > lastRoom.x)
        {
            for (int y = -1; y <= 1; y++)
            {
                object[] parms = new object[2] { new Vector2Int(-1, y) + lastRoom, curentRoom - lastRoom };
                StartCoroutine("UnloadRoom", parms);
            }
        }
        else
        if (curentRoom.x < lastRoom.x)
        {
            for (int y = -1; y <= 1; y++)
            {
                object[] parms = new object[2] { new Vector2Int(1, y) + lastRoom, curentRoom - lastRoom };
                StartCoroutine("UnloadRoom", parms);
            }
        }
        else
        if (curentRoom.y > lastRoom.y)
        {
            for (int x = -1; x <= 1; x++)
            {
                object[] parms = new object[2] { new Vector2Int(x, -1) + lastRoom, curentRoom - lastRoom };
                StartCoroutine("UnloadRoom", parms);
            }
        }
        else
       if (curentRoom.y < lastRoom.y)
        {
            for (int x = -1; x <= 1; x++)
            {
                object[] parms = new object[2] { new Vector2Int(x, 1) + lastRoom, curentRoom - lastRoom };
                StartCoroutine("UnloadRoom", parms);
            }
        }
    }

    public Vector2Int GetBoardAtVector(float x, float y)
    {
        return new Vector2Int((int)(x / roomSize), (int)(y / roomSize));
    }

    private void loadTilesToMap(Vector2 pos, int x, int y, RoomData loadingRoomData)
    {
        backgroundTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.backgroundTiles[(int)(x * roomSize + y)]));
        collisionTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.collisionTiles[(int)(x * roomSize + y)]));
        decorationBTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.decorationBTiles[(int)(x * roomSize + y)]));
        decorationfTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.decorationFTiles[(int)(x * roomSize + y)]));
    }

    private void unloadTilesToMap(Vector2 pos, int x, int y)
    {
        backgroundTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), null);
        collisionTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), null);
        decorationBTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), null);
        decorationfTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), null);
    }

    private IEnumerator LoadRoom(object[] parms)
    {
        Vector2Int pos = (Vector2Int)parms[0];
        Vector2Int moveDir = (Vector2Int)parms[1];
        if (pos.x >= 0 && pos.y >= 0 && pos.x < roomSize && pos.y < roomSize)
        {
            if (!loaded[pos.x, pos.y])
            {
                loaded[pos.x, pos.y] = true;
                loading[pos.x, pos.y] = true;
                RoomData loadingRoomData;
                if (map[pos.x, pos.y].roomName == "")
                {
                    loadingRoomData = FindValidRoom(map[pos.x, pos.y].BiomeID, map[pos.x, pos.y].RType, map[pos.x, pos.y].OrType);
                }
                else
                {
                    loadingRoomData = FindRoomFromName(map[pos.x, pos.y].roomName);
                }

                if (moveDir.x > 0)
                    for (int x = 0; x < roomSize; x++)
                    {
                        for (int y = 0; y < roomSize; y++)
                        {
                            if (!firstRoom)
                            {
                                loadTilesToMap(pos, x, y, loadingRoomData);
                                yield return null;
                            }
                        }
                    }
                else if (moveDir.x < 0)
                    for (int x = roomSize - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < roomSize; y++)
                        {
                            loadTilesToMap(pos, x, y, loadingRoomData);
                            if (!firstRoom)
                            {
                                yield return null;
                            }
                        }
                    }
                else if (moveDir.y > 0)
                    for (int y = 0; y < roomSize; y++)
                    {
                        for (int x = 0; x < roomSize; x++)
                        {
                            if (!firstRoom)
                            {
                                loadTilesToMap(pos, x, y, loadingRoomData);
                                yield return null;
                            }
                        }
                    }
                else
                    for (int y = roomSize - 1; y >= 0; y--)
                    {
                        for (int x = 0; x < roomSize; x++)
                        {
                            loadTilesToMap(pos, x, y, loadingRoomData);
                            if (!firstRoom)
                            {
                                yield return null;
                            }
                        }
                    }

                if (map[pos.x, pos.y].connectedUp || map[pos.x, pos.y].connectedRight || map[pos.x, pos.y].connectedLeft || map[pos.x, pos.y].connectedDown)
                {
                    // loadConnectingCells(pos);
                }

                if (map[pos.x, pos.y].uniqueID == 0)
                {
                    map[pos.x, pos.y].uniqueID = Random.Range(0, 99999);
                }

                for (int i = 0; i < loadingRoomData.EntityName.Length; i++)
                {
                    SpawnEntity(ScenePersistantData.getEntityFromName(loadingRoomData.EntityName[i]), loadingRoomData.entityUniqueDatas[i], loadingRoomData.EntityPos[i] + (pos * roomSize));
                    if (!firstRoom)
                    {
                        yield return null;
                    }
                }
                loading[pos.x, pos.y] = false;
            }
        }
    }

    public EntityBase SpawnEntity(EntityBase eb, EntityUniqueData euq, Vector2 pos)
    {
        GameObject gb = new GameObject(eb.name);
        EntityBase eb2 = gb.AddComponent<EntityBase>();
        gb.transform.position = pos;
        eb2.name = eb.name;
        eb2.sprite = eb.sprite;
        eb2.solid = eb.solid;
        eb2.pushable = eb.pushable;
        eb2.pickable = eb.pickable;
        eb2.staticObj = eb.staticObj;
        eb2.hitB = eb.hitB;
        eb2.actB = eb.actB;
        eb2.intB = eb.intB;
        eb2.updB = eb.updB;
        eb2.staticObj = eb.staticObj;

        eb2.uq = new EntityUniqueData();
        eb2.uq.health = euq.health;
        eb2.uq.speed = euq.speed;
        eb2.uq.power = euq.power;
        eb2.uq.message = euq.message;
        eb2.uq.bombSpawn = euq.bombSpawn;
        eb2.uq.fuse = euq.fuse;
        eb2.uq.bombSpawn = euq.bombSpawn;
        eb2.uq.damage = euq.damage;
        eb2.uq.switchCode = euq.switchCode;
        eb2.uq.switchMode = euq.switchMode;

        eb2.init();
        return eb2;
    }

    public UnityEngine.Tilemaps.TileBase getTileBasefromName(string name)
    {
        foreach (UnityEngine.Tilemaps.TileBase tb in ScenePersistantData.tileBases)
        {
            if (tb.name == name)
                return tb;
        }

        return null;
    }

    public IEnumerator UnloadRoom(object[] parms)
    {
        Vector2Int pos = (Vector2Int)parms[0];
        Vector2Int moveDir = (Vector2Int)parms[1];
        if (pos.x >= 0 && pos.y >= 0 && pos.x < roomSize && pos.y < roomSize)
        {
            if (loaded[pos.x, pos.y] && !loading[pos.x, pos.y])
            {
                loaded[pos.x, pos.y] = false;
                if (moveDir.x < 0)
                    for (int x = 0; x < roomSize; x++)
                    {
                        for (int y = 0; y < roomSize; y++)
                        {
                            if (!firstRoom && !loading[pos.x, pos.y])
                            {
                                unloadTilesToMap(pos, x, y);

                                yield return new WaitForSeconds(unloadDelay);
                            }
                        }
                    }
                else if (moveDir.x > 0)
                    for (int x = roomSize - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < roomSize; y++)
                        {
                            if (!loading[pos.x, pos.y])
                            {
                                unloadTilesToMap(pos, x, y);
                                if (!firstRoom)
                                {
                                    yield return new WaitForSeconds(unloadDelay);
                                }
                            }
                        }
                    }
                else if (moveDir.y > 0)
                    for (int x = 0; x < roomSize; x++)
                    {
                        for (int y = 0; y < roomSize; y++)
                        {
                            if (!loading[pos.x, pos.y])
                            {
                                unloadTilesToMap(pos, x, y);
                                if (!firstRoom)
                                {
                                    yield return new WaitForSeconds(unloadDelay);
                                }
                            }
                        }
                    }
                else
                    for (int x = 0; x < roomSize; x++)
                    {
                        for (int y = roomSize - 1; y >= 0; y--)
                        {
                            if (!loading[pos.x, pos.y])
                            {
                                unloadTilesToMap(pos, x, y);
                                if (!firstRoom)
                                {
                                    yield return new WaitForSeconds(unloadDelay);
                                }
                            }
                        }
                    }
            }
        }
    }

    public RoomData FindValidRoom(int biomeID, BoardData.RoomType roomType, BoardData.OrientationType orientationType)
    {
        List<RoomData> validRoom = new List<RoomData>();
        for (int i = 0; i < ScenePersistantData.rooms.Count; i++)
        {
            if (ScenePersistantData.rooms[i].orientationType == orientationType && ScenePersistantData.rooms[i].biomeID == biomeID)
            {
                validRoom.Add(ScenePersistantData.rooms[i]);
            }
        }
        if (validRoom.Count > 0)
        {
            print("Finding Room with Biome ID: " + biomeID + " room type: " + roomType.ToString() + " orientation type: " + orientationType.ToString());

            int Rand = Random.Range(0, validRoom.Count);
            print(validRoom.Count.ToString() + " valid rooms found. Selected room:" + validRoom[Rand]);

            return validRoom[Rand];
        }
        print("Finding Room with Biome ID: " + biomeID + " room type: " + roomType.ToString() + " orientation type: " + orientationType.ToString());
        print("no valid room found");
        return ScenePersistantData.rooms[0];
    }

    public RoomData FindRoomFromName(string name)
    {
        for (int i = 0; i < ScenePersistantData.rooms.Count; i++)
        {
            if (ScenePersistantData.rooms[i].roomName == name)
            {
                return ScenePersistantData.rooms[i];
            }
        }
        return ScenePersistantData.rooms[0];
    }
}