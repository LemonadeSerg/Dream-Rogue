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
            decFTilemap.SetTile(new Vector3Int(i % 10, (-i / 10) - 2, 0), ScenePersistantData.tileBases.ToArray()[i]);
        }

        List<BoardData.OrientationType> orTypes = new List<BoardData.OrientationType>();
        List<string> orTypeName = new List<string>();
        for (int i = 0; i <= 14; i++)
        {
            orTypes.Add((BoardData.OrientationType)i);
            orTypeName.Add(orTypes[i].ToString());
        }

        OrientationDropDown.AddOptions(orTypeName);

        List<BoardData.RoomType> rmTypes = new List<BoardData.RoomType>();
        List<string> rmTypeName = new List<string>();
        for (int i = 0; i <= 5; i++)
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
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            LeftClick();
        }
        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            LeftHold();
        }
        if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift))
        {
            RightHold();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentSelection = 0;
            editingEntities = !editingEntities;
        }
        if (Input.mouseScrollDelta.y > 0.1f)
        {
            currentSelection++;
            if (!editingEntities)
                if (currentSelection >= ScenePersistantData.tileBases.ToArray().Length)
                {
                    currentSelection = 0;
                }
                else if (editingEntities)
                    if (currentSelection >= ScenePersistantData.entities.ToArray().Length)
                    {
                        currentSelection = 0;
                    }
        }
        if (Input.mouseScrollDelta.y < -0.1f)
        {
            currentSelection--;
            if (!editingEntities)
                if (currentSelection <= 0)
                {
                    currentSelection = ScenePersistantData.tileBases.ToArray().Length - 1;
                }
                else if (editingEntities)
                    if (currentSelection <= 0)
                    {
                        currentSelection = ScenePersistantData.entities.ToArray().Length - 1;
                    }
        }
        if (!editingEntities)
        {
            if (currentSelection > 0)
            {
                selectorTop.sprite = ScenePersistantData.tileSprites.ToArray()[currentSelection - 1];
                selectorTop.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) + Vector3.up;
            }
            else
            {
                selectorTop.transform.position = new Vector3(-999, -999);
            }
            selectorMid.sprite = ScenePersistantData.tileSprites.ToArray()[currentSelection];
            selectorMid.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            if (currentSelection < ScenePersistantData.tileSprites.ToArray().Length - 1)
            {
                selectorBot.sprite = ScenePersistantData.tileSprites.ToArray()[currentSelection + 1];
                selectorBot.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) + Vector3.down;
            }
            else
            {
                selectorBot.transform.position = new Vector3(-999, -999);
            }
        }
        else
        {
            if (currentSelection > 0)
            {
                selectorTop.sprite = ScenePersistantData.entities.ToArray()[currentSelection - 1].sprite;
                selectorTop.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) + Vector3.up;
            }
            else
            {
                selectorTop.transform.position = new Vector3(-999, -999);
            }
            selectorMid.sprite = ScenePersistantData.entities.ToArray()[currentSelection].sprite;
            selectorMid.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            if (currentSelection < ScenePersistantData.entities.ToArray().Length - 1)
            {
                selectorBot.sprite = ScenePersistantData.entities.ToArray()[currentSelection + 1].sprite;
                selectorBot.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) + Vector3.down;
            }
            else
            {
                selectorBot.transform.position = new Vector3(-999, -999);
            }
        }
    }

    private void changeTile()
    {
        if (currentSelection < ScenePersistantData.tileBases.ToArray().Length - 1)
        {
            currentSelection++;
        }
        else
        {
            currentSelection = 0;
        }
    }

    private void LeftClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = gridBack.WorldToCell(mouseWorldPos);
        if (editingEntities)
            spawnEntity(mouseWorldPos);
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
        eb.uq = new EntityUniqueData();
        eb.uq.health = ScenePersistantData.entities.ToArray()[currentSelection].uq.health;
        eb.uq.speed = ScenePersistantData.entities.ToArray()[currentSelection].uq.speed;
        eb.uq.power = ScenePersistantData.entities.ToArray()[currentSelection].uq.power;
        eb.uq.message = ScenePersistantData.entities.ToArray()[currentSelection].uq.message;
        eb.uq.bombSpawn = ScenePersistantData.entities.ToArray()[currentSelection].uq.bombSpawn;
        eb.uq.damage = ScenePersistantData.entities.ToArray()[currentSelection].uq.damage;
        eb.uq.switchCode = ScenePersistantData.entities.ToArray()[currentSelection].uq.switchCode;
        eb.uq.switchMode = ScenePersistantData.entities.ToArray()[currentSelection].uq.switchMode;
        eb.init();
    }

    private void LeftHold()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = gridBack.WorldToCell(mouseWorldPos);
        if (!editingEntities)
        {
            if (selectedGrid == 0)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    tilemapBack.SetTile(coordinate, ScenePersistantData.tileBases.ToArray()[currentSelection]);
            }
            if (selectedGrid == 1)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    decBTilemap.SetTile(coordinate, ScenePersistantData.tileBases.ToArray()[currentSelection]);
            }
            if (selectedGrid == 2)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    tilemapCol.SetTile(coordinate, ScenePersistantData.tileBases.ToArray()[currentSelection]);
            }
            if (selectedGrid == 3)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    decFTilemap.SetTile(coordinate, ScenePersistantData.tileBases.ToArray()[currentSelection]);
            }
            if (coordinate.y < 0)
                currentSelection = ScenePersistantData.tileIndexFromName(decFTilemap.GetTile(coordinate).name);
        }
    }

    private void RightHold()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = gridBack.WorldToCell(mouseWorldPos);
        if (!editingEntities)
        {
            if (selectedGrid == 0)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    tilemapBack.SetTile(coordinate, null);
            }
            if (selectedGrid == 1)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    decBTilemap.SetTile(coordinate, null);
            }
            if (selectedGrid == 2)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    tilemapCol.SetTile(coordinate, null);
            }
            if (selectedGrid == 3)
            {
                if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                    decFTilemap.SetTile(coordinate, null);
            }
            if (coordinate.y < 0)
                for (int x = 0; x < roomSize; x++)
                {
                    for (int y = 0; y < roomSize; y++)
                    {
                        currentSelection = ScenePersistantData.tileIndexFromName(decFTilemap.GetTile(coordinate).name);

                        if (selectedGrid == 0)
                        {
                            tilemapBack.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
                        }
                        if (selectedGrid == 1)
                        {
                            decBTilemap.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
                        }
                        if (selectedGrid == 2)
                        {
                            tilemapCol.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
                        }
                        if (selectedGrid == 3)
                        {
                            decFTilemap.SetTile(new Vector3Int(x, y, 0), ScenePersistantData.tileBases.ToArray()[currentSelection]);
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