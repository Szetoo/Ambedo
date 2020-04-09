using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3AI : MonoBehaviour
{
    // pubilc Object
    GameObject player;
    public GameObject Projectile;
    
    // Constant 
    private float cooldownTime = 2.0f;
    private float jumpHeight = 0.2f;

    // Spell1 Variable
    private int spell1Step = 1;
    private Vector3 groundPos;
    private GameObject firedProjectile;

    // Spell2 Variable
    private int spell2Step = 1;

    //Boss attack variable
    private int spellOrder = 1;
    private int round = 0;

    // player position
    private Transform playerPosition;

    // boss variable
    private float cooldownRemain = 3.0f;
    private int bossDirection = 1; // -1 == left 1 == right
    private bool hitground = false;
    private CircleCollider2D CircleCollider;

    // wait function variable
    private bool waitstart = true;
    private float timeWaitRemain;

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform;
        BossTurning(playerPosition.position.x); // turning
        CircleCollider = gameObject.GetComponent<CircleCollider2D>() ;
        if (CooldownReady())
            {
                AttackEvent(); // it only use 1 spell after cooldown is 0;
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
                    ResetCooldown(); // reset the cooldown after the spell is done;
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
                round = round + 1;
                spellOrder = 1;
                break;
        }

    }

    private bool Spell1(Vector3 playerPosition)
    {
        switch (spell1Step)
        {
            case 1:
                spell1Step = Jump( 2);
                break;
            case 2:
                if(Wait(1f)) spell1Step = 3;
                break;
            case 3:   
                JumpTowardPoint(gameObject.GetComponent<Rigidbody2D>(),gameObject.transform.position, playerPosition);
                spell1Step = 4;
                break;
            case 4:
                if (hitground) spell1Step = 5;
                break;
            case 5:
                ResetVec();
                spell1Step = 6;
                break;
            case 6:
                GoUnderground();
                spell1Step = 7;
                break;
            case 7:
                spell1Step = Dash(groundPos,5, 7, 8);
                break;
            case 8:
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                if (Wait(1f)) spell1Step = 9;
                break;
            case 9:
                GoUp();
                spell1Step = 10;
                break;
            case 10:
                spell1Step = Dash(groundPos, 5, 10, 11);
                break;
            case 11:
                gameObject.GetComponent<EdgeCollider2D>().enabled = true;
                gameObject.GetComponent<Rigidbody2D>().WakeUp();
                spell1Step = 12;
                break;
            case 12:
                spell1Step = Fire1(13);
                break;
            case 13:
                if (Wait(1f)) spell1Step = 14;
                break;
            case 14:
                spell1Step = 1;
                return true;
        }
        return false;
    }

    private bool Spell2(Vector3 playerPosition)
    {
        switch (spell2Step)
        {
            case 1:
                JumpTowardPoint(gameObject.GetComponent<Rigidbody2D>(),new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 12,0), playerPosition);
                spell2Step = 2;
                break;
            case 2:
                if (Wait(0.5f)) spell2Step = 3;
                break;
            case 3:
                if (hitground) spell2Step = 4;
                break;
            case 4:
                ResetVec();
                spell2Step = Fire2(5);
                break;
            case 5:
                if(Wait(1f)) spell2Step = 6 ;
                break;
            case 6:
                JumpTowardPoint(gameObject.GetComponent<Rigidbody2D>(), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 12, 0), playerPosition);
                spell2Step = 7;
                break;
            case 7:
                if (Wait(0.5f)) spell2Step = 8;
                break;
            case 8:
                if (hitground) spell2Step = 9;
                break;
            case 9:
                ResetVec();
                spell2Step = Fire3(10);
                break;
            case 10:
                spell2Step = 1;
                return true;
        }
        return false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boss2AndPlayer") hitground = true;
    }



    private int Jump(int nextStep)
    {

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0f,2500f));
        return nextStep;
    }

    private void JumpTowardPoint(Rigidbody2D rb, Vector3 from, Vector3 to)
    {
        hitground = false;
        CircleCollider.enabled = true;
        gameObject.GetComponent<Animator>().SetBool("Moving", true);
        float gravity = Physics.gravity.magnitude;
        float initialVelocity = CalculateJumpSpeed(jumpHeight, gravity);

        Vector3 direction = to - from;

        rb.AddForce(initialVelocity * direction, ForceMode2D.Impulse);
    }

    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    private void ResetVec()
    {
        hitground = false;
        CircleCollider.enabled = false;
        gameObject.GetComponent<Animator>().SetBool("Moving", false);
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
    }

    private void GoUnderground()
    {
        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        groundPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 2, 0);
    }

    private void GoUp()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        groundPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3 , 0);
    }

    private int Fire1(int nextStep)
    {
        SummorProjectile(gameObject.transform.position, 15, 1 * 8);
        SummorProjectile(gameObject.transform.position, 12, 1 * 5);
        return nextStep; 
    }

    private int Fire2(int nextStep)
    {
        SummorProjectile(gameObject.transform.position, 15, 1 * 8);
        return nextStep;
    }

    private int Fire3(int nextStep)
    {
        SummorProjectile(gameObject.transform.position, 10, 1 * 6);
        SummorProjectile(gameObject.transform.position, 10, -1 * 6);
        SummorProjectile(gameObject.transform.position, 12, 1 * 8);
        SummorProjectile(gameObject.transform.position, 12, -1 * 8);
        SummorProjectile(gameObject.transform.position, 18, 1 * 10);
        SummorProjectile(gameObject.transform.position, 18, -1 * 10);
        return nextStep;
    }

    private void SummorProjectile(Vector3 firePosition,int up,int right) // -1 right, 1 left
    {
        firedProjectile =  Instantiate(Projectile, firePosition, Quaternion.Euler(0, gameObject.transform.rotation.y, 0f));
        firedProjectile.GetComponent<Rigidbody2D>().velocity = transform.up * up + transform.right * right * -1 ;
    }


    // below are genetic boss function 
    private int Dash(Vector3 playerPosition, float speed, int step1, int step2)
    {
        gameObject.GetComponent<Animator>().SetBool("Moving", true);
        Vector3 dir = playerPosition - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= (distanceThisFrame))
        {
            gameObject.GetComponent<Animator>().SetBool("Moving", false);
            return step2;
        }
        return step1;
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
        if (cooldownRemain <= 0) return true;
        else cooldownRemain -= Time.deltaTime;
        return false;
    }


    private void ResetCooldown()
    {
        cooldownRemain = cooldownTime;
    }

    private bool Wait(float time)
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
