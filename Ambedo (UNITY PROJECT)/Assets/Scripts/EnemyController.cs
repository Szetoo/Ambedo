using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public enum MovementType { moveTowardsPlayer, moveAwayFromPlayer, flying, shootProjectiles, stationary, selfDefense, terrifiedOfDying };
    public MovementType ai;

    public float projectileSpeed;
    public float projectileFireRate;

    public BoxCollider2D territory;
    GameObject player;
    private bool playerNearby;

    public GameObject enemy;
    public GameObject projectileSpawner;
    private float speed = 2;
    private Rigidbody2D rbdy;
    private int moveMode;
    private Vector2 pos;
    

    private void Awake()
    {
        switch (ai)
        {
            case MovementType.moveTowardsPlayer:
                moveMode = 1;
                break;
            case MovementType.moveAwayFromPlayer:
                moveMode = 2;
                break;
            case MovementType.flying:
                moveMode = 3;
                break;
            case MovementType.shootProjectiles:
                moveMode = 4;
                break;
            case MovementType.stationary:
                moveMode = 5;
                break;
            case MovementType.selfDefense:
                moveMode = 6;
                break;
            case MovementType.terrifiedOfDying:
                moveMode = 7;
                break;
        }
    }

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = false;
        rbdy = gameObject.GetComponent<Rigidbody2D>();
        //rbdy.velocity = Vector2.zero;
        if (moveMode == 3)
        {
            rbdy.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        if (moveMode == 4)
        {
            //projectileSpawner.transform.position = new Vector3(-127, 0);
            GameObject spawner = Instantiate(projectileSpawner);
            spawner.transform.parent = gameObject.transform;
            spawner.transform.localPosition = Vector2.zero;
            spawner.transform.localRotation = gameObject.transform.rotation;
        }
        
        pos = new Vector2(gameObject.GetComponent<Transform>().transform.position.x, gameObject.GetComponent<Transform>().transform.position.y);
    }

    // Update is called once per frame
    void Update() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = transform.GetChild(0).GetComponent<EnemyAggroZone>().playerNearby;

        switch(moveMode) { 
            case 1:
                if (playerNearby & player != null & gameObject.GetComponent<Animator>().GetBool("Alive"))
                {
                    MoveToPlayer();
                }
                else
                {
                    Rest();
                }
                break;
            case 2:
                if (playerNearby & player != null & gameObject.GetComponent<Animator>().GetBool("Alive"))
                {
                    MoveAwayFromPlayer();
                }
                else
                {
                    Rest();
                }
                break;
            case 3:
                if (playerNearby & player != null & gameObject.GetComponent<Animator>().GetBool("Alive"))
                {
                    MoveDown();
                }
                else
                {
                    gameObject.GetComponent<Transform>().transform.position = pos;
                    Rest();
                }
                break;
            case 4:
                if (transform.gameObject.GetComponent<SpriteRenderer>().flipX)
                {
                    transform.GetChild(1).transform.localRotation.Set(0,0,180, transform.rotation.w);
                    Debug.Log(transform.GetChild(1).name);
                }
                else
                {
                    transform.GetChild(1).transform.localRotation.Set(0,0,0, transform.rotation.w);
                }
                Rest();
                break;
            case 5:
                Rest();
                break;
            case 6:
                if (playerNearby & player != null & gameObject.GetComponent<Animator>().GetBool("Alive") & gameObject.GetComponent<EnemyHealth>().currentHP < gameObject.GetComponent<EnemyHealth>().maxHP)
                {
                    speed = Mathf.Min(speed * 1.02f, player.GetComponent<PlayerMovementController>().xSpeed);
                    MoveToPlayer();
                }
                else
                {
                    speed = 2;
                    Rest();
                }
                break;
            case 7:
                if (playerNearby & player != null & gameObject.GetComponent<Animator>().GetBool("Alive") & gameObject.GetComponent<EnemyHealth>().currentHP < gameObject.GetComponent<EnemyHealth>().maxHP)
                {
                    speed = Mathf.Min(speed * 1.02f, player.GetComponent<PlayerMovementController>().xSpeed);
                    MoveAwayFromPlayer();
                }
                else
                {
                    speed = 2;
                    Rest();
                }
                break;
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

    private void MoveAwayFromPlayer()
    {
        gameObject.GetComponent<AudioSource>().enabled = true;

        if (transform.position.x - player.transform.position.x < -1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
            
        }
        else if (transform.position.x - player.transform.position.x > 1)
        {
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
           
        }
    }

    private void MoveDown()
    {
        gameObject.GetComponent<AudioSource>().enabled = true;
        transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime));
    }
}
