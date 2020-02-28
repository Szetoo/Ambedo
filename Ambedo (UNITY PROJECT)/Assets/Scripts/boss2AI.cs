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
    private float stepBackSpeed = 150f;


    // Spell1 Variable
    private bool dashToComplete = false;
    private bool turnOverComplete = true;
    private bool stepBackComplete = true;
    private Vector3 spell1Pos;

    // turn Variable
    bool turnForward = true;
    bool turnBackward = false;

    // stepBack Varible
    Vector3 stepBackPos1;
    Vector3 stepBackPos2;
    int currentStep = 1;

    // player position
    private Transform playerPosition;

    

    // boss current HP
    private float currentHP;
    private float cooldownRemain = 3.0f;
    private int bossDirection = 1; // 1 == left 2 == right

    // fire ball animation
    public GameObject fireBall;
    // fire ball position at the starting point
    public Transform firePosition;

    // Camera Position
    public Vector3 camPos;
    public float camX = 16;
    public float camY = 9;

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
        

        if (cooldownReady())
        {
            if (Spell1(spell1Pos))
            {
                cooldownRemain = cooldownTime;
            }
            
        }
        else
        {
            bossTurning(playerPosition.position.x);
            spell1Pos = new Vector3(playerPosition.position.x, transform.position.y, 0.0f);
        }


    }

    private bool Spell1(Vector3 playerPosition)
    {
        if (!dashToComplete)
        {
            dashToComplete = Dash(playerPosition,dashSpeed);
            turnOverComplete = !dashToComplete;
        }
        else if (!turnOverComplete)
        {
            turnOverComplete = turnOver();
            stepBackComplete = !turnOverComplete;
       

        }
        else if (!stepBackComplete)
        {
            stepBackComplete = stepBack();
            dashToComplete = !stepBackComplete;
            return true;
        }

        return false;
    }



    private bool Dash(Vector3 playerPosition, float speed)
    {
        Vector3 dir = playerPosition - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= distanceThisFrame)
        {
            return true;
        }

        return false;
    }


    private bool turnOver()
    {
        float step = turnSpeed * Time.deltaTime;

        if (bossDirection == 1)
        {
           if (turnForward)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180f, 0), step);
                if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180f))
                 {  
                    turnForward = false;
                    turnBackward = true;
                 }
            }
            if (turnBackward)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0f, 0), step);
                if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0))
                {
                    turnForward = true;
                    turnBackward = false;
                    findStepPos();
                    return true;
                }
            }

            return false;
        }


        if (bossDirection == 2)
        {
            if (turnForward)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0f, 0), step);
                if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0f))
                {
                    turnForward = false;
                    turnBackward = true;
                }
            }
            if (turnBackward)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180f, 0), step);
                if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180f))
                {
                    turnForward = true;
                    turnBackward = false;
                    findStepPos();
                    return true;
                }
            }

            return false;
        }



        return false;
    }


    private bool stepBack()
    {
        if (currentStep == 1)
        {
            if (Dash(stepBackPos1,stepBackSpeed))
            {
                currentStep = 2;
            }
        }
        else if (currentStep == 2)
        {
            if (Dash(stepBackPos2,stepBackSpeed))
            {
                currentStep = 1;
                return true;
            }

        }



        return false;
    }




    private void bossTurning(float playerXpos)
    {

        if (playerXpos - transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            bossDirection = 2;
        }
        if (playerXpos - transform.position.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            bossDirection = 1;
        }


    }


    private void findCameraPos()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        camPos = camera.transform.position;
    }


    private void findStepPos()
    {
        stepBackPos1 = new Vector3(4f, 1f, 0.0f) + transform.position;
        Debug.Log("Step 1:"+ stepBackPos1.x);
        stepBackPos2 = new Vector3(8f, 0.0f, 0.0f) + transform.position;
    }


    private bool cooldownReady()
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



    //end of code
}