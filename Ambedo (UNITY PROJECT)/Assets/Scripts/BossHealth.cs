using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossHealth : MonoBehaviour
{

    private float maxHP = 300;
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


    // Use this for initialization
    void Start()
    {
        currentHp = maxHP;
        //isHealing = false;
        invincible = false;
        gameObject.GetComponent<Animator>().SetBool("Alive", true);
        cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
        room = GameObject.FindGameObjectWithTag("BossRoom");


    }

    // Update is called once per frame
    void Update()
    {

        if (currentHp < maxHP & canHealTime < Time.time)
        {
            amountToHeal = maxHP - currentHp;
            Debug.Log(currentHp);
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
        if (currentHp < 1)
        {
            StartCoroutine(killBoss());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox" & invincible == false)
        {


            knockBack(other);


            currentHp = currentHp - 100;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
        }
    }



    public void knockBack(Collider2D other)
    {

        if (gameObject.transform.position.x > other.gameObject.transform.position.x)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, 0f);

        }

        if (gameObject.transform.position.x <= other.gameObject.transform.position.x)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, 0f);
        }


     
    }

    public IEnumerator killBoss()
    {
        gameObject.GetComponent<Animator>().SetBool("Alive", false);
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        EdgeCollider2D collider2 = gameObject.GetComponent<EdgeCollider2D>();
        
      //  Debug.Log("Play Boss Death and Player Transformation Cutscene");
       // cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        // Destroy(gameObject);
        collider2.enabled = false;
        yield return new WaitForSeconds(1);


        Debug.Log("Play Boss Death and Player Transformation Cutscene");
        cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
        player = GameObject.FindGameObjectWithTag("Player");
        cam.GetComponent<CustomCamera.CameraMovement>().lowerDeadBound = 5f;
        cam.transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        cutscene.GetComponent<PlayableDirector>().Play();
      
        yield return new WaitForSeconds(4);

        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        yield return new WaitForSecondsRealtime(0.1f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation2;
        yield return new WaitForSecondsRealtime(0.5f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        yield return new WaitForSecondsRealtime(0.2f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation2;
        yield return new WaitForSecondsRealtime(0.5f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        yield return new WaitForSecondsRealtime(0.3f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation2;
        yield return new WaitForSecondsRealtime(0.5f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        yield return new WaitForSecondsRealtime(0.1f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation2;
        yield return new WaitForSecondsRealtime(0.1f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        yield return new WaitForSecondsRealtime(0.1f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation2;
        yield return new WaitForSecondsRealtime(0.1f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        yield return new WaitForSecondsRealtime(0.1f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation2;
        yield return new WaitForSecondsRealtime(0.2f);
        player.GetComponent<SpriteRenderer>().sprite = Transformation;

        player.GetComponent<PlayerHealthController>().maxHP = 300;
        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        room.GetComponent<AudioSource>().enabled = false;
        cam.GetComponent<AudioSource>().enabled = true;
        
        Destroy(gameObject);
    }
}
