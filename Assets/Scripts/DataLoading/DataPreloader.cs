using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataPreloader : MonoBehaviour
{
    public TileBase[] tileBases;
    public Sprite[] tileSprites;
    public EntityBase[] entities;
    public RoomPreload[] rooms;

    // Start is called before the first frame update
    private void Start()
    {
        ScenePersistantData.tileBases = new List<TileBase>();
        ScenePersistantData.tileSprites = new List<Sprite>();
        ScenePersistantData.rooms = new List<RoomData>();
        ScenePersistantData.entities = new List<EntityBase>();

        for (int i = 0; i < tileBases.Length; i++)
        {
            ScenePersistantData.tileBases.Add(tileBases[i]);
            ScenePersistantData.tileSprites.Add(tileSprites[i]);
        }

        for (int i = 0; i < entities.Length; i++)
        {
            ScenePersistantData.entities.Add(entities[i]);
        }
        for (int i = 0; i < rooms.Length; i++)
        {
            RoomData rm = new RoomData();
            rm.roomName = rooms[i].roomName;
            rm.decorationBTiles = rooms[i].decorationBTiles;
            rm.decorationFTiles = rooms[i].decorationFTiles;
            rm.backgroundTiles = rooms[i].backgroundTiles;
            rm.collisionTiles = rooms[i].collisionTiles;
            rm.roomSize = rooms[i].roomSize;
            rm.biomeID = rooms[i].biomeID;
            rm.roomType = rooms[i].roomType;
            rm.orientationType = rooms[i].orientationType;
            rm.EntityName = rooms[i].EntityName;
            rm.EntityPos = rooms[i].EntityPos;
            rm.entityUniqueDatas = rooms[i].entityUniqueDatas;
            ScenePersistantData.rooms.Add(rm);
        }
        new LoadExternalTiles().loadTiles();
        new LoadExternalRooms().loadRooms();

        for (int i = 0; i < ScenePersistantData.rooms.Count; i++)
        {
            GameObject gm = new GameObject(ScenePersistantData.rooms[i].roomName);
            RoomPreload rp = gm.AddComponent<RoomPreload>();
            rp.roomName = ScenePersistantData.rooms[i].roomName;
            rp.decorationBTiles = ScenePersistantData.rooms[i].decorationBTiles;
            rp.decorationFTiles = ScenePersistantData.rooms[i].decorationFTiles;
            rp.backgroundTiles = ScenePersistantData.rooms[i].backgroundTiles;
            rp.collisionTiles = ScenePersistantData.rooms[i].collisionTiles;
            rp.roomSize = ScenePersistantData.rooms[i].roomSize;
            rp.biomeID = ScenePersistantData.rooms[i].biomeID;
            rp.roomType = ScenePersistantData.rooms[i].roomType;
            rp.orientationType = ScenePersistantData.rooms[i].orientationType;
            rp.EntityName = ScenePersistantData.rooms[i].EntityName;
            rp.EntityPos = ScenePersistantData.rooms[i].EntityPos;
            rp.entityUniqueDatas = ScenePersistantData.rooms[i].entityUniqueDatas;
        }
    }
}