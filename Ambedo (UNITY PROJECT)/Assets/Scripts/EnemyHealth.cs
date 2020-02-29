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
    public GameObject orb;
    public int numberOfOrbs;

    [HideInInspector]
    public float currentHP;

    private bool invincible;
    private float invincibilityExpiry;
    private float canHealTime;
    private bool orbsHaveBeenSpawned;


    private const float healRate = 0.1f;
    
    // Use this for initialization
    void Start()
    {
        currentHP = maxHP;
        invincible = false;
        gameObject.GetComponent<Animator>().SetBool("Alive", true);
        orbsHaveBeenSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(SpawnOrbs());
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
        if (currentHP < 1 & !orbsHaveBeenSpawned)
        {
            
            StartCoroutine(KillEnemy());
            StartCoroutine(SpawnOrbs());

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OnTriggerEnterCall from EnemyHealth");
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
            StartCoroutine(KillEnemy());
            return;
        }
        goInvincible(invDuration);
        canHealTime = Time.time + timeUntilHeal;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
    }

    public IEnumerator KillEnemy()
    {
        gameObject.GetComponent<Animator>().SetBool("Alive", false);
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
}
