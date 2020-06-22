using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : BehaviourBase
{
    public List<EntityBase> ebs;

    public bool switchDefualtState;
    public bool state;
    public SpriteRenderer sr;

    public override void init()
    {
        sr = this.GetComponent<SpriteRenderer>();
        ebs = new List<EntityBase>();
        foreach (EntityBase eb in FindObjectsOfType<EntityBase>())
        {
            if (eb.uq.switchCode == this.entity.uq.switchCode && !(eb.name.Contains("on") || eb.name.Contains("off")))
            {
                ebs.Add(eb);
            }
        }
        if (this.entity.name.Contains("on"))
        {
            switchDefualtState = true;
        }
        if (this.entity.name.Contains("off"))
        {
            switchDefualtState = false;
        }
        state = switchDefualtState;
    }

    public override void Activate()

    {
        throw new System.NotImplementedException();
    }

    public override void EUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(EntityBase.hitType hitType)
    {
        if (this.entity.uq.switchMode == 0)
        {
            flipState();
            state = !state;
            foreach (EntityBase eb in ebs)
            {
                if (state)
                    eb.Activate();
                else
                    eb.Deactivate();
            }
        }
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void flipState()
    {
        if (this.GetComponent<SpriteRenderer>().sprite.name.Contains("on"))
            this.GetComponent<SpriteRenderer>().sprite = ScenePersistantData.getEntityFromName(this.GetComponent<SpriteRenderer>().sprite.name.Replace("-on", "-off")).sprite;
        else
            this.GetComponent<SpriteRenderer>().sprite = ScenePersistantData.getEntityFromName(this.GetComponent<SpriteRenderer>().sprite.name.Replace("-off", "-on")).sprite;
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}