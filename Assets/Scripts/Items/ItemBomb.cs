using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBomb : ItemBase
{
    public int fuse = 5;
    public float strength = 1;

    public override void clickItem(WorldLoaderManager wlm)
    {
        wlm.player.GetComponent<RPGController>().spawnEntityinHand(wlm.spawnEntity(ScenePersistantData.getEntityFromName("Bomb"), wlm.player.transform.position, strength.ToString(), fuse, true, true));
    }

    public override void holdItem(WorldLoaderManager wlm)
    {
        throw new System.NotImplementedException();
    }
}