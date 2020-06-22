using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadContainers
{
    public string WorldPath = Application.dataPath + "/Containers/";

    public void loadContainers()
    {
        if (!Directory.Exists(WorldPath))
            Directory.CreateDirectory(WorldPath);

        foreach (string file in System.IO.Directory.GetFiles(WorldPath))
        {
            if (file.EndsWith(".json"))
            {
                LoadJson(file);
            }
        }
    }

    private void LoadJson(string fileName)
    {
        using (System.IO.StreamReader stream = new System.IO.StreamReader(fileName))
        {
            string json = stream.ReadToEnd();
            //ScenePersistantData.containers.Add(JsonUtility.FromJson<Container>(json));
        }
    }
}