using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public float timer;

    public float power;

    public float IgnitionTime;
    public bool ignited = false;
    public CircleCollider2D explosionCollider;
    public bool exploded = false;
    public ContactFilter2D contactFil;
    // Start is called before the first frame update

    private void Start()
    {
    }

    public void ignite()
    {
        IgnitionTime = Time.time;
        ignited = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!exploded && Time.time > IgnitionTime + timer && ignited)
        {
            explode();
        }
    }

    private void explode()
    {
        exploded = true;
        FindObjectOfType<WorldLoaderManager>().spawnEntity(ScenePersistantData.getEntityFromName("Explosion"), this.transform.position, power.ToString(), 0, false, true);
        Destroy(this.gameObject);
        Destroy(this);
    }
}