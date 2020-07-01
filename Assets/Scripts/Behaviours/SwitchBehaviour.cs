using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : BehaviourBase
{
    public List<EntityBase> ebs;

    public bool switchDefualtState;
    public bool state;
    public SpriteRenderer sr;
    private float hitTime;
    public Animation anim;

    public override void init()
    {
        anim = new Animation();
        AnimationClip anC = new AnimationClip();
        sr = this.GetComponent<SpriteRenderer>();
        ebs = new List<EntityBase>();
        foreach (EntityBase eb in FindObjectsOfType<EntityBase>())
        {
            if (eb.uq.switchCode == this.entity.uq.switchCode && !(eb.name.Contains("on") || eb.name.Contains("off")) && eb.originCell == this.entity.originCell)
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
        if (state != switchDefualtState)
        {
            if (Time.time > hitTime + this.entity.uq.switchMode)
            {
                foreach (EntityBase eb in ebs)
                {
                    flipState();
                }
            }
        }
    }

    public override void Hit(EntityBase.hitType hitType)
    {
        if (this.entity.uq.switchMode == 0)
        {
            flipState();
        }
        else
        {
            hitTime = Time.time;

            flipState();
        }
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void flipState()
    {
        state = !state;
        foreach (EntityBase eb in ebs)
        {
            if (state)
                eb.Activate();
            else
                eb.Deactivate();
        }

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