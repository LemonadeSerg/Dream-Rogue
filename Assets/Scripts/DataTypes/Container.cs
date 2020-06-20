using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Container : MonoBehaviour
{
    public string name;
    public int drops;
    public float[] chances;
    public string[] entities;

    public void drop(WorldLoaderManager wlm, Vector2 pos)
    {
        List<string> dropNames = new List<string>();

        for (int i = 0; i < chances.Length; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) < chances[i])
            {
                dropNames.Add(entities[i]);
            }
        }

        for (int i = 0; i <= drops; i++)
        {
            int rand = UnityEngine.Random.Range(0, dropNames.Count);
            wlm.spawnEntity(ScenePersistantData.getEntityFromName(dropNames[rand]), pos, ScenePersistantData.getEntityFromName(dropNames[rand]).metaText, ScenePersistantData.getEntityFromName(dropNames[rand]).health, ScenePersistantData.getEntityFromName(dropNames[rand]).Pushable, ScenePersistantData.getEntityFromName(dropNames[rand]).Solid);
        }
    }
}