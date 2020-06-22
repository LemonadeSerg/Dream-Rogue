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
        float xAxis = 0, yAxis = 0;
        if (Input.GetKey(KeyCode.UpArrow))
            yAxis = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            yAxis = -1;
        if (Input.GetKey(KeyCode.LeftArrow))
            xAxis = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            xAxis = 1;

        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + (Vector3.right * xAxis) + (Vector3.up * yAxis), 0.1f);
    }
}