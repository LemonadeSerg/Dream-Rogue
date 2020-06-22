using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataPreloader : MonoBehaviour
{
    public TileBase[] tileBases;
    public Sprite[] tileSprites;
    public EntityBase[] entities;

    // Start is called before the first frame update
    private void Start()
    {
        ScenePersistantData.tileBases = new List<TileBase>();
        ScenePersistantData.tileSprites = new List<Sprite>();
        ScenePersistantData.rooms = new List<RoomData>();
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
        new LoadExternalTiles().loadTiles();
        new LoadExternalRooms().loadRooms();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}