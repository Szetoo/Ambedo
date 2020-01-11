using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerHealthController : MonoBehaviour
{

    public int maxHP;
    public int lightDamage;
    public int healAmount;
    public float invDuration;
    public float lightInvDuration;
    
    public AudioSource damage;

    [HideInInspector]
    public float currentHP;

    private const float healRate = 0.1f;
    private float invincibilityExpiry;
    private float canHealTime;
    private float amountToHeal;

    private bool inLight;
    private bool invincible;

    // Use this for initialization
    /*
    private void Awake()
    {
        
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.Log("Reading Save File");
            // 2
            // player = GameObject.FindGameObjectWithTag("Player");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3

            float xPosition = save.xSpawnPosition;
            float yPosition = save.ySpawnPosition;
            Debug.Log(xPosition);
            Debug.Log(yPosition);

            gameObject.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, 0);
            Debug.Log("Game Loaded");

            //Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }*/
    void Start()
    {
        currentHP = maxHP;
        invincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP < maxHP & canHealTime < Time.time)
        {
            healPlayer(healAmount);
            canHealTime = Time.time + healRate;
        }
        if (invincibilityExpiry < Time.time)
        {
            invincible = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        if (inLight & !invincible)
        {
            damagePlayer(lightDamage, lightInvDuration);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox")
        {
            return;
        }

        if ((other.gameObject.tag == "Enemy" | other.gameObject.tag == "Boss") & invincible == false)
        {
            Debug.Log(gameObject.tag);
            damagePlayer(100);  //Constant 100 for now, will change depending on which enemy is doing damage
            Debug.Log(currentHP);
           
        }

        if (other.tag == "LightZone")
        {
            inLight = true;
            Debug.Log("Enter Light");
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "LightZone")
        {
            inLight = false;
            Debug.Log("Exit Light");
        }
    }

    private void healPlayer(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void damagePlayer(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            killPlayer();
            return;
        }
        goInvincible(invDuration);
        canHealTime = Time.time + 6f;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        damage.Play();
    }

    public void damagePlayer(float amount, float invDuration)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            killPlayer();
            return;
        }
        goInvincible(invDuration);
        canHealTime = Time.time + 6f;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        damage.Play();
    }

    private void goInvincible(float duration)
    {
        invincible = true;
        invincibilityExpiry = Time.time + duration;
    }

    private void killPlayer()
    {
        damage.Play();
        Destroy(gameObject);
        Initiate.Fade("Level-alex-tim", Color.black, 1.0f);
    }
}