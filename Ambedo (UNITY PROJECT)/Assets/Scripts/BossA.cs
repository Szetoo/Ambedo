using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossA : MonoBehaviour
{

    GameObject player;


    // charge and dash control

    // charge effect game object
    public GameObject chargeEffect;
    public Transform chargePosition;
    private GameObject ChargeObject;

    // charge bool value
    private bool chargeComplete = false;
    private bool StartCharge = true;
    private bool actionComplete = false;

    // dash turn
    private bool dash1 = false;
    private bool dash2 = true;
    public bool firstDash = true;

    // dashTo position
    public Vector3 dashTo;

    // charge speed
    private float speed = 20;

    // charge time
    public float ChargeTime = 2f;


    // fire ball
    // fire ball animation
    public GameObject fireBall;
    // fire ball position at the starting point
    public Transform firePosition;


    // player position
    private Vector3 playerPos;

    // dash 2 turn boss positions
    private Vector3[] bossPositions = new Vector3[5];

    private Vector3 pos1 = new Vector3(278.4f, -26.63f, 0);
    private Vector3 pos2 = new Vector3(325.89f, -19.59f, 0);
    private Vector3 pos3 = new Vector3(277.48f, -12.32f, 0);
    private Vector3 pos4 = new Vector3(325.85f, -6.11f, 0);
    private Vector3 pos5 = new Vector3(281.16f, 3.06f, 0);


    // boss position index selection
    int index;

    // boss current HP
    private float currentHP;


    //line rendenrer
    public GameObject linePrefab;
    public GameObject currentLine;
    public LineRenderer lineRenderer;

    
    
    void Start()
    {
        bossPositions[0] = pos1;
        bossPositions[1] = pos2;
        bossPositions[2] = pos3;
        bossPositions[3] = pos4;
        bossPositions[4] = pos5;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        currentHP = gameObject.GetComponent<BossHealth>().currentHp;

        // draw lines in first dash

        if (firstDash) {
            currentLine = Instantiate(linePrefab, player.transform.position, Quaternion.identity);
            firstDash = false;
        }

        // turning
        if (player.transform.position.x - transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        if (player.transform.position.x - transform.position.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        if (currentHP > 0) {
            //<charging>
            if (!chargeComplete & StartCharge)
            {
                charge(ChargeTime);
                StartCharge = false;
                playerPos = player.transform.position;
                
                if (dash1)
                {
                    index = Random.Range(0, bossPositions.Length);
                    dashTo = bossPositions[index];
                }
                else if (dash2)
                {
                    
                    dashTo = playerPos;
                    spell();
                }
            }

            if (ChargeTime <= 0)
            {
                chargeComplete = true;
                

            }
            else
            {
                ChargeTime -= Time.deltaTime;
                ChargeObject.transform.position = chargePosition.position;
                CreateLine(dashTo);


            }
            //</charging>
            
            if (chargeComplete)
            {
                if (dash1)
                {

                    actionComplete = Dash(bossPositions[index]);
                    if (actionComplete)
                    {
                        chargeComplete = false;
                        StartCharge = true;
                        ChargeTime = 3f;
                        dash1 = false;
                        dash2 = true;
                    }
                }

                if (dash2)
                {
                    actionComplete = Dash(playerPos);

                    if (actionComplete)
                    {
                        chargeComplete = false;
                        StartCharge = true;
                        ChargeTime = 5f;
                        dash1 = true;
                        dash2 = false;
                    }
                }

            }


        }
        else{

        }

    }
    void CreateLine(Vector3 pos)
    {
        
        lineRenderer = currentLine.GetComponent<LineRenderer>();      
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, pos);
    }


    private bool Dash( Vector3 playerPosition)
    {
        Vector3 dir = playerPosition - transform.position;
     
        float distanceThisFrame = speed * Time.deltaTime;
        
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        if (dir.magnitude <= distanceThisFrame )
        {
            return true;
        }

        return false;
    }

    private void Throw()
    {


    }

    private void spell()
    {
        Vector3 rot;

        if (index == 1 || index == 3)
        {
           rot = new Vector3(0, 0, 270f);
        }
        else
        {
            rot = new Vector3(0, 0, 90f);
        }

        Instantiate(fireBall, firePosition.position, Quaternion.Euler(rot));

    }


    private void charge( float destoryTime)
    {
        ChargeObject = Instantiate(chargeEffect, chargePosition.position, chargePosition.rotation);
        Destroy(ChargeObject, destoryTime);
    
    }
}
