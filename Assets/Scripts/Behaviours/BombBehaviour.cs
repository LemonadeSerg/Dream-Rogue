using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombBehaviour : BehaviourBase
{
    public float timeStart;
    public bool ingited = false;

    public override void Activate()
    {
        timeStart = Time.time;
        ingited = true;
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void EUpdate()
    {
        if (ingited && Time.time > timeStart + this.entity.uq.fuse)
        {
            this.entity.wlm.SpawnEntity(ScenePersistantData.getEntityFromName(this.entity.uq.bombSpawn), this.entity.uq, this.transform.position);
            Destroy(this.gameObject);
        }
    }

    public override void Hit(EntityBase.hitType hitType)
    {
        throw new System.NotImplementedException();
    }

    public override void init()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }
}