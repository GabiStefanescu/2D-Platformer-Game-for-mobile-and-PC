using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOpossum : MonoBehaviour
{

    public float moveSpeed;

    public Transform leftPoint, rightPoint;

    private bool movingRight;

    private int facingDirection = -1;

    private Rigidbody2D theRB;
    public SpriteRenderer theSR;

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();

        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(movingRight)
        {
            theRB.velocity = new Vector2 (moveSpeed, theRB.velocity.y);

            theSR.flipX = true;

            if(transform.position.x > rightPoint.position.x)
            {
                movingRight = false;
            }
        }else
        {
            theRB.velocity = new Vector2 (-moveSpeed, theRB.velocity.y);

            theSR.flipX = false;

            if(transform.position.x < leftPoint.position.x)
            {
                movingRight = true;
            }
        }
    }
}
