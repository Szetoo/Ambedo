using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    private float maxHP = 100;
    private float currentHp;
    private bool invincible;

    private float invincibilityTime = 3;
    private float invincibilityExpiry;
    private float canHealTime;

    private float amountToHeal;


    void Start()
    {
        currentHp = maxHP;
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
            StartCoroutine(KillEnemy());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If enemy touches player attack hit box, take damage
        if (other.gameObject.tag == "PlayerAttackHitbox" & invincible == false)
        {
            currentHp = currentHp - 100;
            Debug.Log(currentHp);
            invincible = true;
            invincibilityExpiry = Time.time + invincibilityTime;
            canHealTime = invincibilityExpiry + 6;
        }       
    }

    //On death, play animation then get destroyed after 1 second
    public IEnumerator KillEnemy()
    {
        gameObject.GetComponent<Animator>().SetBool("Alive", false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
