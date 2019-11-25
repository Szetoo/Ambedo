using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    public float xSpeed;
    public float jumpForce;
    public float accelSlowdown;
    public float landingTolerance;
    public float sprintMultiplier;
    public int waterJumpFrames;

    public GameObject hitbox;

    private bool isJumping;
    private Vector2 lastVelocity;
    private bool canStartSprint;
    private bool isInWater;
    private float nextWaterJump;
    private SpriteRenderer sprite;
    private bool attackDelay;

    private IEnumerator attackCoroutine;

    void Start()
    {
        isJumping = false;
        lastVelocity = new Vector2(0.0f, 0.0f);
        canStartSprint = true;
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        horizontalMovement(rb);
        verticalMovement(rb);

    }

    private void LateUpdate()
    {
        try
        {
            if (Input.GetButtonDown("Attack"))
            {
                string heldObjectTag = gameObject.transform.GetChild(0).name;
                if ((heldObjectTag == "Sword" | heldObjectTag == "Sword2") && !attackDelay)
                {
                    attackCoroutine = attack();
                    StartCoroutine(attackCoroutine);
                }
            }
        }
        catch (UnityException e)
        {
            Debug.Log("Not holding an object");
        }
    }

    void horizontalMovement(Rigidbody2D rb)
    {
        float momentum = xSpeed;

        float moveHorizontal = Input.GetAxis("Horizontal");

        //float sprintHorizontal = Input.GetAxis ("Sprint");
        float sprintHorizontal = 0;

        if (isJumping || isInWater)
        {
            //if (sprintHorizontal == 0)
            canStartSprint = false;
            if (rb.velocity.x == 0)
            {
                momentum = 0;
                if (Input.GetButton("Horizontal"))
                    momentum = moveHorizontal;
            }
        }
        else
            canStartSprint = true;

        if (moveHorizontal < 0)
        {
            sprintHorizontal = -sprintHorizontal;
            sprite.flipX = false;
            transform.localScale.Set(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //momentum = -xSpeed;
        }
        else if (moveHorizontal == 0)
        {
            sprite.flipX = sprite.flipX;
        }
        else
        {
            sprite.flipX = true;
            transform.localScale.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }



        if (!(Input.GetButton("Horizontal") && canStartSprint) || isInWater)
            sprintHorizontal = 0;

        Vector2 movement = new Vector2(
            (moveHorizontal * momentum) + (sprintHorizontal * sprintMultiplier),
            rb.velocity.y);

        rb.velocity = movement;


    }

    void verticalMovement(Rigidbody2D rb)
    {

        float acceleration = (rb.velocity.y - lastVelocity.y) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;


        if (isJumping && -landingTolerance <= rb.velocity.y && rb.velocity.y <= landingTolerance)
        {
            Debug.Log(isJumping);
            isJumping = false;
        }

        else if (Input.GetButton("Jump"))
        {
            if (isInWater)
            {
                if (Time.time > nextWaterJump)
                {
                    nextWaterJump = Time.time + (waterJumpFrames / 60f);
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0.0f, jumpForce));
                }

            }
            else
            {

                if (!isJumping && -landingTolerance <= acceleration && acceleration <= landingTolerance)
                {

                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0.0f, jumpForce));
                    isJumping = true;
                }


                else if (isJumping && acceleration < 0 && rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * (1 + accelSlowdown));
                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {

            isInWater = true;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            isInWater = false;
        }

    }

    private IEnumerator attack()
    {
        //float timeToDespawn = Time.time + (1 / 6f);
        //while (timeToDespawn > Time.time)
        // {

        // }
        Debug.Log("Attack Starts");
        attackDelay = true;
        GameObject tempHitbox = Instantiate(hitbox, gameObject.GetComponent<Transform>());
        if (sprite.flipX)
        {
            tempHitbox.transform.localPosition = new Vector2(1 / 3f, 0.04f);
        }
        else
        {
            tempHitbox.transform.localPosition = new Vector2(-1 / 3f, 0.04f);
        }

        yield return new WaitForSeconds(0.4f);
        Destroy(tempHitbox);

        yield return new WaitForSeconds(0.4f);
        attackDelay = false;
        Debug.Log("Attack Ends");
    }

}
