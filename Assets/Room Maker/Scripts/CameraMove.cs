using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + (Vector3.right * xAxis) + (Vector3.up * yAxis), 0.1f);
    }
}