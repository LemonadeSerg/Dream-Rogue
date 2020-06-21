using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBow : ItemBase
{
    public string projectile;
    public int damage;
    public float speed;

    public override void clickItem(WorldLoaderManager wlm)
    {
        RPGController player = wlm.player.GetComponent<RPGController>();
        EntityBase eb = wlm.spawnEntity(ScenePersistantData.getEntityFromName(projectile), player.arrowFireSpot.transform.position, "", damage, false, true);

        if (player.animator.GetFloat("XAxis") > 0 && player.animator.GetFloat("XAxis") > player.animator.GetFloat("YAxis") && player.animator.GetFloat("XAxis") > -player.animator.GetFloat("YAxis"))
        {
            eb.transform.Rotate(new Vector3(0, 0, -90));
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
        }
        else
        if (player.animator.GetFloat("XAxis") < 0 && player.animator.GetFloat("XAxis") < player.animator.GetFloat("YAxis") && player.animator.GetFloat("XAxis") < -player.animator.GetFloat("YAxis"))
        {
            eb.transform.Rotate(new Vector3(0, 0, 90));
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
        }
        else
        if (player.animator.GetFloat("YAxis") > 0 && player.animator.GetFloat("YAxis") > player.animator.GetFloat("XAxis") && player.animator.GetFloat("YAxis") > -player.animator.GetFloat("XAxis"))
        {
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        }
        else
        if (player.animator.GetFloat("YAxis") < 0 && player.animator.GetFloat("YAxis") < player.animator.GetFloat("XAxis") && player.animator.GetFloat("YAxis") < -player.animator.GetFloat("XAxis"))
        {
            eb.transform.rotation = new Quaternion(0, 0, 180, 0);
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
        }
    }

    public override void holdItem(WorldLoaderManager wlm)
    {
        throw new System.NotImplementedException();
    }
}