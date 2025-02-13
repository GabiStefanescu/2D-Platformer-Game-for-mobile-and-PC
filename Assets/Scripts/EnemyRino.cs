using UnityEngine;

public class EnemyRino : MonoBehaviour
{

    public static EnemyRino instance;

    private Animator anim;
    private Rigidbody2D theRB;

    private int facingDirection = -1;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatToIgnore;
    [SerializeField] private LayerMask whatIsPlayer;    
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    private bool wallDetected;
    private bool groundDetected;
    private RaycastHit2D playerDetection;

    private Transform player;
    [HideInInspector] public bool invincible;

    /*[Header("FX")]
    [SerializeField] private GameObject deathFx; */

    [Header("Move info")]
    [SerializeField] private float speed;
    [SerializeField] private float idleTime = 2;
    private float idleTimeCounter;

    private bool canMove = true;
    private bool aggresive;

    [Header("Rino specific")]
    [SerializeField] private float agroSpeed;
    [SerializeField] private float shockTime;
    private float shockTimeCounter;

    protected void Start()
    {
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody2D>();

        InvokeRepeating("FindPlayer", 0, .9f);
        FindPlayer();

        if (groundCheck == null)
            groundCheck = transform;
        if (wallCheck == null)
            wallCheck = transform;

        invincible = true;
    }

    private void FindPlayer()
    {
        if (player != null)
            return;

        if (PlayerController.instance.transform != null)
            player = PlayerController.instance.transform;
    }

    private void Update()
    {
        PerformCollisionChecks();
        UpdateAnimator();

        if (!IsPlayerDetected())
        {
            WalkAround();
            return;
        }

        aggresive = IsPlayerDetected();

        if (!aggresive)
        {
            WalkAround();
        }
        else
        {
            if (!groundDetected)
            {
                aggresive = false;
                Flip();
            }

            AggressiveMove();
        }
    }

    private void PerformCollisionChecks()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 100, ~whatToIgnore);
    }

    private bool IsPlayerDetected()
    {
        return playerDetection.collider != null && playerDetection.collider.GetComponent<PlayerController>() != null;
    }

    private void WalkAround()
    {
        if (idleTimeCounter <= 0 && canMove)
            theRB.velocity = new Vector2(speed * facingDirection, theRB.velocity.y);
        else
            theRB.velocity = new Vector2(0, theRB.velocity.y);

        idleTimeCounter -= Time.deltaTime;

        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
    }

    private void AggressiveMove()
    {
        theRB.velocity = new Vector2(agroSpeed * facingDirection, theRB.velocity.y);

        if (wallDetected && invincible)
        {
            invincible = false;
            shockTimeCounter = shockTime;
        }

        if (shockTimeCounter <= 0 && !invincible)
        {
            invincible = true;
            Flip();
            aggresive = false;
        }

        shockTimeCounter -= Time.deltaTime;
    }

    private void UpdateAnimator()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", theRB.velocity.x);
    }

    public void Damage()
    {
        if (!invincible)
        {
            canMove = false;
            anim.SetTrigger("gotHit");
        }
    }

    /*public void DestroyMe()
    {
        if (deathFx != null)
        {
            GameObject newDeathFx = Instantiate(deathFx, transform.position, transform.rotation);
            Destroy(newDeathFx, .3f);
        }

        var dropController = GetComponent<Enemy_DropController>();
        if (dropController != null)
            dropController.DropFruits();
        else
            Debug.LogWarning("You don't have Enemy_DropController on the enemy!");

        Destroy(gameObject);
    } */

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        if (wallCheck != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDirection, wallCheck.position.y));
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

    
}
