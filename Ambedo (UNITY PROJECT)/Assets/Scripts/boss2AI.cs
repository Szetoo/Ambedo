using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2AI : MonoBehaviour
{

    GameObject player;
    GameObject camera;


    // Constant 
    private float cooldownTime = 2.0f;
    private float dashSpeed = 10f;
    private float turnSpeed = 90f;
    private float stepBackSpeed = 15f;


    // Spell1 Variable
    private bool dashToComplete = false;
    private bool turnOverComplete = true;
    private bool stepBackComplete = true;
    private int Spell1Step = 1;
    private Vector3 spell1Pos;

    // turn Variable
    float turntime = 1f;
    float timeRemain = 1f;
    private int Spell2Step = 1;

    // stepBack Varible
    Vector3 stepBackPos1;
    Vector3 stepBackPos2;



    // Spell2 Variable
    public GameObject destoryPlat;
    private GameObject destoryPlatform;
    private Vector3 PopUppos;
    private Vector3 destoryPlatPos;


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
    private int bossDirection = 1; // 1 == left 2 == right

   

    


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

        if (!playerOnBoss)
        {
            if (CooldownReady())
            {

                AttackEvent();

            }
            else
            {
                BossTurning(playerPosition.position.x);
                spell1Pos = new Vector3(playerPosition.position.x, transform.position.y, 0.0f);
            }
        }
        else
        {
            float step = 5 * Time.deltaTime;
            BossMovePos = new Vector3(playerPosition.position.x, transform.position.y, 0f);
            BossTurning(playerPosition.position.x);
            transform.position =  Vector3.MoveTowards(transform.position, BossMovePos, step);
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
                    if (round >= 1)
                    {
                        transform.Find("headCollider").gameObject.SetActive(true);
                    }

                    if (Spell2(spell1Pos))
                    {
                        spellOrder = 4;
                        if (round >= 1 && !playerOnBoss )
                         {
                            transform.Find("headCollider").gameObject.SetActive(false);
                         }
                         ResetCooldown();
                    }
                    break;
                case 4:
                    round = round + 1;
                    spellOrder = 1;
                    break;
          }

    }




    private bool Spell1(Vector3 playerPosition)
    {

        switch (Spell1Step)
        {
            case 1:
                Spell1Step = Dash(playerPosition, dashSpeed,1,2);
                break;   
            case 2:
                Spell1Step = TailChange();
                break;
            case 3:
                Spell1Step = Dash(stepBackPos2, dashSpeed, 3, 4);
                break;
            case 4:
                Spell1Step = 1;
                return true;
           
        }

        return false;
        
    }


    private bool Spell2(Vector3 playerPosition)
    {


        switch (Spell2Step)
        {
            case 1:
                CreateThePlat(playerPosition);
                Spell2Step = 2;
                break;
            case 2:
                Spell2Step = MoveUpPlatform();
                break;
            case 3:
                Spell2Step = Dash(destoryPlatPos, dashSpeed, 3, 4);
                Debug.Log(destoryPlatPos.y);
                break;
            case 4:
                Spell2Step = 1;
                return true;

        }



        return false;

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


    


    private int TailChange()
    {
        transform.Find("tailCollider").transform.rotation = Quaternion.Euler(0, 180f, 0);

        timeRemain -= Time.deltaTime;

        if (timeRemain <= 0)
        {
            transform.Find("tailCollider").transform.rotation = Quaternion.Euler(0, 0f, 0);
            timeRemain = turntime;
            FindStepPos();
            return 3;

        }
        
        return 2;

    }
    


    private void BossTurning(float playerXpos)
    {

        if ((playerXpos - transform.position.x) > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            bossDirection = -1;
        }
        if ((playerXpos - transform.position.x) < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            bossDirection = 1;
        }


    }

  

    private void FindStepPos()
    {
       // stepBackPos1 = new Vector3(4f, 1f, 0.0f) + transform.position;
        stepBackPos2 = new Vector3(bossDirection * 8f + transform.position.x, transform.position.y, transform.position.z);
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



    private void CreateThePlat(Vector3 playerPosition)
    {
        //    ShowEucationMark();
        //    HideEucationMark();
        //    summor the platform;
        //    The platform showup;


        Vector3 destoryPlatpos = new Vector3(Random.Range(-1, 2) + playerPosition.x, -12f, 0);
        destoryPlatform = Instantiate(destoryPlat, destoryPlatpos, Quaternion.Euler(0, 0f, 0f));
        PopUppos = destoryPlatform.transform.position + new Vector3(0, 3.2f, 0);

    }



    private int MoveUpPlatform()
    {
        
        if (Move(destoryPlatform.transform, PopUppos, 20))
        {
            destoryPlatPos = new Vector3(destoryPlatform.transform.position.x,transform.position.y,0f);
            return 3;
        }

        return 2;
    }





    //private bool BreakPlatform()
    //{
    //    dashToPlatform();
    //    BreakPlatform;
    //    Checkif player is onplatform


    //}

    //private void wallBreak()
    //{
    //    summor the projectile;
    //}

   

    //end of code


    private void ResetCooldown()
    {
        cooldownRemain = cooldownTime;
    }
}