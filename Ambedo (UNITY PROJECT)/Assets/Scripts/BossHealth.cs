using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossHealth : MonoBehaviour
{

    private float maxHP = 100;
    private float currentHp;
    public Sprite Transformation;
    private GameObject player;
    private GameObject cutscene;

    // private bool isHealing;
    private bool invincible;

    private float invincibilityTime = 3;
    private float invincibilityExpiry;
    private float canHealTime;

    private float amountToHeal;


    // Use this for initialization
    void Start()
    {
        currentHp = maxHP;
        invincible = false;
        cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");

    }

    // Update is called once per frame
    void Update()
    {
        //Conditions for healing
        if (currentHp < maxHP & canHealTime < Time.time)
        {
            amountToHeal = maxHP - currentHp;
            Debug.Log(currentHp);
        }

        //If invincibility has expired, set to false
        if (invincibilityExpiry < Time.time)
        {
            invincible = false;
        }

        //If healing puts health over max, set back to max Hp
        if (currentHp > maxHP)
        {
            currentHp = maxHP;
        }
        if (currentHp < 1)
        {
            StartCoroutine(KillBoss());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox" & invincible == false)
        {
            currentHp = currentHp - 100;
            Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
        }
    }

    public IEnumerator KillBoss()
    {
        gameObject.GetComponent<Animator>().SetBool("Alive", false);
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Debug.Log("Play Boss Death Cutscene");
        cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
        cutscene.GetComponent<PlayableDirector>().Play();
        
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        yield return new WaitForSeconds(1);
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<SpriteRenderer>().sprite = Transformation;
        Destroy(gameObject);
    }
}
