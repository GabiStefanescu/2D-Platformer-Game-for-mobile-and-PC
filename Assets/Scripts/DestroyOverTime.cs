using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{

    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*lifeTime -= Time.deltaTime;

        if(lifeTime < 0 )
        {
            Destroy(gameObject);
        }*/

        Destroy(gameObject, lifeTime); //it will use that value "lifeTime" and wait for the amount of time to pass and destroy 
    }
}
