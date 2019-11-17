using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{

    private float maxHP = 200;
    private float currentHp;

    // private bool isHealing;
    private bool invincible;

    private float invincibilityTime = 3;
    private float invincibilityExpiry;
    private float canHealTime;

    private float amountToHeal;
    public AudioSource damage;


    // Use this for initialization
    void Start()
    {
        currentHp = maxHP;
        //isHealing = false;
        invincible = false;
        //damage.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHp < maxHP & canHealTime < Time.time)
        {
            amountToHeal = maxHP - currentHp;
            healPlayer(amountToHeal);
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

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" & invincible == false)
        {
            currentHp = currentHp - 100;
            Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            damage.Play();
        }
        if (currentHp < 1)
        {
            damage.Play();
            Destroy(gameObject);
        }
    }

    private void healPlayer(float amount)
    {
        currentHp += amount;
    }
}