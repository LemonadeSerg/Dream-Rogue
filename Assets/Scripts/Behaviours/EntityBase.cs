using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public WorldLoaderManager wlm;

    public SpriteRenderer sr;
    private Rigidbody2D rb;
    private PolygonCollider2D pc;

    public Sprite sprite;
    public bool solid;
    public bool pushable;
    public bool pickable;
    public bool staticObj;
    public bool removeOff;

    private BehaviourBase hitBehaviour;
    private BehaviourBase activateBehaviour;
    private BehaviourBase updateBehaviour;
    private BehaviourBase interactBehaviour;

    public EntityUniqueData uq;

    public Vector2 originCell;

    public bool onScreen = false;

    public enum hitType
    {
        None,
        Hand,
        Arrow,
        Bomb,
        Explosion,
        Sword,
    }

    public enum hitBehaviours
    {
        None,
        Switch,
        Bush,
        Blob,
    }

    public hitBehaviours hitB;

    public enum activateBehaviours
    {
        None,
        Bomb,
    }

    public activateBehaviours actB;

    public enum updateBehaviours
    {
        None,
        Bomb,
        Explosion,
        Arrow,
        Switch,
        Blob,
    }

    public updateBehaviours updB;

    public enum interactBehaviours
    {
        None,
        Sign,
    }

    public interactBehaviours intB;

    private void Start()
    {
    }

    public void init()
    {
        if (wlm == null)
            wlm = FindObjectOfType<WorldLoaderManager>();
        sr = this.gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 2;
        pc = this.gameObject.AddComponent<PolygonCollider2D>();
        pc.isTrigger = !solid;

        rb = this.gameObject.AddComponent<Rigidbody2D>();
        if (staticObj)
            rb.bodyType = RigidbodyType2D.Static;
        rb.drag = 20;
        rb.angularDrag = 10f;

        if (wlm != null)
            originCell = wlm.GetBoardAtVector(this.transform.position.x, this.transform.position.y);

        switch (hitB)
        {
            case hitBehaviours.Switch:
                hitBehaviour = this.gameObject.AddComponent<SwitchBehaviour>();
                hitBehaviour.entity = this;
                hitBehaviour.init();
                break;

            case hitBehaviours.Bush:
                hitBehaviour = this.gameObject.AddComponent<BushBehaviour>();
                hitBehaviour.entity = this;
                break;

            case hitBehaviours.Blob:
                hitBehaviour = this.gameObject.AddComponent<BlobBehaviour>();
                hitBehaviour.entity = this;
                hitBehaviour.init();
                break;

            default:
                break;
        }

        switch (intB)
        {
            case interactBehaviours.Sign:
                interactBehaviour = this.gameObject.AddComponent<SignBehaviour>();
                interactBehaviour.entity = this;

                break;

            default:
                break;
        }
        switch (updB)
        {
            case updateBehaviours.None:
                break;

            case updateBehaviours.Bomb:
                if (this.gameObject.GetComponent<BombBehaviour>() == null)
                    updateBehaviour = this.gameObject.AddComponent<BombBehaviour>();
                else
                    updateBehaviour = this.gameObject.GetComponent<BombBehaviour>();
                break;

            case updateBehaviours.Explosion:
                updateBehaviour = this.gameObject.AddComponent<ExplosionBehaviour>();
                updateBehaviour.entity = this;
                rb.isKinematic = true;
                rb.useFullKinematicContacts = true;
                break;

            case updateBehaviours.Arrow:
                updateBehaviour = this.gameObject.AddComponent<ArrowBehaviour>();
                updateBehaviour.entity = this;
                rb.isKinematic = true;
                rb.useFullKinematicContacts = true;
                break;

            case updateBehaviours.Switch:
                if (this.gameObject.GetComponent<SwitchBehaviour>() == null)
                    updateBehaviour = this.gameObject.AddComponent<SwitchBehaviour>();
                else
                    updateBehaviour = this.gameObject.GetComponent<SwitchBehaviour>();
                break;

            case updateBehaviours.Blob:
                if (this.gameObject.GetComponent<BlobBehaviour>() == null)
                    updateBehaviour = this.gameObject.AddComponent<BlobBehaviour>();
                else
                    updateBehaviour = this.gameObject.GetComponent<BlobBehaviour>();
                break;

            default:
                break;
        }
        switch (actB)
        {
            case activateBehaviours.Bomb:
                if (this.gameObject.GetComponent<BombBehaviour>() == null)
                {
                    activateBehaviour = this.gameObject.AddComponent<BombBehaviour>();
                    activateBehaviour.init();
                }
                else
                    activateBehaviour = this.gameObject.GetComponent<BombBehaviour>();
                activateBehaviour.entity = this;
                break;

            case activateBehaviours.None:
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (wlm != null)
            if (!wlm.loaded[(int)originCell.x, (int)originCell.y])
            {
                Destroy(this.gameObject);
            }
    }

    public void Hit(hitType hitType)
    {
        if (onScreen)
        {
            print(this.name + " hit by type: " + hitType.ToString());
            if (hitBehaviour != null)
            {
                hitBehaviour.Hit(hitType);
            }
        }
    }

    public void Interact()
    {
        if (onScreen)
        {
            if (pickable)
            {
                wlm.player.GetComponent<RPGController>().Pickup(this);
            }
            if (interactBehaviour != null)
            {
                interactBehaviour.Interact();
            }
        }
    }

    public void Activate()
    {
        if (onScreen)
        {
            if (activateBehaviour != null)
            {
                activateBehaviour.Activate();
            }
        }
    }

    public void Deactivate()
    {
        if (onScreen)
        {
            if (activateBehaviour != null)
            {
                activateBehaviour.Deactivate();
            }
        }
    }

    public void FixedUpdate()
    {
        if (onScreen)
        {
            if (updateBehaviour != null)
            {
                updateBehaviour.EUpdate();
            }
        }
    }

    private void OnBecameInvisible()
    {
        onScreen = false;
    }

    private void OnBecameVisible()
    {
        onScreen = true;
    }
}