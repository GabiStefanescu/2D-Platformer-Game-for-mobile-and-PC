using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    public static EnemyGhost instance;
    private Animator anim;

    private int facingDirection = -1;

    [Header("Ghost specific")]
    [SerializeField] private float activeTime;
    private float activeTimeCounter = 4;

    [Header("Move info")]
    [SerializeField] private float speed;
    [SerializeField] private float idleTime = 2;
    private float idleTimeCounter;
    private bool canMove = true;
    private bool aggresive;

    private Transform player;
    
    [HideInInspector] public bool invincible;
    
    private SpriteRenderer theSR;

    [SerializeField] private float[] xOffset;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();

        aggresive = true;
        invincible = true;

        player = GameObject.Find("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            anim.SetTrigger("desappear");
            return;
        }

        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if (activeTimeCounter > 0)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && aggresive)
        {
            anim.SetTrigger("desappear");
            aggresive = false;
            idleTimeCounter = idleTime;
        }

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && !aggresive)
        {
            ChoosePosition();
            anim.SetTrigger("appear");
            aggresive = true;
            activeTimeCounter = activeTime;
        }

        FlipController();
    }

    private void FlipController()
    {
        if (player == null)
            return;

        if (facingDirection == -1 && transform.position.x < player.transform.position.x)
            Flip();
        else if (facingDirection == 1 && transform.position.x > player.transform.position.x)
            Flip();
    }

    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    private void ChoosePosition()
    {
        float _xOffset = xOffset[Random.Range(0, xOffset.Length)];
        float _yOffset = Random.Range(-10, 10);
        transform.position = new Vector2(player.transform.position.x + _xOffset, player.transform.position.y + _yOffset);
    }

    public void Desappear() => theSR.enabled = false;

    public void Appear() => theSR.enabled = true;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {   
        if(aggresive)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                PlayerController player = other.GetComponent<PlayerController>();

                player.KnockBack();
            }
        }
        
    }
}
