using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityBase : MonoBehaviour
{
    public Vector2Int OriginCell;
    public Sprite sprite;
    public WorldLoaderManager wlm;
    public int health;
    public string metaText;
    public bool Solid = true;

    public EntityManagmnet.Behaviour behaviour;

    private DreamFragBehaviour dreamFragBehaviour;

    // Start is called before the first frame update
    public void init()
    {
        //Add Behavioural Scripts such as
        //Movement controllers
        //Animation controllers

        SpriteRenderer sr = this.gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        this.gameObject.AddComponent<PolygonCollider2D>();
        sr.sortingOrder = 3;
        if (behaviour == EntityManagmnet.Behaviour.DFrag)
            dreamFragBehaviour = this.gameObject.AddComponent<DreamFragBehaviour>();

        if (Solid)
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
        else
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!onScreen && !wlm.loaded[OriginCell.x, OriginCell.y])
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
    }

    public void Hit(EntityManagmnet.hitType hitType)
    {
        if (behaviour == EntityManagmnet.Behaviour.Bush && hitType == EntityManagmnet.hitType.sword)
            die();
    }

    public void Interact(RPGController player)

    {
        if (behaviour == EntityManagmnet.Behaviour.Sign)
            MessageSystem.message(metaText);
        if (behaviour == EntityManagmnet.Behaviour.Rock || behaviour == EntityManagmnet.Behaviour.Bush)
            player.pickup(this);
    }

    public void die()
    {
        if (wlm.loaded[OriginCell.x, OriginCell.y])
        {
            wlm.killCount[OriginCell.x, OriginCell.y]--;
            if (wlm.killCount[OriginCell.x, OriginCell.y] == 0)
                wlm.map[OriginCell.x, OriginCell.y].cleared = true;
        }
        Destroy(this.gameObject);
        Destroy(this);
    }

    public void collide(RPGController player)
    {
        if (behaviour == EntityManagmnet.Behaviour.DFrag)
        {
            ScenePersistantData.DreamFragments += health;
            MessageSystem.message("Dream Fragments : " + ScenePersistantData.DreamFragments.ToString());
            Destroy(this.gameObject);
            Destroy(this);
        }
    }

    public override string ToString()
    {
        return sprite.name;
    }

    private void OnMouseDown()
    {
        RoomMaker rm = FindObjectOfType<RoomMaker>();
        if (rm != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                rm.deleteEntity(this);
            if (Input.GetKey(KeyCode.LeftControl))
                rm.entityUpdateMeta(this);
        }
    }
}