using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossA : MonoBehaviour
{
    GameObject player;
    public GameObject chargeEffect;
    public GameObject fireBall;
    public Transform firePosition;
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
    private Vector3[] bossPositions = new Vector3[5];

    private Vector3 pos1 = new Vector3(278.4f, -26.63f, 0);
    private Vector3 pos2 = new Vector3(325.89f, -19.59f, 0);
    private Vector3 pos3 = new Vector3(277.48f, -12.32f, 0);
    private Vector3 pos4 = new Vector3(325.85f, -6.11f, 0);
    private Vector3 pos5 = new Vector3(281.16f, 3.06f, 0);
    int index;
    private float currentHP;

    public float ChargeTime ;


    public GameObject linePrefab;
    public GameObject currentLine;
    public LineRenderer lineRenderer;
    //public EdgeCollider2D edgeCollider;
    public List<Vector2> fingerPositions;
    public bool run1 = true;
    public Vector3 dashTo;
    void Start()
    {
        bossPos = transform.position;
       // bossPositions[0] = bossPos;
        bossPositions[0] = pos1;
        bossPositions[1] = pos2;
        bossPositions[2] = pos3;
        bossPositions[3] = pos4;
        bossPositions[4] = pos5;


        player = GameObject.FindGameObjectWithTag("Player");
        ChargeTime = 2f;
        currentLine = Instantiate(linePrefab, player.transform.position, Quaternion.identity);
        //playerNearby = false;
        //rbdy = gameObject.GetComponent<Rigidbody2D>();
        // curHeight = transform.position.y;
        //rbdy.velocity = Vector2.zero;
    }


    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHP = gameObject.GetComponent<BossHealth>().currentHp;
        if (run1) { 
              currentLine = Instantiate(linePrefab, player.transform.position, Quaternion.identity);
            run1 = false;
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
                charge();
                StartCharge = false;
                playerPos = player.transform.position;
                
                if (dash1)
                {
                    index = Random.Range(0, bossPositions.Length);
                    dashTo = bossPositions[index];
                    //spell();
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
                chargeEffect.SetActive(false);

            }
            else
            {
                ChargeTime -= Time.deltaTime;
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

            chargeEffect.SetActive(false);

        }

    }
    void CreateLine(Vector3 pos)
    {
        
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        //edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        fingerPositions.Clear();
        fingerPositions.Add(gameObject.transform.position);
        fingerPositions.Add(pos);
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        //edgeCollider.points = fingerPositions.ToArray();
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
        Vector3 rot ;
           if (index == 1 || index == 3)
        {
           rot = new Vector3(0, 0, 270f);
        }
        else
        {
            rot = new Vector3(0, 0, 90f);
        }
       // rot = new Vector3(0, 0, 90f);
        Instantiate(fireBall, firePosition.position, Quaternion.Euler(rot));
        Debug.Log("Spell");
    }


    private void charge()
    {
        //Debug.Log("Charge again");
        chargeEffect.SetActive(true);
    }
}
