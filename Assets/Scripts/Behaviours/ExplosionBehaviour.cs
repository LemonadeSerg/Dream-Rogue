using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : BehaviourBase
{
    public float explosionSpeed = 1.2f;
    public bool flip = false;

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void EUpdate()
    {
        if (!flip && this.transform.localScale.x < this.entity.uq.power)
        {
            this.transform.localScale = this.transform.localScale * (explosionSpeed);
        }
        if (!flip && this.transform.localScale.x > this.entity.uq.power)
            flip = true;
        if (flip && this.transform.localScale.x > 0)
        {
            this.transform.localScale = this.transform.localScale / (explosionSpeed);
        }
        if (flip && this.transform.localScale.x <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Hit(EntityBase.hitType hitType)
    {
        throw new System.NotImplementedException();
    }

    public override void init()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EntityBase>())
        {
            collision.gameObject.GetComponent<EntityBase>().Hit(EntityBase.hitType.Explosion);
        }
    }
}