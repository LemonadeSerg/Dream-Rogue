using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ScenePersistantData
{
    public static string worldName;
    public static List<TileBase> tileBases;
    public static List<Sprite> tileSprites;

    public static void addTile(Sprite sprite)
    {
        CustomTileBase tile = (CustomTileBase)ScriptableObject.CreateInstance(typeof(CustomTileBase));
        tile.sprite = sprite;
        tileBases.Add(tile);
        tileSprites.Add(tile.sprite);
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