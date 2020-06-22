using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ItemSword : ItemBase
{
    public float damage;
    public float range;
    public float width;

    public float swordHitForce;
    public ContactFilter2D contact;

    public override void Down(WorldLoaderManager wlm)
    {
        if (wlm.player.GetComponent<RPGController>().arrowFireSpot.transform.localPosition.x == 0)
        {
            if (wlm.player.GetComponent<RPGController>().arrowFireSpot.transform.position.y > wlm.player.transform.position.y)
            {
                RaycastHit2D[] hits = new RaycastHit2D[10];
                int c = Physics2D.Raycast(new Vector2(wlm.player.transform.position.x + (width / 2), wlm.player.transform.position.y + range), Vector2.left, contact, hits, width);
                for (int i = 0; i < c; i++)
                {
                    if (hits[i].collider.GetComponent<EntityBase>() != null)
                    {
                        hits[i].collider.GetComponent<EntityBase>().Hit(EntityBase.hitType.Sword);
                        hits[i].collider.GetComponent<Rigidbody2D>().AddForce((hits[i].collider.transform.position - new Vector3(wlm.player.transform.position.x + (width / 2), wlm.player.transform.position.y + range)).normalized * swordHitForce);
                    }
                }
            }
            else
            if (wlm.player.GetComponent<RPGController>().arrowFireSpot.transform.position.y < wlm.player.transform.position.y)
            {
                RaycastHit2D[] hits = new RaycastHit2D[10];
                int c = Physics2D.Raycast(new Vector2(wlm.player.transform.position.x - (width / 2), wlm.player.transform.position.y - range), Vector2.right, contact, hits, width);
                for (int i = 0; i < c; i++)
                {
                    if (hits[i].collider.GetComponent<EntityBase>() != null)
                    {
                        hits[i].collider.GetComponent<EntityBase>().Hit(EntityBase.hitType.Sword);
                        hits[i].collider.GetComponent<Rigidbody2D>().AddForce((hits[i].collider.transform.position - new Vector3(wlm.player.transform.position.x - (width / 2), wlm.player.transform.position.y - range)).normalized * swordHitForce);
                    }
                }
            }
        }
        else if (wlm.player.GetComponent<RPGController>().arrowFireSpot.transform.position.x > wlm.player.transform.position.x)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int c = Physics2D.Raycast(new Vector2(wlm.player.transform.position.x + range, wlm.player.transform.position.y - (width / 2)), Vector2.up, contact, hits, width);
            for (int i = 0; i < c; i++)
            {
                if (hits[i].collider.GetComponent<EntityBase>() != null)
                {
                    hits[i].collider.GetComponent<EntityBase>().Hit(EntityBase.hitType.Sword);
                    hits[i].collider.GetComponent<Rigidbody2D>().AddForce((hits[i].collider.transform.position - new Vector3(wlm.player.transform.position.x + range, wlm.player.transform.position.y - (width / 2))).normalized * swordHitForce);
                }
            }
        }
        else if (wlm.player.GetComponent<RPGController>().arrowFireSpot.transform.position.x < wlm.player.transform.position.x)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int c = Physics2D.Raycast(new Vector2(wlm.player.transform.position.x - range, wlm.player.transform.position.y + (width / 2)), Vector2.down, contact, hits, width);
            for (int i = 0; i < c; i++)
            {
                if (hits[i].collider.GetComponent<EntityBase>() != null)
                {
                    hits[i].collider.GetComponent<EntityBase>().Hit(EntityBase.hitType.Sword);
                    hits[i].collider.GetComponent<Rigidbody2D>().AddForce((hits[i].collider.transform.position - new Vector3(wlm.player.transform.position.x - range, wlm.player.transform.position.y + (width / 2))).normalized * swordHitForce);
                }
            }
        }
    }

    public override void Up(WorldLoaderManager wlm)
    {
    }
}