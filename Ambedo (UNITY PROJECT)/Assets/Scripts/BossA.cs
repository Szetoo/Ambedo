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

    public float ChargeTime = 2f;

    void Start()
    {
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


        if (!chargeComplete & StartCharge)
        {
            charge();
            StartCharge = false;
           // Debug.Log("11111111111");
           // playerX = player.transform.position.x;
          //  playerY = player.transform.position.y;
            playerPos = player.transform.position;
        }

        if (ChargeTime <= 0)
        {
            chargeComplete = true;
        }
        else
        {
            ChargeTime -= Time.deltaTime;

        }


        if (chargeComplete)
        {

            actionComplete = Dash(playerPos);
            //Debug.Log(actionComplete);
            //Debug.Log("player:" +playerPos.x);
          //  Debug.Log("Boss:" + transform.position.x);
            if (actionComplete )
            {
                chargeComplete = false;
                StartCharge = true;
                ChargeTime = 1f;
            }

        }
    }


    private bool Dash( Vector3 playerPosition)
    {
        //bool complete = false;

        Vector3 dir = playerPosition - transform.position;
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
