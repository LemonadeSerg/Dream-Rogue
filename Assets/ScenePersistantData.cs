using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ScenePersistantData
{
    public static string worldName;
    public static List<TileBase> tileBases;
    public static List<Sprite> tileSprites;
    public static List<EntityBase> entities;
    public static List<RoomData> rooms;
    public static bool paused;
    public static int DreamFragments;

    public static void addTile(Sprite sprite)
    {
        CustomTileBase tile = (CustomTileBase)ScriptableObject.CreateInstance(typeof(CustomTileBase));
        tile.sprite = sprite;
        tileBases.Add(tile);
        tileSprites.Add(tile.sprite);
    }

    public static int tileIndexFromName(string name)
    {
        for (int i = 0; i < tileBases.ToArray().Length; i++)
        {
            if (name == tileBases.ToArray()[i].name)
                return i;
        }
        return 0;
    }

    public static TileBase getTileBasefromName(string name)
    {
        foreach (TileBase tb in tileBases.ToArray())
        {
            if (tb.name == name)
                return tb;
        }

        return null;
    }

    public static EntityBase getEntityFromName(string name)
    {
        foreach (EntityBase tb in entities.ToArray())
        {
            if (tb.name == name)
                return tb;
        }

        return entities[0];
    }
}

public class CustomTileBase : TileBase
{
    public Sprite sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }
}