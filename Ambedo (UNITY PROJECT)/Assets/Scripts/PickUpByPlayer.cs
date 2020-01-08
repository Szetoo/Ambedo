using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpByPlayer : MonoBehaviour {

    public GameObject player;
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
    void Start () {
        beingHeld = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
    

        if (Input.GetButtonDown("Interact")) {
            if (!beingHeld)
            {
                float distance = Vector2.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position);
                if (distance <= maxDistance)
                {
                    pickUp();
                }
            } else
            {
                drop();
            }
            
        }

        if (horizontalAxis > 0 && swing == false)
        {
            swordXPos = 0.1f;
            swordYPos = -0.076f;
            swordZRotate = -50f;
        }

        if (horizontalAxis < 0 && swing == false)
        {
            swordXPos = -0.1f;
            swordYPos = -0.07f;
            swordZRotate = 50f;

        }

        if (Input.GetMouseButtonUp(0) && beingHeld)
        {
            if (swing == false)
            {
                swing = true;
                swordZRotate = 0;
                direction = swordXPos;
                //Debug.Log("direction : "+ direction);

            }
        }


        if (swing)
        {

            if (direction > 0)
            {
                swordZRotate = swordZRotate - 3;
            }
            if(direction < 0)
            {
                swordZRotate = swordZRotate + 3;
            }

            if (Mathf.Abs(swordZRotate) >= 80)
            {

                swing = false;
            }



        }

        if (beingHeld)
        {
            Debug.Log(swordZRotate);
            beinghold(swordXPos,  swordYPos, swordZRotate);


        }
    }


    void pickUp()
    {
        gameObject.transform.SetParent(player.GetComponent<Transform>());
        beingHeld = true;
        sprite = player.GetComponent<SpriteRenderer>();
        Debug.Log(sprite);
    }

    void drop()
    {
        gameObject.transform.SetParent(null);
        beingHeld = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }


    void beinghold(float posx, float posy, float rotz)
    {
        gameObject.transform.localPosition = new Vector2(posx, posy);
        gameObject.transform.eulerAngles = new Vector3(0, 0, rotz);

    }

}
