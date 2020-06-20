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
    public int[,] killCount;
    public GameObject player;

    public int roomSize = 20;

    public UnityEngine.Tilemaps.Tilemap backgroundTiles;
    public UnityEngine.Tilemaps.Tilemap decorationBTiles;
    public UnityEngine.Tilemaps.Tilemap collisionTiles;
    public UnityEngine.Tilemaps.Tilemap decorationfTiles;

    private System.DateTime beginRoomLoad;

    public Vector2Int lastRoom;

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
        player.transform.position = new Vector3Int((int)worldinfo.playerPos.x, (int)worldinfo.playerPos.y, 0);
        loadConnectingCells(getBoardAtVector(worldinfo.playerPos.x, worldinfo.playerPos.y));
        loadRoom(getBoardAtVector(worldinfo.playerPos.x, worldinfo.playerPos.y));
    }

    // Update is called once per frame
    private void Update()
    {
        if (getBoardAtVector(player.transform.position.x, player.transform.position.y) != lastRoom)
        {
            roomChange(getBoardAtVector(player.transform.position.x, player.transform.position.y), lastRoom);
        }
        lastRoom = getBoardAtVector(player.transform.position.x, player.transform.position.y);
    }

    public void roomChange(Vector2Int curentRoom, Vector2Int lastRoom)
    {
        print("Changing from room : " + lastRoom.ToString() + " to room : " + curentRoom.ToString());
        loadConnectingCells(curentRoom);
        unLoadConnectingCells(lastRoom, curentRoom - lastRoom);
    }

    public Vector2Int getBoardAtVector(float x, float y)
    {
        return new Vector2Int((int)(x / roomSize), (int)(y / roomSize));
    }

    public void loadConnectingCells(Vector2Int pos)
    {
        if (pos.x > 0 && !map[pos.x, pos.y].LeftWall)
        {
            loadRoom(new Vector2Int(pos.x - 1, pos.y));
        }
        if (pos.y > 0 && !map[pos.x, pos.y].BottomWall)
        {
            loadRoom(new Vector2Int(pos.x, pos.y - 1));
        }
        if (pos.x < worldinfo.width - 1 && !map[pos.x, pos.y].RightWall)
        {
            loadRoom(new Vector2Int(pos.x + 1, pos.y));
        }
        if (pos.y < worldinfo.height - 1 && !map[pos.x, pos.y].TopWall)
        {
            loadRoom(new Vector2Int(pos.x, pos.y + 1));
        }
    }

    public void unLoadConnectingCells(Vector2Int pos, Vector2Int dir)
    {
        if (dir != Vector2.right)
        {
            if (!map[pos.x, pos.y].RightWall)
            {
                unloadRoom(pos.x + 1, pos.y);
            }
        }
        if (dir != Vector2.left)
        {
            if (!map[pos.x, pos.y].LeftWall)
            {
                unloadRoom(pos.x - 1, pos.y);
            }
        }
        if (dir != Vector2.up)
        {
            if (!map[pos.x, pos.y].TopWall)
            {
                unloadRoom(pos.x, pos.y + 1);
            }
        }
        if (dir != Vector2.down)
        {
            if (!map[pos.x, pos.y].BottomWall)
            {
                unloadRoom(pos.x, pos.y - 1);
            }
        }
    }

    public void loadRoom(Vector2Int pos)
    {
        if (!loaded[pos.x, pos.y])
        {
            RoomData loadingRoomData;
            if (map[pos.x, pos.y].roomName == "")
            {
                loadingRoomData = findValidRoom(map[pos.x, pos.y].BiomeID, map[pos.x, pos.y].RType, map[pos.x, pos.y].OrType);
            }
            else
            {
                loadingRoomData = findRoomFromName(map[pos.x, pos.y].roomName);
            }
            loaded[pos.x, pos.y] = true;
            for (int x = 0; x < roomSize; x++)
            {
                for (int y = 0; y < roomSize; y++)
                {
                    backgroundTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.backgroundTiles[(int)(x * roomSize + y)]));
                    collisionTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.collisionTiles[(int)(x * roomSize + y)]));
                    decorationBTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.decorationBTiles[(int)(x * roomSize + y)]));
                    decorationfTiles.SetTile(new Vector3Int((int)((pos.x * roomSize) + x), (int)((pos.y * roomSize) + y), 0), getTileBasefromName(loadingRoomData.decorationFTiles[(int)(x * roomSize + y)]));
                }
            }

            if (map[pos.x, pos.y].connectedUp || map[pos.x, pos.y].connectedRight || map[pos.x, pos.y].connectedLeft || map[pos.x, pos.y].connectedDown)
            {
                loadConnectingCells(pos);
            }
            if (loadingRoomData.entityName != null && !map[pos.x, pos.y].cleared)
            {
                for (int i = 0; i < loadingRoomData.entityName.Length; i++)
                {
                    spawnEntity(ScenePersistantData.getEntityFromName(loadingRoomData.entityName[i]), (pos * roomSize) + loadingRoomData.entityPos[i], loadingRoomData.metaText[i], loadingRoomData.entityHealth[i], loadingRoomData.entityPushable[i], loadingRoomData.entitySolid[i]);
                }
                killCount[pos.x, pos.y] = loadingRoomData.entityName.Length;
            }
            if (map[pos.x, pos.y].uniqueID == 0)
            {
                map[pos.x, pos.y].uniqueID = Random.Range(0, 99999);
            }
        }
    }

    public void spawnEntity(EntityBase eb, Vector2 pos, string metaText, int health, bool pushable, bool solid)
    {
        GameObject go = new GameObject(eb.name);
        EntityBase eb2 = go.AddComponent<EntityBase>();
        eb2.sprite = eb.sprite;
        eb2.behaviour = eb.behaviour;
        eb2.metaText = metaText;
        eb2.health = health;
        eb2.Pushable = pushable;
        eb2.Solid = solid;
        eb2.wlm = this;
        eb2.init();
        go.transform.position = pos;
        eb2.OriginCell = getBoardAtVector(pos.x, pos.y);
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

    public void unloadRoom(int px, int py)
    {
        beginRoomLoad = System.DateTime.Now;
        if (loaded[px, py])
        {
            loaded[px, py] = false;
            for (int x = 0; x < roomSize; x++)
            {
                for (int y = 0; y < roomSize; y++)
                {
                    backgroundTiles.SetTile(new Vector3Int((int)((px * roomSize) + x), (int)((py * roomSize) + y), 0), null);
                    collisionTiles.SetTile(new Vector3Int((int)((px * roomSize) + x), (int)((py * roomSize) + y), 0), null);
                    decorationBTiles.SetTile(new Vector3Int((int)((px * roomSize) + x), (int)((py * roomSize) + y), 0), null);
                    decorationfTiles.SetTile(new Vector3Int((int)((px * roomSize) + x), (int)((py * roomSize) + y), 0), null);
                }
            }
        }
    }

    public RoomData findValidRoom(int biomeID, BoardData.RoomType roomType, BoardData.OrientationType orientationType)
    {
        List<RoomData> validRoom = new List<RoomData>();
        for (int i = 0; i < ScenePersistantData.rooms.Count; i++)
        {
            if (ScenePersistantData.rooms[i].orientationType == orientationType)
            {
                validRoom.Add(ScenePersistantData.rooms[i]);
            }
        }
        if (validRoom.Count > 0)
        {
            int Rand = Random.Range(0, validRoom.Count);
            return validRoom[Rand];
        }
        return ScenePersistantData.rooms[0];
    }

    public RoomData findRoomFromName(string name)
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