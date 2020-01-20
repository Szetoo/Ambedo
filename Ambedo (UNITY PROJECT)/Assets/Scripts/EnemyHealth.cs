using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHP;
    public int enemyDamageFromPlayer;
    public int healAmount;
    public float invDuration;
    public float timeUntilHeal;
    
    [HideInInspector]
    public float currentHP;

    private bool invincible;
    private float invincibilityExpiry;
    private float canHealTime;

    private const float healRate = 0.1f;
    
    // Use this for initialization
    void Start()
    {
        currentHP = maxHP;
        invincible = false;
        gameObject.GetComponent<Animator>().SetBool("Alive", true);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHP < maxHP & canHealTime < Time.time)
        {
            healEnemy(healAmount);
            canHealTime = Time.time + healRate;
        }
        if (invincibilityExpiry < Time.time)
        {
            invincible = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);

        } 

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnterCall from EnemyHealth");
        if (other.gameObject.tag == "PlayerAttackHitbox" & invincible == false)
        {
            damageEnemy(enemyDamageFromPlayer);
        }

    }

    private void healEnemy(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void damageEnemy(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            StartCoroutine(killEnemy());
            return;
        }
        goInvincible(invDuration);
        canHealTime = Time.time + timeUntilHeal;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
    }

    public IEnumerator killEnemy()
    {
        gameObject.GetComponent<Animator>().SetBool("Alive", false);
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        //gameObject.GetComponent<Rigidbody2D>().enabled = false;
       // gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private void goInvincible(float duration)
    {
        invincible = true;
        invincibilityExpiry = Time.time + duration;
    }
}
