using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public Vector2 Dir;
    public int damage;
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
        this.gameObject.layer = 9;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (collision.GetComponent<EntityBase>())
            {
                collision.GetComponent<EntityBase>().Hit(EntityManagmnet.hitType.arrow);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (collision.gameObject.GetComponent<EntityBase>())
            {
                collision.gameObject.GetComponent<EntityBase>().Hit(EntityManagmnet.hitType.arrow);
            }
            Destroy(this.gameObject);
        }
    }
}