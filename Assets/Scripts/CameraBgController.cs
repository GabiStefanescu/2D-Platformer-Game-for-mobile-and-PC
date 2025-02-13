using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraBgController : MonoBehaviour
{

    public static CameraBgController instance;

    public Transform target;

    public Transform farBackground, middleFarBackground, middleBackground, middleNearBackground, nearBackground;

    public float minHeight, maxHeight;

    public bool stopFollow;

    //private float lastXPos;
    private Vector2 lastPos;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /* transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        float clampedY = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z); */

        if (!stopFollow)
        {
            transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);


            //float amountToMoveX = transform.position.x - lastXPos;
            Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

            farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);

            middleFarBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;

            middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .55f;

            middleNearBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .65f;

            nearBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .75f;

            //lastXPos = transform.position.x;
            lastPos = transform.position;
        }

    }
}
