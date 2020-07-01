using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobBehaviour : BehaviourBase
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void EUpdate()
    {
        if (Camera.main.WorldToScreenPoint(this.transform.position).x > 0 && Camera.main.WorldToScreenPoint(this.transform.position).x > 1 && Camera.main.WorldToScreenPoint(this.transform.position).y > 0 && Camera.main.WorldToScreenPoint(this.transform.position).y > 1)
        {
            Vector2 playerDir = (this.entity.wlm.player.transform.position - this.transform.position).normalized;
            this.GetComponent<Rigidbody2D>().velocity = playerDir * this.entity.uq.speed;
        }
    }

    public override void Hit(EntityBase.hitType hitType)
    {
    }

    public override void init()
    {
    }

    public override void Interact()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<RPGController>().Hit(EntityBase.hitType.Hand, this.entity.uq.damage);
        }
    }
}