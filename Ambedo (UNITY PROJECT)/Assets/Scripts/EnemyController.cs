using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public BoxCollider2D territory;
    GameObject player;
    private bool playerNearby;

    public GameObject enemy;
    private float speed = 3;
    private Rigidbody2D rbdy;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = false;
        rbdy = gameObject.GetComponent<Rigidbody2D>();
        //rbdy.velocity = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = transform.GetChild(0).GetComponent<EnemyAggroZone>().playerNearby;
        if (playerNearby & player != null)
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

        //move towards and face player
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

    //rest if not nearby
    private void Rest()
    {
        rbdy.velocity = Vector2.zero;
       // rbdy.Sleep();
        gameObject.GetComponent<AudioSource>().enabled = false;
    } 
}