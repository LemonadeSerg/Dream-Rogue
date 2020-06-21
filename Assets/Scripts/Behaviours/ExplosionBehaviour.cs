using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    public float power;
    public bool growing;
    public float explosionSpeed = 20f;

    // Start is called before the first frame update
    private void Start()
    {
        growing = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.localScale.x < power && growing)
        {
            this.transform.localScale += new Vector3(explosionSpeed * Time.deltaTime, explosionSpeed * Time.deltaTime, explosionSpeed * Time.deltaTime);
        }
        else
        if (transform.localScale.x > power && growing)
        {
            growing = false;
        }
        else
        if (!growing && transform.localScale.x > 0)
        {
            this.transform.localScale -= new Vector3(explosionSpeed * Time.deltaTime, explosionSpeed * Time.deltaTime, explosionSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.ToString() + "ExT");
        if (other.GetComponent<EntityBase>() != null)
        {
            other.GetComponent<EntityBase>().Hit(EntityManagmnet.hitType.explosion);
        }
    }
}