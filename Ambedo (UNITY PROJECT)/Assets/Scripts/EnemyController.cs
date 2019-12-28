using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    GameObject player;
    private bool playerNearby;


    private float speed = 2;
    private Rigidbody2D rbdy;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = false;
        rbdy = gameObject.GetComponent<Rigidbody2D>();
    }
	
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = transform.GetChild(0).GetComponent<EnemyAggroZone>().playerNearby;
        
        //If enemy is alive and player is nearby, move to player
        if (playerNearby & player != null & gameObject.GetComponent<Animator>().GetBool("Alive") == true)
        {
            MoveToPlayer();
        }
        else
        {
            Rest();
        }
	}

     private void MoveToPlayer()
    {

        //Move towards and face (by flipping sprite) player
        gameObject.GetComponent<AudioSource>().enabled = true;

        if (transform.position.x - player.transform.position.x > 1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(new Vector2(-1*speed * Time.deltaTime, 0));
        }
        if (transform.position.x - player.transform.position.x < -1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }
    }

    //Rest if not nearby
    private void Rest()
    {
        rbdy.velocity = Vector2.zero;
        gameObject.GetComponent<AudioSource>().enabled = false;
    } 
}