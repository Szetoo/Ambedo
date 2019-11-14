using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public BoxCollider2D territory;
    GameObject player;
    private bool playerNearby;

    public GameObject enemy;
    private float speed = 2;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        playerNearby = transform.GetChild(0).GetComponent<EnemyAggroZone>().playerNearby;
        if (playerNearby & player != null)
        {
            MoveToPlayer();
        }
        else
        {
            rest();
        }
 
	}





     private void MoveToPlayer()
    {
        
        //move towards player
        
        if (transform.position.x - player.transform.position.x > 1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(new Vector2(-1*speed * Time.deltaTime, 0));
        }
        else if (transform.position.x - player.transform.position.x < -1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }
    }

    private void rest()
    {

    }
 
}