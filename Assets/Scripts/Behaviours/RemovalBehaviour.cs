using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovalBehaviour : BehaviourBase
{
    public override void Activate()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<Collider2D>().enabled = true;
    }

    public override void Deactivate()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
    }

    public override void EUpdate()
    {
        throw new System.NotImplementedException();
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
}