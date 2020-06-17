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

    public EntityManagmnet.Behaviour behaviour;

    // Start is called before the first frame update
    public void init()
    {
        SpriteRenderer sr = this.gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        this.gameObject.AddComponent<PolygonCollider2D>();
        sr.sortingLayerID = 3;
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
        die();
    }

    public void Interact()
    {
        print(behaviour);
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

    private string ToString()
    {
        return sprite.name;
    }
}