using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamFragBehaviour : MonoBehaviour
{
    public float rotationSpeed = 1f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        this.transform.Rotate(0, 0, rotationSpeed);
    }
}