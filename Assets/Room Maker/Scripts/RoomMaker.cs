using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RoomMaker : MonoBehaviour
{
    public Tilemap tilemapBack;
    public Grid gridBack;
    public Tilemap tilemapCol;
    public Grid gridBackCol;
    public TileBase[] tileBases;
    public int currentTile = 1;
    public int selectedGrid = 1;

    public int roomSize = 20;
    public Sprite[] tileTexture;

    public SpriteRenderer selectorTop, selectorMid, selectorBot;
    public LoadRoomFiles loadRoom;

    public Dropdown OrientationDropDown;
    public Dropdown RoomTypeDropDown;
    public InputField BiomeIDField;
    public InputField RoomNameField;
    public Dropdown loadRoomName;

    public string roomsPath;

    public RoomData roomDataLoading;
    // Start is called before the first frame update

    private void Start()
    {
        for (int x = 0; x < roomSize; x++)
        {
            for (int y = 0; y < roomSize; y++)
            {
                tilemapBack.SetTile(new Vector3Int(x, y, 0), null);
                tilemapCol.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        for (int i = 0; i < tileBases.Length; i++)
        {
            if (i < roomSize)
            {
                tilemapBack.SetTile(new Vector3Int(i, -2, 0), tileBases[i]);
            }
            else if (i < roomSize * 2)
            {
                tilemapBack.SetTile(new Vector3Int(i - roomSize, -3, 0), tileBases[i]);
            }
            else if (i < roomSize * 3)
            {
                tilemapBack.SetTile(new Vector3Int(i - roomSize * 2, -4, 0), tileBases[i]);
            }
            else if (i < roomSize * 4)
            {
                tilemapBack.SetTile(new Vector3Int(i - roomSize * 3, -5, 0), tileBases[i]);
            }
            else if (i < roomSize * 4)
            {
                tilemapBack.SetTile(new Vector3Int(i - roomSize * 4, -6, 0), tileBases[i]);
            }
        }

        loadRoom = new LoadRoomFiles();
        loadRoom.loadRooms();
        roomsPath = Application.dataPath + "/Rooms/";
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
        for (int i = 0; i < loadRoom.rooms.Count; i++)
        {
            roomNames.Add(loadRoom.rooms[i].roomName);
        }
        loadRoomName.AddOptions(roomNames);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LeftClick();
        }
        if (Input.GetMouseButton(1))
        {
            RightClick();
        }
        if (Input.mouseScrollDelta.y > 0.1f)
        {
            currentTile++;
            if (currentTile >= tileBases.Length)
            {
                currentTile = 0;
            }
        }
        if (Input.mouseScrollDelta.y < -0.1f)
        {
            currentTile--;
            if (currentTile <= 0)
            {
                currentTile = tileBases.Length - 1;
            }
        }
        if (currentTile > 0)
        {
            selectorTop.sprite = tileTexture[currentTile - 1];
            selectorTop.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) + Vector3.up;
        }
        selectorMid.sprite = tileTexture[currentTile];
        selectorMid.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        if (currentTile < tileTexture.Length - 1)
        {
            selectorBot.sprite = tileTexture[currentTile + 1];
            selectorBot.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) + Vector3.down;
        }
    }

    private void OnGUI()
    {
    }

    private void changeTile()
    {
        if (currentTile < tileBases.Length - 1)
        {
            currentTile++;
        }
        else
        {
            currentTile = 0;
        }
    }

    private void LeftClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = gridBackCol.WorldToCell(mouseWorldPos);
        if (selectedGrid == 1)
        {
            if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                tilemapBack.SetTile(coordinate, tileBases[currentTile]);
        }
        if (selectedGrid == 0)
        {
            if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                tilemapCol.SetTile(coordinate, tileBases[currentTile]);
        }

        if (coordinate.y < 0)
            currentTile = tileIndexFromName(tilemapBack.GetTile(coordinate).name);
    }

    private void RightClick()
    {
        if (selectedGrid == 1)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = gridBack.WorldToCell(mouseWorldPos);
            if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                tilemapBack.SetTile(coordinate, null);
        }
        if (selectedGrid == 0)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = gridBackCol.WorldToCell(mouseWorldPos);
            if (coordinate.x >= 0 && coordinate.x < roomSize && coordinate.y >= 0 && coordinate.y < roomSize)
                tilemapCol.SetTile(coordinate, null);
        }
    }

    private int tileIndexFromName(string name)
    {
        for (int i = 0; i < tileBases.Length; i++)
        {
            if (name == tileBases[i].name)
                return i;
        }
        return 0;
    }

    public void saveRoom()
    {
        List<string> BackTiles = new List<string>();
        List<string> ColTiles = new List<string>();
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
            }
        }
        roomData.collisionTiles = ColTiles.ToArray();
        roomData.backgroundTiles = BackTiles.ToArray();
        roomData.roomName = RoomNameField.text;
        roomData.biomeID = int.Parse(BiomeIDField.text);

        roomData.orientationType = (BoardData.OrientationType)OrientationDropDown.value;
        roomData.roomType = (BoardData.RoomType)RoomTypeDropDown.value;
        saveRoom(roomData);

        loadRoom.loadRooms();
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

    public UnityEngine.Tilemaps.TileBase getTileBasefromName(string name)
    {
        foreach (UnityEngine.Tilemaps.TileBase tb in tileBases)
        {
            if (tb.name == name)
                return tb;
        }

        return null;
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
                tilemapBack.SetTile(new Vector3Int(x, y, 0), getTileBasefromName(loadRoom.rooms[loadRoomName.value].backgroundTiles[x * roomSize + y]));
            }
        }
    }

    public void loadRoomChanged()
    {
        loadRoomFromFile();
        OrientationDropDown.value = (int)loadRoom.rooms[loadRoomName.value].orientationType;
        RoomTypeDropDown.value = (int)loadRoom.rooms[loadRoomName.value].roomType;
        BiomeIDField.text = loadRoom.rooms[loadRoomName.value].biomeID.ToString();
        RoomNameField.text = loadRoom.rooms[loadRoomName.value].roomName;
    }
}