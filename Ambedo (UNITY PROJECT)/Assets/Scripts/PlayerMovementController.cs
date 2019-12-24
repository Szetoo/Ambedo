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

        float horizontalAxis = Input.GetAxis("Horizontal");
        bool jumpButton = Input.GetButton("Jump");

        //Runs when pressing a button that moves left or right
        if (horizontalAxis != 0)
        {
            horizontalMovement(rb, horizontalAxis);

        }

        //Stops the player if they're still moving after they shouldn't be 
        else if (rb.velocity.x != 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //Falling physics of player. MUST RUN BEFORE JUMP CALL.
        if (!jumpButton)
        {
            verticalMovement(rb);

        }

        //Runs only when the jump button is being pressed/held
        else
        {
            jump(rb, jumpButton);

        }
    }

    private void LateUpdate()
    {
        try
        {
            if (Input.GetMouseButtonDown(0))
            {
                string heldObjectTag = gameObject.transform.GetChild(0).name;
                if (heldObjectTag == "Sword" && !attackDelay)
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

    public void horizontalMovement(Rigidbody2D rb, float moveHorizontal)
    {
        /*
         * Note: Make sure sprintHorizontal stays at 0. Even though we don't use sprinting, this function requires
         * a lot of checks of this variable.
        */
        float momentum = xSpeed;

        //float sprintHorizontal = Input.GetAxis ("Sprint");
        float sprintHorizontal = 0;


        //Water physics. Irrelevant to us for now.
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

        //Moving to the left
        if (moveHorizontal < 0)
        {
            sprintHorizontal = -sprintHorizontal;
            sprite.flipX = false;
            transform.localScale.Set(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //momentum = -xSpeed;
        }

        //Stationary (I'm particularly worried that this won't work, but I can't test it)
        else if (moveHorizontal == 0)
        {
            sprite.flipX = sprite.flipX;
        }

        //Moving to the right
        else
        {
            sprite.flipX = true;
            transform.localScale.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }


        //More sprinting stuff
        if (!(Input.GetButton("Horizontal") && canStartSprint) || isInWater)
            sprintHorizontal = 0;

        //Create the movement vector and apply it to the rigidBody2D
        Vector2 movement = new Vector2(
            (moveHorizontal * momentum) + (sprintHorizontal * sprintMultiplier),
            rb.velocity.y);

        rb.velocity = movement;


    }

    public void verticalMovement(Rigidbody2D rb)
    {

        float acceleration = (rb.velocity.y - lastVelocity.y) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;

        if (isJumping && -landingTolerance <= rb.velocity.y && rb.velocity.y <= landingTolerance)
        {
            isJumping = false;
        }


    }

    public void jump(Rigidbody2D rb, bool jump)
    {
        float acceleration = (rb.velocity.y - lastVelocity.y) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;

        //Water physics stuff
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
            //Executes if player is eligible to jump
            if (!isJumping && -landingTolerance <= acceleration && acceleration <= landingTolerance)
            {
                Debug.Log("Jump!");
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0.0f, jumpForce));
                isJumping = true;
            }

            //Executes if player is already jumping
            else if (isJumping && acceleration < 0 && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * (1 + accelSlowdown));
                Debug.Log("Holding Jump");
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {

            isInWater = true;
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            isInWater = false;
        }

    }

    public IEnumerator attack()
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
