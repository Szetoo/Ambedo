using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossHealth : MonoBehaviour
{
    public int bossNumber;
    private float maxHP = 400;
    public float currentHp;
    public Sprite Transformation;
    public Sprite Transformation2;
    private GameObject player;
    private GameObject cutscene;
    private GameObject room;
    public Camera cam;

    // private bool isHealing;
    private bool invincible;

    private float invincibilityTime = 0.3f;
    private float invincibilityExpiry;
    private float canHealTime;

    private float amountToHeal;

    public GameObject orb;
    public int numberOfOrbs;
    private bool orbsHaveBeenSpawned;


    // Use this for initialization
    void Start()
    {
        currentHp = maxHP;
        //isHealing = false;
        invincible = false;
        gameObject.GetComponent<Animator>().SetBool("Alive", true);
        //cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
        room = GameObject.FindGameObjectWithTag("BossRoom");
        orbsHaveBeenSpawned = false;


    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(SpawnOrbs());
        if (currentHp < maxHP & canHealTime < Time.time)
        {
            amountToHeal = maxHP - currentHp;
            //Debug.Log(currentHp);
        }
        if (invincibilityExpiry < Time.time)
        {
            invincible = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        if (currentHp > maxHP)
        {
            currentHp = maxHP;
        }
        if (currentHp < 1 & !orbsHaveBeenSpawned)
        {
            StartCoroutine(SpawnOrbs());
            StartCoroutine(killBoss());
           // StartCoroutine(SpawnOrbs());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox" & invincible == false)
        {


            knockBack(other);


            currentHp = currentHp - 100;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
           // Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
        }
        /*if (other.gameObject.tag == "PlayerAttackHitbox" & invincible == false)
        {


            knockBack(gameObject);


            currentHp = currentHp - 100;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            // Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
        }*/
    }

    public IEnumerator SpawnOrbs()
    {
        orbsHaveBeenSpawned = true;
        float step = 2 * Time.deltaTime;
        for (int i = 0; i < numberOfOrbs; i++)
        {
            Instantiate(orb, new Vector3(gameObject.transform.position.x + Random.Range(0, 1), gameObject.transform.position.y, 0), Quaternion.identity);
        }

        //float step = 2 * Time.deltaTime;

        yield return null;



    }



    public void knockBack(Collider2D other)
    {
        //gameObject.GetComponent<Boss1AI>().enabled = false;
        //gameObject.GetComponent<ParabolaController>().enabled = false;


        if (gameObject.transform.position.x > other.gameObject.transform.position.x)
        {
            if (bossNumber == 2)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 2, gameObject.transform.position.y, 0f);
            }
            else
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 100, gameObject.transform.position.y, 0f);
            }
        }

        if (gameObject.transform.position.x <= other.gameObject.transform.position.x)
        {
            if (bossNumber == 2)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 2, gameObject.transform.position.y, 0f);
            }
            else
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 100, gameObject.transform.position.y, 0f);
            }
        }
        //gameObject.GetComponent<Boss1AI>().enabled = true;
       // gameObject.GetComponent<ParabolaController>().enabled = true;
        



    }

    public IEnumerator killBoss()
    {
        gameObject.GetComponent<Animator>().SetBool("Alive", false);
        if (bossNumber != 2)
        {
            gameObject.GetComponent<ParabolaController>().enabled = false;
            EdgeCollider2D collider2 = gameObject.GetComponent<EdgeCollider2D>();
            collider2.enabled = false;
            room.GetComponent<AudioSource>().enabled = false;
            cam.GetComponent<AudioSource>().enabled = true;
            BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }

        }
        //BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        //EdgeCollider2D collider2 = gameObject.GetComponent<EdgeCollider2D>();
        //yield return new WaitForSeconds(3);

        //  Debug.Log("Play Boss Death and Player Transformation Cutscene");
        // cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
       // for (int i = 0; i < colliders.Length; i++)
        //{
         //   colliders[i].enabled = false;
        //}
        // Destroy(gameObject);

        //collider2.enabled = false;
        yield return new WaitForSeconds(3);
        //yield return new WaitForSeconds(5);


        
        //player = GameObject.FindGameObjectWithTag("Player");
        //cam.GetComponent<CustomCamera.CameraMovement>().lowerDeadBound = 5f;
        //cam.transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        //cutscene.GetComponent<PlayableDirector>().Play();
      
        
        //room.GetComponent<AudioSource>().enabled = false;
        //cam.GetComponent<AudioSource>().enabled = true;
        
        Destroy(gameObject);
    }
}
