using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : BehaviourBase
{
    public override void Activate()
    {
        //throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void EUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public override void Hit(EntityBase.hitType hitType)
    {
        //throw new System.NotImplementedException();
    }

    public override void init()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        //throw new System.NotImplementedException();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collide");
        if (collision.gameObject.GetComponent<EntityBase>())
        {
            collision.gameObject.GetComponent<EntityBase>().Hit(EntityBase.hitType.Arrow);
            Destroy(this.gameObject);
        }
    }
}