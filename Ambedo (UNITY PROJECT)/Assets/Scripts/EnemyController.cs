using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    GameObject player;
    private bool playerNearby;


    private float speed = 2.5f;
    private Rigidbody2D rbdy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = false;
        rbdy = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
        rbdy.velocity = new Vector2(0, rbdy.velocity.y);

        //Move towards and face (by flipping sprite) player
        gameObject.GetComponent<AudioSource>().enabled = true;

        if (transform.position.x - player.transform.position.x > 1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
        }
        else if (transform.position.x - player.transform.position.x < -1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }
        /*
        if (transform.position.x - player.transform.position.x > 1)
        {
           
            transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
        }
        else if (transform.position.x - player.transform.position.x < -1)
        {
            
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }*/
        //yield return new WaitForSeconds(0f);
    }

    //Rest if not nearby
    private void Rest()
    {
        rbdy.velocity = new Vector2(0, rbdy.velocity.y);
        gameObject.GetComponent<AudioSource>().enabled = false;
    }
}