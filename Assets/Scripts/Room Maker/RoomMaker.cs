using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RoomMaker : MonoBehaviour
{
    public Grid gridBack;
    public Tilemap tilemapBack;
    public Tilemap decBTilemap;
    public Tilemap tilemapCol;
    public Tilemap decFTilemap;

    public int currentSelection = 1;
    public int selectedGrid = 1;

    public int roomSize = 20;

    public SpriteRenderer selectorTop, selectorMid, selectorBot;

    public Dropdown OrientationDropDown;
    public Dropdown RoomTypeDropDown;
    public InputField BiomeIDField;
    public InputField RoomNameField;
    public Dropdown loadRoomName;
    public Dropdown layerChangeDd;
    public InputField metaStringIn;

    private string roomsPath;

    private bool editingEntities = false;
    private int selectionMax;

    private bool lockEntitiesToGrid = false;

    public int brushRadius = 1;

    private Vector3 mouseWorldPos;
    private Vector3Int coordinate;

    // Start is called before the first frame update

    private void Start()
    {
        roomsPath = Application.dataPath + "/Rooms/";
        for (int x = 0; x < roomSize; x++)
        {
            for (int y = 0; y < roomSize; y++)
            {
                tilemapBack.SetTile(new Vector3Int(x, y, 0), null);
                decBTilemap.SetTile(new Vector3Int(x, y, 0), null);
                tilemapBack.SetTile(new Vector3Int(x, y, 0), null);
                decFTilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        for (int i = 0; i < ScenePersistantData.tileBases.ToArray().Length; i++)
        {
            decFTilemap.SetTile(new Vector3Int(i % roomSize, (-i / roomSize) - 2, 0), ScenePersistantData.tileBases.ToArray()[i]);
        }

        List<BoardData.OrientationType> orTypes = new List<BoardData.OrientationType>();
        List<string> orTypeName = new List<string>();
        for (int i = 0; i < System.Enum.GetNames(typeof(BoardData.OrientationType)).Length; i++)
        {
            orTypes.Add((BoardData.OrientationType)i);
            orTypeName.Add(orTypes[i].ToString());
        }

        OrientationDropDown.AddOptions(orTypeName);

        List<BoardData.RoomType> rmTypes = new List<BoardData.RoomType>();
        List<string> rmTypeName = new List<string>();
        for (int i = 0; i < System.Enum.GetNames(typeof(BoardData.RoomType)).Length; i++)
        {
            rmTypes.Add((BoardData.RoomType)i);
            rmTypeName.Add(rmTypes[i].ToString());
        }

        RoomTypeDropDown.AddOptions(rmTypeName);

        List<string> roomNames = new List<string>();
        for (int i = 0; i < ScenePersistantData.rooms.Count; i++)
        {
            roomNames.Add(ScenePersistantData.rooms[i].roomName);
        }
        loadRoomName.AddOptions(roomNames);

        selectionMax = ScenePersistantData.tileBases.ToArray().Length;
    }

    // Update is called once per frame
    private void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        coordinate = gridBack.WorldToCell(mouseWorldPos);

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            LeftClick();
        }
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
        {
            RightClick();
        }
        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            paint(ScenePersistantData.tileBases.ToArray()[currentSelection]);
        }
        if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift))
        {
            paint(null);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentSelection = 0;
            editingEntities = !editingEntities;
            if (editingEntities)
                selectionMax = ScenePersistantData.entities.ToArray().Length;
            else
                selectionMax = ScenePersistantData.tileBases.ToArray().Length;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            lockEntitiesToGrid = !lockEntitiesToGrid;
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            brushRadius++;
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            brushRadius--;

        currentSelection = Mathf.Clamp(currentSelection + (int)Input.mouseScrollDelta.y, 0, selectionMax);

        print(Input.mouseScrollDelta.y);
        if (!editingEntities)
        {
            updateSelector(
                ScenePersistantData.tileSprites.ToArray()[Mathf.Clamp(currentSelection - 1, 0, selectionMax - 1)],
                ScenePersistantData.tileSprites.ToArray()[Mathf.Clamp(currentSelection, 0, selectionMax - 1)],
                ScenePersistantData.tileSprites.ToArray()[Mathf.Clamp(currentSelection + 1, 0, selectionMax - 1)]);
        }
        else
        {
            updateSelector(
                ScenePersistantData.entities.ToArray()[Mathf.Clamp(currentSelection - 1, 0, selectionMax - 1)].sprite,
                ScenePersistantData.entities.ToArray()[Mathf.Clamp(currentSelection, 0, selectionMax - 1)].sprite,
                ScenePersistantData.entities.ToArray()[Mathf.Clamp(currentSelection + 1, 0, selectionMax - 1)].sprite);
        }
    }

    private void updateSelector(Sprite top, Sprite mid, Sprite bot)
    {
        Vector3 pos;
        if (lockEntitiesToGrid)
            pos = coordinate + new Vector3(0.5f, 0.5f);
        else
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        if (top != null)
        {
            selectorTop.sprite = top;
            selectorTop.transform.position = pos + Vector3.up;
        }

        if (mid != null)
        {
            selectorMid.sprite = mid;
            selectorMid.transform.position = pos;
        }

        if (bot != null)
        {
            selectorBot.sprite = bot;
            selectorBot.transform.position = pos + Vector3.down;
        }
    }

    private void LeftClick()
    {
        if (editingEntities && coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
            if (!lockEntitiesToGrid)
                spawnEntity(mouseWorldPos);
            else
                spawnEntity(new Vector2(coordinate.x + 0.5f, coordinate.y + 0.5f));

        if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y < 0)
        {
            currentSelection = ScenePersistantData.tileIndexFromName(decFTilemap.GetTile(coordinate).name);
        }
    }

    private void RightClick()
    {
        if (coordinate.y < 0)
        {
            currentSelection = ScenePersistantData.tileIndexFromName(decFTilemap.GetTile(coordinate).name);
            for (int x = 0; x < roomSize; x++)
            {
                for (int y = 0; y < roomSize; y++)
                {
                    switch (selectedGrid)
                    {
                        case 0:
                            placeTileInMap(tilemapBack, new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
                            break;

                        case 1:
                            placeTileInMap(decBTilemap, new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
                            break;

                        case 2:
                            placeTileInMap(tilemapCol, new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
                            break;

                        case 3:
                            placeTileInMap(decFTilemap, new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
                            break;
                    }
                }
            }
        }
    }

    private void spawnEntity(Vector2 pos)
    {
        GameObject gb = new GameObject(ScenePersistantData.entities.ToArray()[currentSelection].name);
        EntityBase eb = gb.AddComponent<EntityBase>();
        gb.transform.position = pos;

        eb.name = ScenePersistantData.entities.ToArray()[currentSelection].name;
        eb.sprite = ScenePersistantData.entities.ToArray()[currentSelection].sprite;
        eb.solid = ScenePersistantData.entities.ToArray()[currentSelection].solid;
        eb.pushable = ScenePersistantData.entities.ToArray()[currentSelection].pushable;
        eb.pickable = ScenePersistantData.entities.ToArray()[currentSelection].pickable;
        eb.hitB = ScenePersistantData.entities.ToArray()[currentSelection].hitB;
        eb.actB = ScenePersistantData.entities.ToArray()[currentSelection].actB;
        eb.intB = ScenePersistantData.entities.ToArray()[currentSelection].intB;
        eb.updB = ScenePersistantData.entities.ToArray()[currentSelection].updB;
        eb.staticObj = ScenePersistantData.entities.ToArray()[currentSelection].staticObj;

        eb.uq = new EntityUniqueData();
        eb.uq.health = ScenePersistantData.entities.ToArray()[currentSelection].uq.health;
        eb.uq.speed = ScenePersistantData.entities.ToArray()[currentSelection].uq.speed;
        eb.uq.power = ScenePersistantData.entities.ToArray()[currentSelection].uq.power;
        eb.uq.fuse = ScenePersistantData.entities.ToArray()[currentSelection].uq.fuse;
        eb.uq.message = ScenePersistantData.entities.ToArray()[currentSelection].uq.message;
        eb.uq.bombSpawn = ScenePersistantData.entities.ToArray()[currentSelection].uq.bombSpawn;
        eb.uq.damage = ScenePersistantData.entities.ToArray()[currentSelection].uq.damage;
        eb.uq.switchCode = ScenePersistantData.entities.ToArray()[currentSelection].uq.switchCode;
        eb.uq.switchMode = ScenePersistantData.entities.ToArray()[currentSelection].uq.switchMode;
        eb.init();

        eb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private void placeTileInMap(Tilemap tm, Vector3Int tilePos, TileBase tb)
    {
        if (tilePos.x >= 0 && tilePos.x < roomSize && tilePos.y >= 0 && tilePos.y < roomSize)
            tm.SetTile(tilePos, tb);
    }

    private void paint(TileBase tile)
    {
        if (!editingEntities && coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
        {
            for (int x = -brushRadius + 1; x < brushRadius; x++)
            {
                for (int y = -brushRadius + 1; y < brushRadius; y++)
                {
                    switch (selectedGrid)
                    {
                        case 0:
                            placeTileInMap(tilemapBack, coordinate + new Vector3Int(x, y, 0), tile);
                            break;

                        case 1:
                            placeTileInMap(decBTilemap, coordinate + new Vector3Int(x, y, 0), tile);
                            break;

                        case 2:
                            placeTileInMap(tilemapCol, coordinate + new Vector3Int(x, y, 0), tile);
                            break;

                        case 3:
                            placeTileInMap(decFTilemap, coordinate + new Vector3Int(x, y, 0), tile);
                            break;
                    }
                }
            }
        }
    }

    public void saveRoom()
    {
        List<string> BackTiles = new List<string>();
        List<string> decBTiles = new List<string>();
        List<string> ColTiles = new List<string>();
        List<string> DecFTiles = new List<string>();
        List<string> EntityName = new List<string>();
        List<Vector2> EntityPos = new List<Vector2>();
        List<EntityUniqueData> EntityData = new List<EntityUniqueData>();

        RoomData roomData = new RoomData();
        roomData.roomSize = roomSize;
        for (int x = 0; x < roomSize; x++)
        {
            for (int y = 0; y < roomSize; y++)
            {
                if (tilemapBack.GetTile(new Vector3Int(x, y, 0)) != null)
                    BackTiles.Add(tilemapBack.GetTile(new Vector3Int(x, y, 0)).name);
                else
                    BackTiles.Add("Unknown");

                if (tilemapCol.GetTile(new Vector3Int(x, y, 0)) != null)
                    ColTiles.Add(tilemapCol.GetTile(new Vector3Int(x, y, 0)).name);
                else
                    ColTiles.Add("Unknown");

                if (decBTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    decBTiles.Add(decBTilemap.GetTile(new Vector3Int(x, y, 0)).name);
                else
                    decBTiles.Add("Unknown");

                if (decFTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    DecFTiles.Add(decFTilemap.GetTile(new Vector3Int(x, y, 0)).name);
                else
                    DecFTiles.Add("Unknown");
            }
        }

        foreach (EntityBase eb in FindObjectsOfType<EntityBase>())
        {
            EntityName.Add(eb.name);
            EntityPos.Add(eb.transform.position);
            EntityData.Add(eb.uq);
        }

        roomData.collisionTiles = ColTiles.ToArray();
        roomData.backgroundTiles = BackTiles.ToArray();
        roomData.decorationBTiles = decBTiles.ToArray();
        roomData.decorationFTiles = DecFTiles.ToArray();

        roomData.roomName = RoomNameField.text;
        roomData.biomeID = int.Parse(BiomeIDField.text);

        roomData.EntityName = EntityName.ToArray();
        roomData.EntityPos = EntityPos.ToArray();
        roomData.entityUniqueDatas = EntityData.ToArray();
        roomData.orientationType = (BoardData.OrientationType)OrientationDropDown.value;
        roomData.roomType = (BoardData.RoomType)RoomTypeDropDown.value;
        saveRoom(roomData);
    }

    public void saveRoom(RoomData roomData)
    {
        if (!Directory.Exists(roomsPath))
            Directory.CreateDirectory(roomsPath);

        using (StreamWriter stream = new StreamWriter(roomsPath + roomData.roomName + ".json"))
        {
            string json = JsonUtility.ToJson(roomData);
            stream.Write(json);
        }
    }

    public void exitRoomMaker()
    {
        SceneManager.LoadScene(0);
    }

    public void loadRoomFromFile()
    {
        for (int x = 0; x < roomSize; x++)
        {
            for (int y = 0; y < roomSize; y++)
            {
                tilemapBack.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.getTileBasefromName(ScenePersistantData.rooms[loadRoomName.value].backgroundTiles[x * roomSize + y]));
                decBTilemap.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.getTileBasefromName(ScenePersistantData.rooms[loadRoomName.value].decorationBTiles[x * roomSize + y]));
                tilemapCol.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.getTileBasefromName(ScenePersistantData.rooms[loadRoomName.value].collisionTiles[x * roomSize + y]));
                decFTilemap.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.getTileBasefromName(ScenePersistantData.rooms[loadRoomName.value].decorationFTiles[x * roomSize + y]));
            }
        }
    }

    public void loadRoomChanged()
    {
        loadRoomFromFile();
        OrientationDropDown.value = (int)ScenePersistantData.rooms[loadRoomName.value].orientationType;
        RoomTypeDropDown.value = (int)ScenePersistantData.rooms[loadRoomName.value].roomType;
        BiomeIDField.text = ScenePersistantData.rooms[loadRoomName.value].biomeID.ToString();
        RoomNameField.text = ScenePersistantData.rooms[loadRoomName.value].roomName;
    }

    public void layerChange()
    {
        selectedGrid = layerChangeDd.value;
    }
}