using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public bool actionState;

    public abstract void Down(WorldLoaderManager wlm);

    public abstract void Up(WorldLoaderManager wlm);
}