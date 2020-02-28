using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerMovementController : MonoBehaviour
{

    public float xSpeed;
    public float jumpForce;
    public float accelSlowdown;
    public float landingTolerance;
    public float sprintMultiplier;
    public float climbSpeed;
    public int waterJumpFrames;
    
    private bool isJumping;
    private Vector2 lastVelocity;
    private bool canStartSprint;
    private bool isInWater;
    private bool isTouchingLadder;
    private bool isClimbing;
    private float nextWaterJump;
    private SpriteRenderer sprite;
    public bool isWielding;
    
    //void Awake()
    //{
        
    //    if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //    {
    //        Debug.Log("Reading Save File");
    //        // 2
    //        // player = GameObject.FindGameObjectWithTag("Player");
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //        Save save = (Save)bf.Deserialize(file);
    //        file.Close();

    //        // 3

    //        float xPosition = save.xSpawnPosition;
    //        float yPosition = save.ySpawnPosition;
    //        isWielding = save.isWielding;

    //        Debug.Log(xPosition);
    //        Debug.Log(yPosition);

    //        gameObject.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, 0);
    //        Debug.Log("Game Loaded");

    //        //Unpause();
    //    }
    //    else
    //    {
    //        Debug.Log("No game saved!");
    //    }
    //    if (isWielding)
    //    {
    //        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    //    }
    //}

    void Start()
    {
       
        isJumping = false;
        isClimbing = false;
        lastVelocity = new Vector2(0.0f, 0.0f);
        canStartSprint = true;
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
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


        if (gameObject.GetComponent<Rigidbody2D>().velocity.y < -10)
        {
            jumpForce = 300;
        }
        else
        {
            jumpForce = 180;
        }

        //Falling physics of player. MUST RUN BEFORE JUMP CALL.
        if (!jumpButton & !isClimbing)
        {
        verticalMovement(rb, verticalAxis);

        }



        //Runs only when the jump button is being pressed/held
        else if (verticalAxis != 0)
        {
           // Debug.Log(verticalAxis);
            if (isTouchingLadder)
            {
                ladderMovement(rb, verticalAxis);
            }
            else
            {
                jump(rb, jumpButton);

            }

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

    public void verticalMovement(Rigidbody2D rb, float moveVertical)
    {
        float acceleration = (rb.velocity.y - lastVelocity.y) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;

        if (isJumping && -landingTolerance <= rb.velocity.y && rb.velocity.y <= landingTolerance)
        {
            isJumping = false;
            jumpForce = 180;
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
            if (!isJumping & !isTouchingLadder & -landingTolerance <= acceleration & acceleration <= landingTolerance)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0.0f, jumpForce));
                isJumping = true;
                if (gameObject.GetComponent<Rigidbody2D>().velocity.y < 5)
                {
                    jumpForce = 400;
                }
            }

            //Executes if player is already jumping
            else if (isJumping && acceleration < 0 && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * (1 + accelSlowdown));
            }
        }
    }

    public void ladderMovement(Rigidbody2D rb, float verticalAxis)
    {
        Vector2 movement = new Vector2(rb.position.x, rb.position.y + (climbSpeed * verticalAxis));

        rb.transform.position = movement;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {

            isInWater = true;
        }

        if (other.tag == "Ladder")
        {
            isTouchingLadder = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        
    }

    

    public void OnTriggerStay2D(Collider2D other)
    {
        if (!isClimbing & other.tag == "Ladder" & Input.GetAxis("Vertical") != 0)
        {
            isClimbing = true;
          //  Debug.Log("Climbing Ladder");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            isInWater = false;
        }

        else if (other.tag == "Ladder")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            //rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            isClimbing = false;
            isTouchingLadder = false;
            //Debug.Log("Stopped climbing ladder");
        }

    }

}