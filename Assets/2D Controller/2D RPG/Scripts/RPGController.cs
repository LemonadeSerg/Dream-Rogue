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

    private Vector2 axis;
    private Vector2 dashDir;
    private Animator animator;
    private Rigidbody2D rb;

    public bool canDash;
    public bool dashing;
    private float dashInitTime;

    private bool fixedPos;
    private Vector2 lastDir;

    private bool pickingUp = false;
    private bool holding = false;

    public BoxCollider2D interactionBox;

    public ContactFilter2D interactionContactFilter;

    public int objectsInInteraction;

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
            pickingUp = false;
            fixedPos = false;
        }
    }

    private void InputHandling()
    {
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && canDash && !holding && !pickingUp)
            Dash();
        if (!dashing && !fixedPos)
        {
            if (Input.GetKeyDown(KeyCode.Z) && !holding)
                pickup();
            else if (Input.GetKeyDown(KeyCode.Z) && holding)
                throwO();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] collidersInInteraction = new Collider2D[10];
            objectsInInteraction = interactionBox.OverlapCollider(interactionContactFilter, collidersInInteraction);
            for (int i = 0; i < objectsInInteraction; i++)
            {
                if (collidersInInteraction[i].GetComponent<EntityBase>() != null)
                {
                    collidersInInteraction[i].GetComponent<EntityBase>().Interact();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D[] collidersInInteraction = new Collider2D[10];

            objectsInInteraction = interactionBox.OverlapCollider(interactionContactFilter, collidersInInteraction);
            for (int i = 0; i < objectsInInteraction; i++)
            {
                if (collidersInInteraction[i].GetComponent<EntityBase>() != null)
                {
                    collidersInInteraction[i].GetComponent<EntityBase>().Hit(EntityManagmnet.hitType.hand);
                }
            }
        }
        if (axis.x > 0)
            interactionBox.offset = new Vector2(0.5f, -0.1f);
        if (axis.x < 0)
            interactionBox.offset = new Vector2(-0.5f, -0.1f);
        if (axis.y < 0)
            interactionBox.offset = new Vector2(0f, -0.5f);
        if (axis.y > 0)
            interactionBox.offset = new Vector2(0f, 0.4f);
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

    private void pickup()
    {
        pickingUp = true;
        holding = true;
        fixPos();
    }

    private void throwO()
    {
        pickingUp = false;
        holding = false;
    }

    private void fixPos()
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
}