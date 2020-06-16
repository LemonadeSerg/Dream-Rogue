using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadExternalTiles
{
    public string WorldPath = Application.dataPath + "/Tiles/";
    public List<Sprite> sprites;

    public void loadTiles()
    {
        if (!Directory.Exists(WorldPath))
            Directory.CreateDirectory(WorldPath);
        sprites = new List<Sprite>();

        foreach (string file in System.IO.Directory.GetFiles(WorldPath))
        {
            if (file.EndsWith(".png"))
            {
                Texture2D tex = LoadPNG(file, file.Replace(".png", ""));
                ScenePersistantData.addTile(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 32f));
            }
        }
    }

    public static Texture2D LoadPNG(string filePath, string spriteName)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(1, 1);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        tex.name = spriteName;
        return tex;
    }
}