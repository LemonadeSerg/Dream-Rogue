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
    public bool Pushable = false;

    public EntityManagmnet.Behaviour behaviour;

    private DreamFragBehaviour dreamFragBehaviour;
    private BombBehaviour bombBehaviour;
    private ExplosionBehaviour explosionBehaviour;
    private ArrowBehaviour arrowBehaviour;
    private SwitchBehaviour switchBehaviour;

    // Start is called before the first frame update
    public void init()
    {
        //Add Behavioural Scripts such as
        //Movement controllers
        //Animation controllers

        SpriteRenderer sr = this.gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 3;
        this.gameObject.AddComponent<PolygonCollider2D>();

        if (Solid)
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
        else
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;

        if (behaviour == EntityManagmnet.Behaviour.DFrag)
        {
            dreamFragBehaviour = this.gameObject.AddComponent<DreamFragBehaviour>();
        }
        if (behaviour == EntityManagmnet.Behaviour.Bomb)
        {
            bombBehaviour = this.gameObject.AddComponent<BombBehaviour>();
            bombBehaviour.timer = health;
            bombBehaviour.power = float.Parse(metaText);
        }
        if (behaviour == EntityManagmnet.Behaviour.Explosion)
        {
            explosionBehaviour = this.gameObject.AddComponent<ExplosionBehaviour>();
            explosionBehaviour.power = float.Parse(metaText);
        }

        if (behaviour == EntityManagmnet.Behaviour.Arrow)
        {
            arrowBehaviour = this.gameObject.AddComponent<ArrowBehaviour>();
            arrowBehaviour.damage = health;
        }

        if (behaviour == EntityManagmnet.Behaviour.Switch)
        {
            switchBehaviour = this.gameObject.AddComponent<SwitchBehaviour>();
            switchBehaviour.mode = health;
            switchBehaviour.connectionString = metaText;
            switchBehaviour.init();
        }
        Rigidbody2D rb2d;

        if (Pushable)
        {
            rb2d = this.gameObject.AddComponent<Rigidbody2D>();
            rb2d.gravityScale = 0;
            rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb2d.freezeRotation = true;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.drag = 10f;
            rb2d.mass = 20f;
        }

        if (behaviour == EntityManagmnet.Behaviour.Explosion || behaviour == EntityManagmnet.Behaviour.Sign || behaviour == EntityManagmnet.Behaviour.Bush)
        {
            rb2d = this.gameObject.AddComponent<Rigidbody2D>();
            rb2d.gravityScale = 0;
            rb2d.freezeRotation = true;
            rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            if (behaviour == EntityManagmnet.Behaviour.Explosion)
            {
                rb2d.useFullKinematicContacts = true;
            }
        }
        if (behaviour == EntityManagmnet.Behaviour.Arrow)
        {
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
            rb2d = this.gameObject.AddComponent<Rigidbody2D>();
            rb2d.useFullKinematicContacts = true;
            rb2d.drag = 1f;
            rb2d.gravityScale = 0;
            rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
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
        print(this.name + " hit by " + hitType.ToString());
        if (behaviour == EntityManagmnet.Behaviour.Bush)
        {
            Container con = ScenePersistantData.GetContainerFromName(metaText);
            con.drop(wlm, this.transform.position);
            die();
        }
        if (behaviour == EntityManagmnet.Behaviour.Switch)
        {
            switchBehaviour.hit();
        }
    }

    public void Interact(RPGController player)

    {
        if (behaviour == EntityManagmnet.Behaviour.Sign)
            MessageSystem.message(metaText);
        if (behaviour == EntityManagmnet.Behaviour.Rock || behaviour == EntityManagmnet.Behaviour.Bush || behaviour == EntityManagmnet.Behaviour.Bomb)
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

    public void activate()
    {
        print(this.name + " Entity has been activated");
    }

    public void deactivate()
    {
        print(this.name + " Entity has been dectivated");
    }

    public void collide(RPGController player)
    {
        if (behaviour == EntityManagmnet.Behaviour.DFrag)
        {
            ScenePersistantData.DreamFragments += health;
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