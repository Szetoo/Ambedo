using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2AI : MonoBehaviour
{
    // public object
    GameObject player;
    public bool playerOnBoss = false;

    // Constant 
    private float cooldownTime = 2.0f;
    private float dashSpeed = 10f;
    private float stepBackSpeed = 20f;

    // Spell1 Variable
    private int spell1Step = 1;
    private Vector3 spell1Pos;
    private Vector3 bound;

    // Spell2 Variable
    float turntime = 1f;
    float timeRemain = 1f;
    private int spell2Step = 1;

    // stepBack Varible
    Vector3 stepBackPos2;

    // Spell2 Variable
    public GameObject destoryPlat;
    private GameObject destoryPlatform;
    private Vector3 popUpPos;
    private Vector3 destoryPlatPos;
    private Vector3 destoryPlatpos;
    private Vector3 markPos;

    //Boss attack variable
    private int spellOrder = 1;
    private int round = 0;
    Vector3 bossMovePos;

    // player position
    private Vector3 playerPosition;

    // boss current HP
    private float cooldownRemain = 3.0f;
    private int bossDirection = 1; 


    // wait variable
    private bool waitstart = true;
    private float timeWaitRemain;

    // exclamation variable
    public GameObject exclamation;
    private GameObject exclamationMark;


    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = Setbound(player.transform.position);
 
        if (!playerOnBoss)
        {
            if (CooldownReady())  AttackEvent();
            else
            {
                BossTurning(playerPosition.x); // Check turning
                spell1Pos = new Vector3(playerPosition.x, transform.position.y, 0.0f);
            }
        }
        else
        {
            playerPosition = player.transform.position;
            float step = 10 * Time.deltaTime;
            bossMovePos = new Vector3(playerPosition.x, transform.position.y, 0f);
            BossTurning(playerPosition.x);
            transform.position =  Vector3.MoveTowards(transform.position, bossMovePos, step);
        }
    }


    void AttackEvent()
    {
        switch (spellOrder)
         {
                case 1:
                    if (Spell1(spell1Pos))
                    {
                        spellOrder = 2;
                        ResetCooldown();
                    }   
                     break;
                case 2:
                    if (Spell1(spell1Pos))
                    {
                        spellOrder = 3;
                        ResetCooldown();
                     }
                    break;
                case 3:
                    if (round >= 1) transform.Find("headCollider").gameObject.SetActive(true);
                    
                    if (Spell2(spell1Pos))
                    {
                        spellOrder = 4;
                        if (round >= 1 && !playerOnBoss )transform.Find("headCollider").gameObject.SetActive(false);
                         ResetCooldown();
                    }
                    if (playerOnBoss) if (destoryPlatform != null) Destroy(destoryPlatform, 1f);
                    break;
                case 4:
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
                bound = Setbound(playerPosition);
                spell1Step = Dash(bound, dashSpeed,1,2);
                break;   
            case 2:
                spell1Step = TailChange();
                break;
            case 3:
                 spell1Step = Dash(stepBackPos2, stepBackSpeed, 3, 4);
                break;
            case 4:
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
                SummorExclamation(playerPosition);
                spell2Step = 2;
                break;
            case 2:
                if (Wait(1.75f))spell2Step = 3;
                break;
            case 3:
                CreateThePlat(playerPosition);
                spell2Step = 4;
                break;
            case 4:
                spell2Step = MoveUpPlatform();
                break;
            case 5:
                destoryPlatPos = destoryPlatPos + new Vector3(bossDirection * 4 , 0, 0);
                Debug.Log(destoryPlatPos.x);
                spell2Step = 6;
                break;
            case 6:
                spell2Step = Dash(destoryPlatPos, dashSpeed, 6, 7);
                break;  
            case 7:
                Destroy(destoryPlatform);
                spell2Step = 1;
                return true;
        }
        return false;
    }

    private Vector3 Setbound(Vector3 targetPosition)
    {
        Vector3 result = targetPosition;


        if (targetPosition.x > 391) result = new Vector3(390.5f, targetPosition.y, 0);
        else if (targetPosition.x < 364) result = new Vector3(364, targetPosition.y, 0);

        return result;
    }

    private int TailChange()
    {

        timeRemain -= Time.deltaTime;

        if (timeRemain <= 0)
        {
            timeRemain = turntime;
            FindStepPos();
            return 3;
        }
        return 2;
    }

    private void FindStepPos()
    {
        stepBackPos2 = new Vector3(bossDirection * 8f + transform.position.x, transform.position.y, transform.position.z);
    }

    private void CreateThePlat(Vector3 playerPosition)
    {
        destoryPlatform = Instantiate(destoryPlat, destoryPlatpos, Quaternion.Euler(0, 0f, 0f));
        Destroy(destoryPlatform, 4f);
        popUpPos = destoryPlatform.transform.position + new Vector3(0, 3.2f, 0);
    }

    private int MoveUpPlatform()
    {
        if (Move(destoryPlatform.transform, popUpPos, 20))
        {
            destoryPlatPos = new Vector3(destoryPlatform.transform.position.x, transform.position.y, 0f);
            return 5;
        }
        return 4;
    }

    private void SummorExclamation(Vector3 playerPosition)
    {
        destoryPlatpos = new Vector3(Random.Range(-1, 2) + playerPosition.x, -68f, 0);
        markPos = destoryPlatpos - new Vector3(1f, 11f, 0f);
        exclamationMark = Instantiate(exclamation, markPos, Quaternion.Euler(0, 0f, 0f));
        Destroy(exclamationMark, 2f);
    }

    // below are genetic boss function
    private int Dash(Vector3 playerPosition, float speed, int step1, int step2)
    {
        gameObject.GetComponent<Animator>().SetBool("Moving", true);
        Vector3 dir = playerPosition - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= (distanceThisFrame + 0.1f))
        {
            gameObject.GetComponent<Animator>().SetBool("Moving", false);
            return step2;
        }
        return step1;
    }


    private bool Move(Transform theObject, Vector3 targetPosition, float speed)
    {

        gameObject.GetComponent<Animator>().SetBool("Moving", true);
        Vector3 dir = targetPosition - theObject.position;

        float distanceThisFrame = speed * Time.deltaTime;

        theObject.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= distanceThisFrame)
        {
            gameObject.GetComponent<Animator>().SetBool("Moving", false);
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
            bossDirection = -1;
        }
        if ((playerXpos - transform.position.x) < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.GetChild(1).transform.localPosition = new Vector3(0f, 0.1f, -49f);
            bossDirection = 1;
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
        gameObject.GetComponent<Animator>().SetBool("Moving", false);

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