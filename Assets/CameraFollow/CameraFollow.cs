using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public WorldLoaderManager wlManger;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 CamC = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 CamBL = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 CamTR = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector2Int BLLimit = wlManger.lastRoom * wlManger.roomSize;
        Vector2Int TRLimit = wlManger.lastRoom * wlManger.roomSize + new Vector2Int(1, 1) * wlManger.roomSize;

        UnityEngine.Vector3 target1 = new Vector3(wlManger.player.transform.position.x, wlManger.player.transform.position.y, -10);

        if (target1.x > TRLimit.x - (CamTR.x - CamC.x))
        {
            target1.x = TRLimit.x - (CamTR.x - CamC.x);
        }
        if (target1.y > TRLimit.y - (CamTR.y - CamC.y))
        {
            target1.y = TRLimit.y - (CamTR.y - CamC.y);
        }
        if (target1.x < BLLimit.x + (CamC.x - CamBL.x))
        {
            target1.x = BLLimit.x + (CamC.x - CamBL.x);
        }
        if (target1.y < BLLimit.y + (CamC.y - CamBL.y))
        {
            target1.y = BLLimit.y + (CamC.y - CamBL.y);
        }
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target1, 0.05f);
    }
}