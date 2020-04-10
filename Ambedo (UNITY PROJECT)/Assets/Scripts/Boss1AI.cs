using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ParabolaController;

public class Boss1AI : MonoBehaviour
{

    // public object
    GameObject player;
    GameObject camera;
    public GameObject parabolaRoot;
    public GameObject fireBall;

    // Constant
    private float cooldownTime = 1f;
    private float dashSpeed = 25f;

    // Spell1 Variable
    private int spell1Step = 0;

    // Spell2 Variable
    private int spell2Step = 1;
    private Vector3 firePosition;

    // Boss attack variable
    private int spellOrder = 1;
    private int round = 0;
    Vector3 bossOriginPos;
    Vector3 bossflytoPos;

    // player position
    private Transform playerPosition;

    // boss variables
    private float cooldownRemain = 0.4f;
    private int bossDirection = 1;  //1 is from right to left, -1 is from left to right

    // Parabola variable
    protected ParabolaFly parabolaFly;

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
        
        if (CooldownReady()) AttackEvent(playerPosition.position, bossOriginPos, bossflytoPos);
        else
        {
            bossOriginPos = parabolaRoot.transform.GetChild(2).transform.position;
            bossflytoPos = bossOriginPos + new Vector3(0f, 20f, 0f);
        }
    }

    void AttackEvent(Vector3 PlayerPosition, Vector3 BossOriginPosition, Vector3 BossFlyto)
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
                if (Spell2(BossOriginPosition, BossFlyto))
                {
                    spellOrder = 3;
                    ResetCooldown();
                }
                break;
            case 3:
                round++;
                spellOrder = 1;
                break;
        }
    }


    private bool Spell1(Vector3 PlayerPosition)
    {
        switch (spell1Step)
        {
            case 0:
                FindCameraPos();
                UpdatePosition(parabolaRoot, 1, PlayerPosition);
                spell1Step = CheckStop(0, 1);
                break;
            case 1:
                spell1Step = Dash(parabolaRoot.transform.GetChild(0).transform.position, dashSpeed, 1, 2);
                break;
            case 2:
                ParabolaFlying();
                spell1Step = 3 ;
                break;
            case 3:
                FindCameraPos();
                UpdatePosition(parabolaRoot, -1, PlayerPosition);
                spell1Step = CheckStop(3, 4);
                break;
            case 4:
                spell1Step = Dash(parabolaRoot.transform.GetChild(0).transform.position, dashSpeed, 4, 5);
                break;
            case 5:
                ParabolaFlying();
                spell1Step = 6;
                break;
            case 6:
                spell1Step = CheckStop(6, 7);
                break;
            case 7:
                spell1Step = 0;
                return true;
        }
        return false;
    }

    private bool Spell2(Vector3 BossOriginPosition, Vector3 BossFlyto)
    {
        switch (spell2Step)
        {
            case 1:
                spell2Step = Dash(BossFlyto, 18f, 1, 2);
                break;
            case 2:
                spell2Step = SummorFireball();
                break;
            case 3:
                if (Wait(1.0f))spell2Step = 4;
                break;
            case 4:
                spell2Step = Dash(BossOriginPosition, 10f, 4, 5);
                break;
            case 5:
                spell2Step = 1;
                spell2Step = SummorFireball();
                return true;
        }
        return false;
    }

    private int SummorFireball()
    {
        for (int i = 0; i < 13;  i ++ )
        {
            firePosition = new Vector3(274f + (i - 1)* 5.2f + Random.Range(0f, 5.2f), 4.3f, 0f);
            Instantiate(fireBall, firePosition, Quaternion.Euler(0, 0f, 0f) );
        }
        return 3;
    }

    private void ParabolaFlying()
    {
        transform.GetComponent<ParabolaController>().FollowParabola();
    }

    private int CheckStop(int step1, int step2)
    {
        if(transform.GetComponent<ParabolaController>().Animation == false) return step2;
        
        return step1;
    }

    private void FindCameraPos()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        camPos = new Vector3(camera.transform.position.x, camera.transform.position.y, 0f) ;
    }

    private void UpdatePosition(GameObject parabolaRoot, int dir, Vector3 playerPosition)
    {
        // 1 is from right to left, -1 is from left to right
        parabolaRoot.transform.GetChild(0).transform.position = camPos + new Vector3(dir * camX, camY, 0f);
        parabolaRoot.transform.GetChild(1).transform.position = playerPosition;
        parabolaRoot.transform.GetChild(2).transform.position = camPos + new Vector3(-1 * dir * camX, camY, 0f);

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
            gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0,0, 0);
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

    private bool Wait(float time)
    {
        if (waitstart)
        {
            waitstart = false;
            timeRemain = time;
        }

        timeRemain  -= Time.deltaTime;
        if (timeRemain <= 0)
        {
            waitstart = true;
            return true;
        }
        return false;
    }



}