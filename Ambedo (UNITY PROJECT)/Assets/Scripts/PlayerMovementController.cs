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
    public bool isWielding = true;
    
    private bool isJumping;
    private Vector2 lastVelocity;
    private bool canStartSprint;
    private bool isInWater;
    private bool isTouchingLadder;
    private bool isClimbing;
    private float nextWaterJump;
    private SpriteRenderer sprite;
    private float jumpForceConstant;


    private float horizontalAxis;
    private float verticalAxis;
    private bool jumpButton;

    void Awake()
    {

        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.Log("Reading Save File");
            // 2
            // player = GameObject.FindGameObjectWithTag("Player");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3

            float xPosition = save.xSpawnPosition;
            float yPosition = save.ySpawnPosition;
            isWielding = save.isWielding;

            Debug.Log(xPosition);
            Debug.Log(yPosition);

            //gameObject.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, 0);
            Debug.Log("Game Loaded");

            //Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
        if (isWielding)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }


    void Start()
    {
        isJumping = false;
        isClimbing = false;
        lastVelocity = new Vector2(0.0f, 0.0f);
        canStartSprint = true;
        sprite = GetComponent<SpriteRenderer>();
        jumpForceConstant = jumpForce;
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        jumpButton = Input.GetButton("Jump");
        //Debug.Log(isJumping);

        if (jumpButton || verticalAxis != 0)
        {
            //Debug.Log(verticalAxis);
            if (verticalAxis != 0 && isTouchingLadder)
            {
                ladderMovement(rb, verticalAxis);
            }
            else if (jumpButton)
            {
                jump(rb, jumpButton);

            }

        }

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
        if (!jumpButton & !isClimbing)
        {
        verticalMovement(rb, verticalAxis);

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
        /*if (isJumping || isInWater)
        {
            //if (sprintHorizontal == 0)
            canStartSprint = false;
            if (rb.velocity.x == 0)
            {
                momentum = 0;
                if (jumpButton)
                    momentum = moveHorizontal;
            }
        }
        else
            canStartSprint = true;*/

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
        }
    }

    public void jump(Rigidbody2D rb, bool jump)
    {
        float acceleration = (rb.velocity.y - lastVelocity.y) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;

        //Water physics stuff
        /*if (isInWater)
        {
            if (Time.time > nextWaterJump)
            {
                nextWaterJump = Time.time + (waterJumpFrames / 60f);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0.0f, jumpForce));
            }
        }

        if
        {*/
            //Executes if player is eligible to jump
            if (!isJumping & !isTouchingLadder & -landingTolerance <= acceleration & acceleration <= landingTolerance)
            //if (!isJumping & !isTouchingLadder)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0.0f, jumpForce));
                isJumping = true;
            }

            //Executes if player is already jumping
            else if (isJumping & acceleration < 0 & rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * (1 + accelSlowdown));
            }
        
    }

    public void ladderMovement(Rigidbody2D rb, float verticalAxis)
    {
        if (isTouchingLadder) { 
            Vector2 movement = new Vector2(rb.position.x, rb.position.y + (climbSpeed * verticalAxis));

            rb.transform.position = movement;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {

            isInWater = true;
        }

        else if(other.tag == "Ladder")
        {
            isTouchingLadder = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else if(other.tag == "Platform")
        {
            isJumping = false;
        }
        else if(other.tag == "Ramp")
        {
            isJumping = false;
            jumpForce = 250;
        }
        else if (other.tag == "Untagged")
        {
            jumpForce = jumpForceConstant;
        }
    }



    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Platform")
        {
            isJumping = false;
        }
        else if (other.tag == "Ramp")
        {
            isJumping = false;
            jumpForce = 250;
            Debug.Log(jumpForce);
        }
        else if (other.tag == "Untagged")
        {
            jumpForce = jumpForceConstant;
        }
        else if (isTouchingLadder)
        {
            isClimbing = verticalAxis != 0;
        }
        else
        {
            isClimbing = false;
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
            rb.velocity = new Vector2(rb.velocity.x, -(landingTolerance+0.001f));
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            isClimbing = false;
            isTouchingLadder = false;
            Debug.Log("Stopped climbing ladder");
            isJumping = false;
        }
        

    }

}