using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpByPlayer : MonoBehaviour {

    public GameObject player;
    public float maxDistance;

    private bool beingHeld;

	// Use this for initialization
	void Start () {
        beingHeld = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        pickUp();

    }

    // Update is called once per frame
    void Update()
    {
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

        if (beingHeld) {
            gameObject.transform.localPosition = new Vector2(0, 0);
        }
    }


    public void pickUp()
    {
        gameObject.transform.SetParent(player.GetComponent<Transform>());
        beingHeld = true;
    }

    void drop()
    {
        gameObject.transform.SetParent(null);
        beingHeld = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
