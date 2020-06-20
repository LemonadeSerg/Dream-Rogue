using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    public float changeSpeed = 0.1f;

    public SpriteRenderer Background, Overlay;
    private float r = 1f, g = 0f, b = 0f;

    public int stage = 0;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (stage == 0)
        {
            g += changeSpeed;

            if (g >= 1)
            {
                stage = 1;
            }
        }
        if (stage == 1)
        {
            r -= changeSpeed;
            if (r <= 0)
            {
                stage = 2;
            }
        }
        if (stage == 2)
        {
            b += changeSpeed;
            if (b >= 1)
            {
                stage = 3;
            }
        }
        if (stage == 3)
        {
            g -= changeSpeed;
            if (g <= 0)
            {
                stage = 4;
            }
        }
        if (stage == 4)
        {
            r += changeSpeed;
            if (r >= 1)
            {
                stage = 5;
            }
        }
        if (stage == 5)
        {
            b -= changeSpeed;
            if (b <= 0)
            {
                stage = 0;
            }
        }
        Background.color = new Color(r, g, b);
        Overlay.color = new Color(1 - r, 1 - g, 1 - b);
    }
}