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
    public float speed = 2;
    private Rigidbody2D rbdy;
    private int moveMode;
    private Vector2 pos;

    private bool flyingIdle;
    private bool isFacingRight;

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
                flyingIdle = true;
                isFacingRight = true;
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
            rbdy.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (moveMode == 4)
        {
            //projectileSpawner.transform.position = new Vector3(-127, 0);
            GameObject spawner = Instantiate(projectileSpawner);
            spawner.transform.parent = gameObject.transform;
            spawner.transform.localPosition = Vector2.zero;
            spawner.transform.localRotation = gameObject.transform.rotation;
            spawner.GetComponent<ProjectileSpawnerController>().fireRate = projectileFireRate;
        }
        
        pos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNearby = transform.GetChild(0).GetComponent<EnemyAggroZone>().playerNearby;
        bool isAlive = gameObject.GetComponent<Animator>().GetBool("Alive");
        if (!isAlive)
        {
            Rest();
        }
        else
        {
            switch (moveMode)
            {
                case 1:
                    if (playerNearby & player != null & isAlive)
                    {
                        MoveToPlayer();
                    }
                    else
                    {
                        Rest();
                    }
                    break;
                case 2:
                    if (playerNearby & player != null & isAlive)
                    {
                        MoveAwayFromPlayer();
                    }
                    else
                    {
                        Rest();
                    }
                    break;
                case 3:
                    if (playerNearby & player != null & isAlive)
                    {
                        flyingIdle = false;
                        MoveDown();
                    }
                    else if (flyingIdle)
                    {
                        if (transform.position.x < pos.x - 1)
                        {

                            isFacingRight = true;
                        }
                        else if (transform.position.x > pos.x + 1)
                        {
                            isFacingRight = false;
                        }

                        if (isFacingRight)
                        {
                            MoveRight();
                        }
                        else
                        {
                            MoveLeft();
                        }
                    }
                    else
                    {
                        MoveUp();
                        if (gameObject.GetComponent<Transform>().transform.position.y >= pos.y)
                        {
                            gameObject.GetComponent<Transform>().transform.position = pos;
                            flyingIdle = true;
                        }


                    }
                    break;
                case 4:
                    if (gameObject.GetComponent<SpriteRenderer>().flipX)
                    {
                        transform.GetChild(1).SetPositionAndRotation(transform.position, Quaternion.identity);
                    }
                    else
                    {
                        transform.GetChild(1).SetPositionAndRotation(transform.position, new Quaternion(0, 0, 180, 1));
                    }

                    Rest();
                    break;
                case 5:
                    Rest();
                    break;
                case 6:
                    if (playerNearby & player != null & isAlive & gameObject.GetComponent<EnemyHealth>().currentHP < gameObject.GetComponent<EnemyHealth>().maxHP)
                    {
                        gameObject.GetComponentInParent<Animator>().SetBool("Hurt", true);
                        speed = Mathf.Min(speed * 1.02f, player.GetComponent<PlayerMovementController>().xSpeed);
                        MoveToPlayer();
                    }
                    else
                    {
                        gameObject.GetComponentInParent<Animator>().SetBool("Hurt", false);
                        speed = 2;
                        Rest();
                    }
                    break;
                case 7:
                    if (playerNearby & player != null & isAlive & gameObject.GetComponent<EnemyHealth>().currentHP < gameObject.GetComponent<EnemyHealth>().maxHP)
                    {
                        gameObject.GetComponentInParent<Animator>().SetBool("Hurt", true);
                        speed = Mathf.Min(speed * 1.02f, player.GetComponent<PlayerMovementController>().xSpeed);
                        MoveAwayFromPlayer();
                    }
                    else
                    {
                        gameObject.GetComponentInParent<Animator>().SetBool("Hurt", false);
                        speed = 2;
                        Rest();
                    }
                    break;
            }
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

    private void MoveUp()
    {
        transform.Translate(transform.up * 2 * speed * Time.deltaTime);
    }

    private void MoveDown()
    {
        gameObject.GetComponent<AudioSource>().enabled = true;
        transform.Translate(-transform.up*4*speed*Time.deltaTime);
    }

    private void MoveLeft()
    {
        transform.Translate(-transform.right * speed * Time.deltaTime);
    }

    private void MoveRight()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);
    }
}
