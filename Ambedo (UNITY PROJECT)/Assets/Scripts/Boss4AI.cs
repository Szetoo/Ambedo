using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4AI : MonoBehaviour
{

    GameObject player;
    public GameObject fireBall;
    public GameObject fireBall2;

    // Constant
    private float cooldownTime = 1f;
    private float dashSpeed = 8f;

    // dashTo position
    public Vector3 dashTo;

    // Spell1 Variable
    private int Spell1Step = 1;

    // Spell2 Variable
    private int Spell2Step = 1;
    private Vector3 firePosition;


    // Spell3 Variable
    private int Spell3Step = 1;
    private GameObject fireBallObject1;
    private GameObject fireBallObject2;
    private GameObject fireBallObject3;
    private GameObject fireBallObject4;
    private Vector3 dir1, dir2, dir3, dir4;
    private float fireballSpeed = 13f;

    // Escape Variable
    int puzzleCode = 0;

    //Boss attack variable
    public int spellOrder = 1;
    private int round = 0;

    // player position
    private Transform playerPosition;

    // boss current HP
    private float cooldownRemain = 0.4f;
    private int bossDirection = 1; // 1 == left 2 == right


    // Camera Position
    public Vector3 camPos;
    public float camX = 16;
    public float camY = 6;

    //wait variable
    private bool waitstart = true;
    private float timeRemain;

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPosition = player.transform;
            BossTurning(playerPosition.position.x);
        }

        if (CooldownReady()) AttackEvent(playerPosition.position);
    }

    void AttackEvent(Vector3 PlayerPosition)
    {
        switch (spellOrder)
        {
            case 1:
                if (Spell1(PlayerPosition))
                {
                    spellOrder = 2;
                    ResetCooldown();
                }
                break;
            case 2:
                if (Spell2(PlayerPosition))
                {
                    spellOrder = 3;
                    ResetCooldown();
                }
                break;
            case 3:
                if (Spell3(PlayerPosition))
                {
                    spellOrder = 1;
                    ResetCooldown();
                }
                break;
            case 4:
                round = round + 1;
                spellOrder = 1;
                break;

        }
    }


    private bool Spell1(Vector3 PlayerPosition)
    {

        switch (Spell1Step)
        {
            case 1:
                dashTo = PlayerPosition;
                Spell1Step = 2;
                break;
            case 2:
                Spell1Step = Dash(dashTo, dashSpeed, 2, 3);
                break;
            case 3:
                Spell1Step = 4;
                dashTo = PlayerPosition + new Vector3(10 * bossDirection, 0, 0);
                break;
            case 4:
                Spell1Step = Dash(dashTo, dashSpeed, 4, 5);
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Spell1Step = 6;
                break;
            case 6:
                Spell1Step = 1;
                return true;
        }
        return false;
    }

    private bool Spell2(Vector3 PlayerPosition)
    {
        switch (Spell2Step)
        {
            case 1:
                gameObject.transform.position =  new Vector3(PlayerPosition.x, -18, 0);
                Spell2Step = 2;
                break;
            case 2:
                if (wait(1.0f)) Spell2Step = 3;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                Spell2Step = 4;
                dashTo = new Vector3(PlayerPosition.x, -18, 0);
                break;
            case 4:
                Spell2Step = Dash(dashTo, dashSpeed, 4, 5);
                break;
            case 5:
                summorFireball();
                Spell2Step = 6;
                break;
            case 6:
                if (wait(0.2f))
                {
                    Spell2Step = 7;
                    summorFireball();
                }
                break;
            case 7:
                if (wait(0.3f))
                {
                    Spell2Step = 8;
                    summorFireball();
                }
                break;
            case 8:
                if (wait(0.4f))
                {
                    Spell2Step = 9;
                    summorFireball();
                }
                break;
            case 9:
                if (wait(1.5f)) Spell2Step = 10;
                break;
            case 10:
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Spell2Step = 11;
                break;
            case 11:
                Spell2Step = 1;
                return true;
        }
        return false;
    }

    private bool Spell3(Vector3 PlayerPosition)
    {
        switch (Spell3Step)
        {
            case 1:
                gameObject.transform.position = new Vector3(PlayerPosition.x + 5 * bossDirection, -18, 0);
                Spell3Step = 2;
                break;
            case 2:
                if (wait(0.4f)) Spell3Step = 3;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                Spell3Step = 4;
                break;
            case 4:
                summorflyingfireball();
                Spell3Step = 5;
                break;
            case 5:
                MoveAll(1);
                if (wait(1.0f))
                {
                    dir1 = PlayerPosition - fireBallObject1.transform.position;
                    Spell3Step = 6;
                }
                break;
            case 6:
                MoveAll(2);
                if (wait(1.0f))
                {
                    dir2 = PlayerPosition - fireBallObject2.transform.position;
                    Spell3Step = 7;
                }
                break;
            case 7:
                MoveAll(3);
                if (wait(1.0f))
                {
                    dir3 = PlayerPosition - fireBallObject3.transform.position;
                    Spell3Step = 8;
                }
                break;
            case 8:
                MoveAll(4);
              
                if (wait(1.0f))
                {
                    dir4 = PlayerPosition - fireBallObject4.transform.position;
                    Spell3Step = 9;
                }
                break;
            case 9:
                MoveAll(5);
                if (wait(4.0f))
                { 
                    Spell3Step = 10;
                }
                break;
            case 10:
                Spell3Step = 1;
                return true;
        }
        return false;
    }


    private void summorFireball()
    {
        float angle;
        for (int i = 0; i < 4; i++)
        {
            firePosition = transform.position;
            angle = -60f + ((float)i * 30f);
            Instantiate(fireBall, firePosition, Quaternion.Euler(0, 0f,  Random.Range(angle, angle + 25)));
         }
    }

   

    private void summorflyingfireball()
    {
        firePosition = transform.position + new Vector3(0,2.5f,0);
        fireBallObject1 =Instantiate(fireBall2, firePosition, Quaternion.Euler(0, 0f, 270f));
        firePosition = transform.position + new Vector3(0, -2.5f, 0);
        fireBallObject2 = Instantiate(fireBall2, firePosition, Quaternion.Euler(0, 0f, 90f));
        firePosition = transform.position + new Vector3(2.5f, 0, 0);
        fireBallObject3 = Instantiate(fireBall2, firePosition, Quaternion.Euler(0, 0f, 180f));
        firePosition = transform.position + new Vector3(-2.5f, 0, 0);
        fireBallObject4 = Instantiate(fireBall2, firePosition, Quaternion.Euler(0, 0f, 0f));


    }

    private void MoveFireBall(GameObject target)
    {
        if (target != null) target.transform.RotateAround(gameObject.transform.position, Vector3.forward, 120* Time.deltaTime);
    }

    private void MoveAll(int num)
    {
        switch (num)
        {
            case 1:
                MoveFireBall(fireBallObject1);
                MoveFireBall(fireBallObject2);
                MoveFireBall(fireBallObject3);
                MoveFireBall(fireBallObject4);
                break;
            case 2:
                moveToward(dir1, fireballSpeed, fireBallObject1);
                MoveFireBall(fireBallObject2);
                MoveFireBall(fireBallObject3);
                MoveFireBall(fireBallObject4);
                break;
            case 3:
                moveToward(dir1, fireballSpeed, fireBallObject1);
                moveToward(dir2, fireballSpeed, fireBallObject2);
                MoveFireBall(fireBallObject3);
                MoveFireBall(fireBallObject4);
                break;
            case 4:
                moveToward(dir1, fireballSpeed, fireBallObject1);
                moveToward(dir2, fireballSpeed, fireBallObject2);
                moveToward(dir3, fireballSpeed, fireBallObject3);
                MoveFireBall(fireBallObject4);
                break;
            case 5:
                moveToward(dir1, fireballSpeed, fireBallObject1);
                moveToward(dir2, fireballSpeed, fireBallObject2);
                moveToward(dir3, fireballSpeed, fireBallObject3);
                moveToward(dir4, fireballSpeed, fireBallObject4);
                break;
        }
           
    }

    public void puzzleFunction(int code)
    {
        switch (puzzleCode)
        {
            case 1:
                if (code == 2) puzzleCode = code;
                else puzzleCode = 0;
                break;
            case 2:
                if (code == 3) puzzleCode = code;
                else puzzleCode = 0;
                break;
            case 3:
                if (code == 4) puzzleCode = code;
                else puzzleCode = 0;
                break;
            case 4:
                if (code == 5) puzzleCode = code;
                else puzzleCode = 0;
                break;
            case 5:
                if (code == 6) Destroy(gameObject, 0.5f);
                else puzzleCode = 0;
                break;

        }
        if (code == 1) puzzleCode = code;


        Debug.Log("Puzzle Code:" + puzzleCode);
    }

    private void moveToward(Vector3 dir, float speed, GameObject Origin)
    {
        if (Origin != null)
        {
            float distanceThisFrame = speed * Time.deltaTime;

            Origin.transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }

    }

    // below are genetic boss function 
    private int Dash(Vector3 targetPosition, float speed, int step1, int step2)
    {
        Vector3 dir = targetPosition - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= (distanceThisFrame))
        {
            return step2;
        }

        return step1;
    }


    private int move(Vector3 targetPosition, float speed, Transform Origin,int step1, int step2)
    {
        Vector3 dir = targetPosition - Origin.position;

        float distanceThisFrame = speed * Time.deltaTime;

        Origin.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= (distanceThisFrame))
        {
            return step2;
        }

        return step1;
    }

    private void ResetCooldown()
    {
        cooldownRemain = cooldownTime;
    }


    private bool CooldownReady()
    {
        if (cooldownRemain <= 0) return true;
        else cooldownRemain -= Time.deltaTime;
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

    private bool wait(float time)
    {
        if (waitstart)
        {
            waitstart = false;
            timeRemain = time;
        }

        timeRemain -= Time.deltaTime;
        if (timeRemain <= 0)
        {
            waitstart = true;
            return true;
        }
        return false;
    }

    // end of code 
}