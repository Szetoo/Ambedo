using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroZone : MonoBehaviour {

    public bool playerNearby;
	// Use this for initialization
	void Start () {
        playerNearby = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNearby = true;
            gameObject.GetComponentInParent<Animator>().SetBool("Moving", true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNearby = true;
            gameObject.GetComponentInParent<Animator>().SetBool("Moving", true);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNearby = false;
            gameObject.GetComponentInParent<Animator>().SetBool("Moving", false);
        }
    }
}
