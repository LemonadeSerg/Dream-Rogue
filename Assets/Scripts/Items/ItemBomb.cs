using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBomb : ItemBase
{
    public string bombName;
    public string bombSpawn;
    public float power;
    public float fuse;

    public override void Down(WorldLoaderManager wlm)

    {
        EntityUniqueData eud = new EntityUniqueData();
        eud.bombSpawn = bombSpawn;
        eud.power = power;
        eud.fuse = fuse;
        wlm.player.GetComponent<RPGController>().spawnToHand(wlm.SpawnEntity(ScenePersistantData.getEntityFromName(bombName), eud, wlm.player.GetComponent<RPGController>().holdPos.transform.position));
    }

    public override void Up(WorldLoaderManager wlm)
    {
        throw new System.NotImplementedException();
    }
}