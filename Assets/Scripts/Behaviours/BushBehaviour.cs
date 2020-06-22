using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushBehaviour : BehaviourBase
{
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(EntityBase.hitType hitType)
    {
        Destroy(this.gameObject);
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public override void EUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void init()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}