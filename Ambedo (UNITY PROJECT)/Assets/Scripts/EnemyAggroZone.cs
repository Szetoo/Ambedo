using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroZone : MonoBehaviour {

    public bool playerNearby;
	
    //Player is not nearby on load
	void Start () {
        playerNearby = false;
	}
	
	

	void Update () {
		
	}

    //If player touches aggrozone, player is now nearby and play enemy moving animation
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNearby = true;
            gameObject.GetComponentInParent<Animator>().SetBool("Moving", true);

        }
    }

    //If player is still in aggrozone, player is still nearby and continue playing enemy moving animation
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNearby = true;
            gameObject.GetComponentInParent<Animator>().SetBool("Moving", true);
        }
    }

    //If player leaves aggrozone, player is no longer nearby and go to resting animation
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNearby = false;
            gameObject.GetComponentInParent<Animator>().SetBool("Moving", false);
        }
    }
}
