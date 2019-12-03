using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    private float maxHP = 200;
    public float currentHp;

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

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox" & invincible == false)
        {
            receiveDamage(100);
            Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
        }

        if (currentHp < 1)
        {
            EnemyDie();
        }
    }


    public void healEnemy(float amount)
    {
        currentHp += amount;
    }

    public void receiveDamage(float amount)
    {
        currentHp -= amount;
    }

    public void EnemyDie()
    {
        Destroy(gameObject);
    }


}