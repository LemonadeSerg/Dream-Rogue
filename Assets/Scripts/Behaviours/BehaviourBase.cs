using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourBase : MonoBehaviour
{
    public EntityBase entity;

    public abstract void Hit(EntityBase.hitType hitType);

    public abstract void Activate();

    public abstract void Interact();

    public abstract void MoveUpdate();
}