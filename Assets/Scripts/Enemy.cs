using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected int facingDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage()
    {
        Debug.Log("I was damaged!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.collider.GetComponent<PlayerController>();

            if(player.transform.position.x < transform.position.x) 
            {
                player.KnockBack();
            } else if(player.transform.position.x > transform.position.x)
            {
                player.KnockBack();
            }
        }
    }

    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }
}
