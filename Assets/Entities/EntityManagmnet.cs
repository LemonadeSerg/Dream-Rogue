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
        cow,
    }

    public enum hitType
    {
        hand,
        sword,
        explosion,
        fire,
        arrow,
    }

}