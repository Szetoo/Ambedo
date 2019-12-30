using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    private float maxHP = 100;
    private float currentHp;
    public Sprite Transformation;
    private GameObject player;

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
        //isHealing = false;
        invincible = false;
        gameObject.GetComponent<Animator>().SetBool("Alive", true);


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
            currentHp = currentHp - 100;
            Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
        }
    }

    private void healPlayer(float amount)
    {
        currentHp += amount;
    }

    public IEnumerator killBoss()
    {
        gameObject.GetComponent<Animator>().SetBool("Alive", false);
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
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
