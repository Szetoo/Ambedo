using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpByPlayer : MonoBehaviour {

    public GameObject player;
    public float maxDistance;
    public float offset;

    private bool beingHeld;
    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        beingHeld = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

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
            gameObject.transform.localPosition = new Vector2(0.2f * (System.Convert.ToSingle(sprite.flipX) - 0.5f), 0.1f);
            
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
}
