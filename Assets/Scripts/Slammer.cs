using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slammer : MonoBehaviour
{
    public Transform theSlammer, slammerTarget;
    private Vector3 startPoint;

    public float slamSpeed, waitAfterSlam, resetSpeed;
    private float waitCounter;
    private bool slamming, resetting;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = theSlammer.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!slamming && !resetting)
        {
            if (Vector3.Distance(slammerTarget.position, PlayerController.instance.transform.position) < 2f)
            {
                slamming = true;
                waitCounter = waitAfterSlam;
            }
        }

        if (slamming)
        {
            theSlammer.position = Vector3.MoveTowards(theSlammer.position, slammerTarget.position, slamSpeed * Time.deltaTime);



            if (theSlammer.position == slammerTarget.position)
            {
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    slamming = false;
                    resetting = true;
                }

            }
        }

        if (resetting)
        {
            theSlammer.position = Vector3.MoveTowards(theSlammer.position, startPoint, resetSpeed * Time.deltaTime);

            if (theSlammer.position == startPoint)
            {
                resetting = false;
            }
        }
    }
}
