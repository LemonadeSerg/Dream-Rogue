using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBow : ItemBase
{
    public string projectileName;
    public float speed;
    public float damage;

    public override void Down(WorldLoaderManager wlm)
    {
        EntityUniqueData eud = new EntityUniqueData();
        eud.damage = damage;
        EntityBase eb = wlm.SpawnEntity(ScenePersistantData.getEntityFromName(projectileName), eud, wlm.player.GetComponent<RPGController>().arrowFireSpot.transform.position);
        eb.transform.gameObject.layer = 9;

        if (wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") > 0 && wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") > wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") && wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") > -wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis"))
        {
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        }
        else
        if (wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") < 0 && wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") < wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") && wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") < -wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis"))
        {
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
            eb.transform.Rotate(new Vector3(0, 0, 180));
        }
        else
        if (wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") > 0 && wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") > wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") && wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") > -wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis"))
        {
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            eb.transform.Rotate(new Vector3(0, 0, -90));
        }
        else
        if (wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") < 0 && wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") < wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis") && wlm.player.GetComponent<RPGController>().animator.GetFloat("XAxis") < -wlm.player.GetComponent<RPGController>().animator.GetFloat("YAxis"))
        {
            eb.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
            eb.transform.Rotate(new Vector3(0, 0, 90));
        }
        eb.GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    public override void Up(WorldLoaderManager wlm)
    {
        //throw new System.NotImplementedException();
    }
}