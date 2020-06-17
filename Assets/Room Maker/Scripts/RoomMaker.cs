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

    public bool entities = false;

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

    public string roomsPath;

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
        if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
        if (Input.GetMouseButton(0))
        {
            LeftHold();
        }
        if (Input.GetMouseButton(1))
        {
            RightClick();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            entities = !entities;
            currentSelection = 0;
        }
        if (Input.mouseScrollDelta.y > 0.1f)
        {
            currentSelection++;
            if (entities && currentSelection >= ScenePersistantData.entities.ToArray().Length)
            {
                currentSelection = 0;
            }
            if (!entities && currentSelection >= ScenePersistantData.tileBases.ToArray().Length)
            {
                currentSelection = 0;
            }
        }
        if (Input.mouseScrollDelta.y < -0.1f)
        {
            currentSelection--;
            if (!entities && currentSelection <= 0)
            {
                currentSelection = ScenePersistantData.tileBases.ToArray().Length - 1;
            }
            if (entities && currentSelection <= 0)
            {
                currentSelection = ScenePersistantData.entities.ToArray().Length - 1;
            }
        }
        if (!entities)
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
        if (entities)
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
        if (entities)
        {
            GameObject go = new GameObject(ScenePersistantData.entities[currentSelection].name);
            EntityBase eb = go.AddComponent<EntityBase>();
            eb.sprite = ScenePersistantData.entities[currentSelection].sprite;
            eb.behaviour = ScenePersistantData.entities[currentSelection].behaviour;
            eb.init();
            go.transform.position = coordinate;
        }
    }

    private void LeftHold()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = gridBack.WorldToCell(mouseWorldPos);
        if (!entities)
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

    private void RightClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = gridBack.WorldToCell(mouseWorldPos);
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

    public void saveRoom()
    {
        List<string> BackTiles = new List<string>();
        List<string> decBTiles = new List<string>();
        List<string> ColTiles = new List<string>();
        List<string> DecFTiles = new List<string>();
        List<string> EntityName = new List<string>();
        List<Vector2> EntityPos = new List<Vector2>();
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
        }

        roomData.collisionTiles = ColTiles.ToArray();
        roomData.backgroundTiles = BackTiles.ToArray();
        roomData.decorationBTiles = decBTiles.ToArray();
        roomData.decorationFTiles = DecFTiles.ToArray();

        roomData.entityName = EntityName.ToArray();
        roomData.entityPos = EntityPos.ToArray();

        roomData.roomName = RoomNameField.text;
        roomData.biomeID = int.Parse(BiomeIDField.text);

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