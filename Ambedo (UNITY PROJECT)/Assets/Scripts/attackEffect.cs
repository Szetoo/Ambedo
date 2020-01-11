using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackEffect : MonoBehaviour
{ 
 GameObject player;
public float maxDistance;
public float offset;

private bool beingHeld;
private SpriteRenderer sprite;
private float swordXPos = 0f;
private float swordYPos = 0f;
private float swordZRotate = 0;
private bool swing = false;
private float direction;

// Use this for initialization
void Start()
{
        beingHeld = false;

}

// Update is called once per frame
void Update()
{
    float horizontalAxis = Input.GetAxis("Horizontal");


    if (horizontalAxis > 0 && swing == false)
    {
        swordXPos = 0.098f;
        swordYPos = -0.08f;
      //  swordZRotate = -50f;
    }

    if (horizontalAxis < 0 && swing == false)
    {
        swordXPos = -0.102f;
        swordYPos = -0.077f;
       // swordZRotate = 50f;

    }

    if (Input.GetMouseButtonUp(0) && beingHeld)
    {
        if (swing == false)
        {
            swing = true;
            swordZRotate = 0;
            direction = swordXPos;

        }
    }



        if (swing){
            GameObject effectIns = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);
           // effectIns.transform.localPosition = new Vector2(swordXPos, swordYPos);
            Destroy(effectIns, 1f);
        }


  
}




}
