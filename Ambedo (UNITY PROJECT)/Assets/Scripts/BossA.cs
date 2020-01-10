using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossA : MonoBehaviour
{
    GameObject player;
    private float leftBoundX = 277.6f;
    private float rightBoundX = 324.2f;
    private float TopBoundY = 5;
    private float BottomBoundY = -24.3f;
    private bool chargeComplete = false;
    private bool StartCharge = true;
    private bool actionComplete = false;
    private float playerX;
    private float playerY;
    private float speed = 20;
    private Vector3 playerPos;
    private Vector3 bossPos;
    private bool dash1 = false;
    private bool dash2 = true;
    private Vector3[] bossPositions = new Vector3[3];

    private Vector3 pos1 = new Vector3(289, -18, 0);
    private Vector3 pos2 = new Vector3(316, -24, 0);
    private Vector3 pos3 = new Vector3(309, -10, 0);
    int index;

    public float ChargeTime = 2f;

    void Start()
    {
        bossPos = transform.position;
        bossPositions[0] = bossPos;
        bossPositions[1] = pos1;
        bossPositions[2] = pos2;
        bossPositions[3] = pos3;
        player = GameObject.FindGameObjectWithTag("Player");
        //playerNearby = false;
        //rbdy = gameObject.GetComponent<Rigidbody2D>();
        // curHeight = transform.position.y;
        //rbdy.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //<charging>
        if (!chargeComplete & StartCharge)
        {
            charge();
            StartCharge = false;
            playerPos = player.transform.position;
            index = Random.Range(0, bossPositions.Length);
        }

        if (ChargeTime <= 0)
        {
            chargeComplete = true;
        }
        else
        {
            ChargeTime -= Time.deltaTime;

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
                    ChargeTime = 4f;
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
                    ChargeTime = 10f;
                    dash1 = true;
                    dash2 = false;
                }
            }

        }
        



    }


    private bool Dash( Vector3 playerPosition)
    {
        //bool complete = false;

        Vector3 dir = playerPosition - transform.position;
        if(playerPosition.x - transform.position.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (playerPosition.x - transform.position.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        float distanceThisFrame = speed * Time.deltaTime;
        
        transform.Translate(dir.normalized *distanceThisFrame, Space.World);
        // transform.LookAt(player.transform);

        Debug.Log("mag:" + dir.magnitude);
        Debug.Log("distanceThisFrame:" + distanceThisFrame);
        if (dir.magnitude <= distanceThisFrame )
       // if (Mathf.Abs(Mathf.Abs(playerPosition.x) - Mathf.Abs(transform.position.x ))<= distanceThisFrame*1.5)
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

    }


    private void charge()
    {
        //Debug.Log("Charge again");

    }
}
