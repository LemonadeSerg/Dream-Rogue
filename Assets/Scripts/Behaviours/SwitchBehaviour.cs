using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    internal string connectionString;

    internal int mode;

    private bool defaultState;

    public EntityBase connectedEntity;

    public bool currentState;

    internal void init()
    {
        EntityBase[] ebs = FindObjectsOfType<EntityBase>();
        foreach (EntityBase eb in ebs)
        {
            if (eb.GetComponent<SwitchBehaviour>() == null && eb.metaText == connectionString)
            {
                connectedEntity = eb;
            }
        }
        if (this.GetComponent<EntityBase>().name.Contains("-on"))
            defaultState = true;
        if (this.GetComponent<EntityBase>().name.Contains("-off"))
            defaultState = false;

        currentState = default;
    }

    public void hit()
    {
        if (mode == 0)
        {
            currentState = !currentState;
            flipState();
            if (currentState == true)
            {
                connectedEntity.activate();
            }
            if (currentState == false)
            {
                connectedEntity.deactivate();
            }
        }
    }

    private void flipState()
    {
        print("flip");
        if (this.GetComponent<SpriteRenderer>().sprite.name.Contains("on"))
            this.GetComponent<SpriteRenderer>().sprite = ScenePersistantData.getEntityFromName(this.GetComponent<SpriteRenderer>().sprite.name.Replace("-on", "-off")).sprite;
        else
            this.GetComponent<SpriteRenderer>().sprite = ScenePersistantData.getEntityFromName(this.GetComponent<SpriteRenderer>().sprite.name.Replace("-off", "-on")).sprite;
    }
}