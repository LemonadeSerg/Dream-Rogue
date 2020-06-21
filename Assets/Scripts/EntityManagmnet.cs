using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManagmnet : MonoBehaviour
{
    public EntityBase[] entities;

    public enum Behaviour
    {
        Bush,
        Rock,
        Sign,
        DFrag,
        Bomb,
        Explosion,
        Arrow,
    }

    public enum hitType
    {
        hand,
        sword,
        explosion,
        fire,
        arrow,
        boomerang,
    }
}