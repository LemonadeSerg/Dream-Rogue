using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBehaviour : BehaviourBase
{
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(EntityBase.hitType hitType)
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        MessageSystem.message(this.entity.uq.message);
    }

    public override void MoveUpdate()
    {
        throw new System.NotImplementedException();
    }
}