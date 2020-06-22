using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public WorldLoaderManager wlm;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private PolygonCollider2D pc;

    public string name;
    public Sprite sprite;
    public bool solid;
    public bool pushable;
    public bool pickable;

    private BehaviourBase hitBehaviour;
    private BehaviourBase activateBehaviour;
    private BehaviourBase movementBehaviour;
    private BehaviourBase interactBehaviour;

    public EntityUniqueData uq;

    public Vector2 originCell;

    public enum hitType
    {
        None,
        Hand,
        Arrow,
        Bomb,
    }

    public enum hitBehaviours
    {
        None,
        Switch,
        Bush,
    }

    public hitBehaviours hitB;

    public enum activateBehaviours
    {
        None,
    }

    public activateBehaviours actB;

    public enum movementBehaviours
    {
        None,
        Arrow,
    }

    public movementBehaviours movB;

    public enum interactBehaviours
    {
        None,
        Sign,
    }

    public interactBehaviours intB;

    private void Start()
    {
        if (FindObjectOfType<WorldLoaderManager>() != null)
            wlm = FindObjectOfType<WorldLoaderManager>();
    }

    public void init()
    {
        sr = this.gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 2;
        pc = this.gameObject.AddComponent<PolygonCollider2D>();
        pc.isTrigger = !solid;

        rb = this.gameObject.AddComponent<Rigidbody2D>();
        if (!pushable)
            rb.bodyType = RigidbodyType2D.Static;

        if (wlm != null)
            originCell = wlm.getBoardAtVector(this.transform.position.x, this.transform.position.y);

        switch (hitB)
        {
            case hitBehaviours.Switch:
                //hitBehaviour = this.gameObject.AddComponent<SwitchBehaviour>();
                //hitBahaviour.entity = this;
                break;

            case hitBehaviours.Bush:
                hitBehaviour = this.gameObject.AddComponent<BushBehaviour>();
                hitBehaviour.entity = this;
                break;

            default:
                break;
        }
        switch (movB)
        {
            case movementBehaviours.Arrow:
                //movementBehaviour = this.gameObject.AddComponent<ArrowBehaviour>();
                //movementBehaviour.entity = this;
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
    }

    private void Update()
    {
    }

    public void Hit(hitType hitType)
    {
        if (hitBehaviour != null)
        {
            hitBehaviour.Hit(hitType);
        }
    }

    public void Interact()
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

    public void Activate()
    {
        if (activateBehaviour != null)
        {
            activateBehaviour.Activate();
        }
    }

    public void FixedUpdate()
    {
        if (movementBehaviour != null)
        {
            activateBehaviour.MoveUpdate();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}