using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Hit"); 

            //whenever the player enter the trigger area we want to find the player health controller script and run the function deal damage
            //FindObjectOfType<PlayerHealthController>().DealDamage();

            PlayerHealthController.instance.DealDamage();
        }

        
    }
}
