using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class RPGController : MonoBehaviour
{
    public float moveSpeed;
    public float dashSpeed;
    public float carrySpeed;
    public float dashRechTime;
    public float dashTime;

    public Vector2 axis;
    private Vector2 dashDir;
    public Animator animator;
    private Rigidbody2D rb;

    public bool canDash;
    public bool dashing;

    private float dashInitTime;

    private bool fixedPos;
    public Vector2 lastDir;

    public bool pickingUp = false;
    public bool holding = false;

    public BoxCollider2D interactionBox;

    public ContactFilter2D interactionContactFilter;

    public int objectsInInteraction;

    public GameObject holdPos;

    public WorldLoaderManager wlm;

    public GameObject arrowFireSpot;

    public EntityBase heldObj;

    public ItemBase item1;
    public ItemBase item2;

    // Start is called before the first frame update
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        InputHandling();
        playerUpdater();
        if (!fixedPos && !ScenePersistantData.paused) applyMovement();

        if (ScenePersistantData.paused)
        {
            lastDir = rb.velocity.normalized;
            rb.velocity = Vector2.zero;
        }

        pushAnimationMotionToAnimtor();
    }

    private AnimatorStateInfo m_CurrentStateInfo;

    private void playerUpdater()

    {
        if (dashInitTime + dashTime < Time.time)
            dashing = false;

        if (dashInitTime + dashRechTime < Time.time)
            canDash = true;
        else
            canDash = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Pickup") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walking_Pickup"))
        {
            if (pickingUp)
            {
                holding = true;
                pickingUp = false;
                fixedPos = false;
            }
        }
        if (holding)
        {
            heldObj.transform.position = holdPos.transform.position;
        }
    }

    private void InputHandling()
    {
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");
        if (!fixedPos)
        {
            if (!dashing)
            {
                if (!holding)
                {
                    if (!pickingUp)
                    {
                        if (Input.GetButtonDown("Jump") && canDash)
                            Dash();
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactionBox.enabled = true;
                            Collider2D[] colliders = new Collider2D[10];
                            interactionBox.OverlapCollider(interactionContactFilter, colliders);

                            foreach (Collider2D c in colliders)
                            {
                                if (c != null)
                                    if (c.GetComponent<EntityBase>() != null)
                                    {
                                        c.GetComponent<EntityBase>().Interact();
                                    }
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            interactionBox.enabled = true;
                            Collider2D[] colliders = new Collider2D[10];
                            interactionBox.OverlapCollider(interactionContactFilter, colliders);

                            foreach (Collider2D c in colliders)
                            {
                                if (c != null)
                                    if (c.GetComponent<EntityBase>() != null)
                                    {
                                        c.GetComponent<EntityBase>().Hit(EntityBase.hitType.Hand);
                                    }
                            }
                        }

                        if (item1 != null)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                item1.Down(wlm);
                            }
                            if (Input.GetMouseButtonUp(0))
                            {
                                item1.Up(wlm);
                            }
                            item1.actionState = Input.GetMouseButton(0);
                        }
                        if (item2 != null)
                        {
                            if (Input.GetMouseButtonDown(1))
                            {
                                item2.Down(wlm);
                            }
                            if (Input.GetMouseButtonUp(1))
                            {
                                item2.Up(wlm);
                            }
                            item2.actionState = Input.GetMouseButton(1);
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        throwObj();
                    }
                }
            }
        }

        if (animator.GetFloat("YAxis") > 0 && animator.GetFloat("YAxis") > animator.GetFloat("XAxis") && animator.GetFloat("YAxis") > -animator.GetFloat("XAxis"))
        {
            arrowFireSpot.transform.localPosition = new Vector2(0f, 0.4f);
            interactionBox.offset = new Vector2(0f, 0.4f);
        }
        else
        if (animator.GetFloat("YAxis") < 0 && animator.GetFloat("YAxis") < animator.GetFloat("XAxis") && animator.GetFloat("YAxis") < -animator.GetFloat("XAxis"))
        {
            arrowFireSpot.transform.localPosition = new Vector2(0f, -0.4f);
            interactionBox.offset = new Vector2(0f, -0.5f);
        }
        else
        if (animator.GetFloat("XAxis") > 0 && animator.GetFloat("XAxis") > animator.GetFloat("YAxis") && animator.GetFloat("XAxis") > -animator.GetFloat("YAxis"))
        {
            arrowFireSpot.transform.localPosition = new Vector2(0.3f, -0.1f);
            interactionBox.offset = new Vector2(0.5f, -0.1f);
        }
        else
        if (animator.GetFloat("XAxis") < 0 && animator.GetFloat("XAxis") < animator.GetFloat("YAxis") && animator.GetFloat("XAxis") < -animator.GetFloat("YAxis"))
        {
            arrowFireSpot.transform.localPosition = new Vector2(-0.3f, -0.1f);
            interactionBox.offset = new Vector2(-0.5f, -0.1f);
        }
    }

    public void spawnToHand(EntityBase entityBase)
    {
        entityBase.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        heldObj = entityBase;
        holding = true;
    }

    public void Pickup(EntityBase entity)
    {
        entity.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        heldObj = entity;
        pickingUp = true;
        fixPos();
    }

    private void throwObj()
    {
        heldObj.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
        if (heldObj.updB == EntityBase.updateBehaviours.Bomb)
            heldObj.Activate();
        holding = false;
    }

    private void applyMovement()
    {
        if (dashing)
            rb.velocity = (dashDir / 2 + axis / 2) * dashSpeed;
        else if (holding)
            rb.velocity = axis * carrySpeed;
        else
            rb.velocity = axis * moveSpeed;
    }

    private void pushAnimationMotionToAnimtor()
    {
        animator.SetBool("Moving", axis != Vector2.zero);
        if (fixedPos || ScenePersistantData.paused)
        {
            animator.SetBool("Moving", false);
            if (lastDir.x != 0 || lastDir.y != 0)
            {
                animator.SetFloat("XAxis", lastDir.x);
                animator.SetFloat("YAxis", lastDir.y);
            }
        }
        else
        {
            if (axis.x != 0 || axis.y != 0)
            {
                animator.SetFloat("XAxis", axis.x);
                animator.SetFloat("YAxis", axis.y);
            }
        }
        animator.SetBool("Pickup", pickingUp);
        animator.SetBool("Holding", holding);
    }

    public void fixPos()
    {
        lastDir = rb.velocity.normalized;
        rb.velocity = Vector2.zero;
        fixedPos = !fixedPos;
    }

    private void Dash()
    {
        dashInitTime = Time.time;
        dashDir = rb.velocity.normalized;
        dashing = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}