using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    private string name;

    public Sprite sprite;

    public abstract void clickItem(WorldLoaderManager wlm);

    public abstract void holdItem(WorldLoaderManager wlm);
}