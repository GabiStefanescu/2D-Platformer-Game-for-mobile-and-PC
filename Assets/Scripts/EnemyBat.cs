using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBat : MonoBehaviour
{
    public static EnemyBat Instance;

    private Animator anim;
    
    private int facingDirection = -1;

    [Header("Bat specifics ")]
    [SerializeField] private Transform[] idlePoint;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("Move info")]
    [SerializeField] private float speed;
    [SerializeField] private float idleTime = 2;
    private float idleTimeCounter;
    private bool canMove = true;
    private bool aggresive;

    private Transform player;
    [HideInInspector] public bool invincible;

    private bool playerDetected;

    private Vector2 destination;
    private bool canBeAggresive = true;


    float defaultSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
        player = GameObject.Find("Player").transform;

        defaultSpeed = speed;
        destination = idlePoint[0].position;
        transform.position = idlePoint[0].position;

        for (int i = 0; i < idlePoint.Length; i++)
        {
            idlePoint[i].GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("canBeAggresive", canBeAggresive);
        anim.SetFloat("speed", speed);


        idleTimeCounter -= Time.deltaTime;
        if (idleTimeCounter > 0)
            return;

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);

        if (playerDetected && !aggresive && canBeAggresive)
        {
            aggresive = true;
            canBeAggresive = false;

            if (player != null)
                destination = player.transform.position;
            else
            {
                aggresive = false;
                canBeAggresive = true;
            }
        }


        if (aggresive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                aggresive = false;

                int i = Random.Range(0, idlePoint.Length);

                destination = idlePoint[i].position;
                speed = speed * .5f;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                if (!canBeAggresive)
                    idleTimeCounter = idleTime;

                canBeAggresive = true;
                speed = defaultSpeed;
            }
        }


        FlipController();
    }


    private void FlipController()
    {
        if (player == null)
            return;

        if (facingDirection == -1 && transform.position.x < destination.x)
            Flip();
        else if (facingDirection == 1 && transform.position.x > destination.x)
            Flip();
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    public void Damage()
    {
        if (!invincible)
        {
            canMove = false;
            anim.SetTrigger("gotHit");
            idleTimeCounter = 5;
        }
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            player.KnockBack();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
