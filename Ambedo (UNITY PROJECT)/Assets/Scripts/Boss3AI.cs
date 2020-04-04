using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3AI : MonoBehaviour
{

    GameObject player;
    GameObject camera;
    public GameObject Projectile;
    

    // Constant 
    private float cooldownTime = 2.0f;
    private float dashSpeed = 10f;
    private float turnSpeed = 90f;
    private float stepBackSpeed = 20f;
    private float jumpHeight = 0.2f;


    // Spell1 Variable
    private int Spell1Step = 1;
    private Vector3 spell1Pos;
    private Vector3 groundPos;
    private GameObject firedProjectile;

    // turn Variable
    float turntime = 1f;
    float timeRemain = 1f;
    private int Spell2Step = 1;

    // stepBack Varible
    Vector3 stepBackPos2;



    // Spell2 Variable
    public GameObject destoryPlat;
    private GameObject destoryPlatform;
    private Vector3 PopUppos;
    private Vector3 destoryPlatPos;
    private Vector3 destoryPlatpos;
    private Vector3 MarkPos;


    //Boss attack variable
    public int spellOrder = 1;
    private int round = 0;
    public bool playerOnBoss = false;
    Vector3 BossMovePos;


    // player position
    private Transform playerPosition;


    // boss current HP
    private float currentHP;
    private float cooldownRemain = 3.0f;
    public int bossDirection = 1; // -1 == left 1 == right


    // wait variable
    private bool waitstart = true;
    private float timeWaitRemain;

    // exclamation variable
    public GameObject exclamation;
    private GameObject exclamationMark;



    void Start()
    {


        // Update is called once per frame
    }
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform;
        currentHP = gameObject.GetComponent<BossHealth>().currentHp;


        // turning

        BossTurning(playerPosition.position.x);

        if (CooldownReady())
            {

                AttackEvent();

            }
        
    }




    void AttackEvent()
    {

        switch (spellOrder)
        {
            case 1:
                if (Spell1(player.transform.position))
                {
                    spellOrder = 2;
                    ResetCooldown();
                }
                break;
            case 2:
                if (Spell2(player.transform.position))
                {
                    spellOrder = 3;
                    ResetCooldown();
                }
                break;
            case 3:
                //round = round + 1;
                spellOrder = 1;
                break;
        }

    }




    private bool Spell1(Vector3 playerPosition)
    {

        switch (Spell1Step)
        {
            case 1:
                Spell1Step = Jump(1, 2);
                break;
            case 2:
                if(wait(1f)) Spell1Step = 3;
                break;
            case 3:
                JumpTowardPoint(gameObject.GetComponent<Rigidbody2D>(),gameObject.transform.position, playerPosition);
                Spell1Step = 4;
                break;
            case 4:
                if (gameObject.transform.position.y < -27.5) Spell1Step = 5;
                break;
            case 5:
                resetVec();
                Spell1Step = 6;
                break;
            case 6:
                GoUnderground();
                Spell1Step = 7;
                break;
            case 7:
                Spell1Step = Dash(groundPos,5, 7, 8);
                break;
            case 8:
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                if (wait(1f)) Spell1Step = 9;
                break;
            case 9:
                GoUp();
                Spell1Step = 10;
                break;
            case 10:
                Spell1Step = Dash(groundPos, 5, 10, 11);
                break;
            case 11:
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                gameObject.GetComponent<Rigidbody2D>().WakeUp();
                Spell1Step = 12;
                break;
            case 12:
                Spell1Step = fire1(13);
                break;
            case 13:
                if (wait(1f)) Spell1Step = 14;
                break;
            case 14:
                return true;
 
        }

        return false;

    }


    private bool Spell2(Vector3 playerPosition)
    {


        switch (Spell2Step)
        {
            case 1:
                JumpTowardPoint(gameObject.GetComponent<Rigidbody2D>(),new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 12,0), playerPosition);
                Spell2Step = 2;
                break;
            case 2:
                if (wait(0.5f)) Spell2Step = 3;
                break;
            case 3:
                if (gameObject.transform.position.y < -27.5) Spell2Step = 4;
                break;
            case 4:
                resetVec();
                Spell2Step = fire2(5);
                break;
            case 5:
                if(wait(1f)) Spell2Step = 6 ;
                break;
            case 6:
                JumpTowardPoint(gameObject.GetComponent<Rigidbody2D>(), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 12, 0), playerPosition);
                Spell2Step = 7;
                break;
            case 7:
                if (wait(0.5f)) Spell2Step = 8;
                break;
            case 8:
                if (gameObject.transform.position.y < -27.6) Spell2Step = 9;
                break;
            case 9:
                resetVec();
                Spell2Step = fire3(10);
                break;
            case 10:
                return true;
        }

        return false;

    }


    private int Jump(int step1, int step2)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0f,2500f));
       // rb.MovePosition(desiredPosition);
        return step2;
    }

    void JumpTowardPoint(Rigidbody2D rb, Vector3 from, Vector3 to)
    {
        float gravity = Physics.gravity.magnitude;
        float initialVelocity = CalculateJumpSpeed(jumpHeight, gravity);

        Vector3 direction = to - from;

        rb.AddForce(initialVelocity * direction, ForceMode2D.Impulse);
    }






    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }


    private void resetVec()
    {

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
    }


    private void GoUnderground()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        groundPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 2, 0);

    }

    private void GoUp()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        groundPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3 , 0);

    }

     private int fire1(int step)
    {
        summorProjectile(gameObject.transform.position, 15, 1 * 8);
        summorProjectile(gameObject.transform.position, 12, 1 * 5);
        return step; 

    }

    private int fire2(int step)
    {
        summorProjectile(gameObject.transform.position, 15, 1 * 8);
        return step;
    }

    private int fire3(int step)
    {
        summorProjectile(gameObject.transform.position, 10, 1 * 6);
        summorProjectile(gameObject.transform.position, 10, -1 * 6);
        summorProjectile(gameObject.transform.position, 12, 1 * 8);
        summorProjectile(gameObject.transform.position, 12, -1 * 8);
        summorProjectile(gameObject.transform.position, 18, 1 * 10);
        summorProjectile(gameObject.transform.position, 18, -1 * 10);
        return step;
    }



    private void summorProjectile(Vector3 firePosition,int up,int right) // -1 right 1 left
    {
        firedProjectile =  Instantiate(Projectile, firePosition, Quaternion.Euler(0, gameObject.transform.rotation.y, 0f));

        firedProjectile.GetComponent<Rigidbody2D>().velocity = transform.up * up + transform.right * right * -1 ;

        //Vector3 from = firePosition - new Vector3(0, up,0);
        //Vector3 to = player.transform.position;
        //JumpTowardPoint(firedProjectile.GetComponent<Rigidbody2D>(), from, to);
    }



    private int Dash(Vector3 playerPosition, float speed, int step1, int step2)
    {
        Vector3 dir = playerPosition - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= (distanceThisFrame))
        {
            return step2;
        }

        return step1;
    }


    private bool Move(Transform theObject, Vector3 targetPosition, float speed)
    {
        Vector3 dir = targetPosition - theObject.position;

        float distanceThisFrame = speed * Time.deltaTime;

        theObject.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= distanceThisFrame)
        {
            return true;
        }

        return false;
    }


    private void BossTurning(float playerXpos)
    {

        if ((playerXpos - transform.position.x) > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            gameObject.transform.GetChild(1).transform.localPosition = new Vector3(0f, 0.1f, 49f);
            gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 0);
            bossDirection = 1;
        }
        if ((playerXpos - transform.position.x) < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.GetChild(1).transform.localPosition = new Vector3(0f, 0.1f, -49f);
            bossDirection = -1;
        }
    }




    private bool CooldownReady()
    {

        if (cooldownRemain <= 0)
        {
            return true;
        }
        else
        {
            cooldownRemain -= Time.deltaTime;
        }

        return false;
    }


    private void ResetCooldown()
    {
        cooldownRemain = cooldownTime;
    }


    private bool wait(float time)
    {
        if (waitstart)
        {
            waitstart = false;
            timeWaitRemain = time;
        }

        timeWaitRemain -= Time.deltaTime;
        if (timeWaitRemain <= 0)
        {
            waitstart = true;
            return true;
        }
        return false;
    }

    //end of code
}
