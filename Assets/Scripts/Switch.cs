using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public GameObject objectToSwitch;

    private SpriteRenderer theSR;
    public Sprite downSprite;

    private bool hasSwitched;

    public bool deactivateOnSwitch;

    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !hasSwitched) 
        {

            if (deactivateOnSwitch)
            {
                objectToSwitch.SetActive(false);
            } else
            {
                objectToSwitch.SetActive(true);              
            }

            theSR.sprite = downSprite;
            hasSwitched = true;
        }
    }
}
