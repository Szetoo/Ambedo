using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerHealthController : MonoBehaviour
{

    private float maxHP = 200;
    public float currentHP;

    // private bool isHealing;
    private bool invincible;
    
    private float invincibilityExpiry;
    private float canHealTime;

    private float amountToHeal;
    public AudioSource damage;

    private bool inLight;

    // Use this for initialization
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
    }
    void Start()
    {
        currentHP = maxHP;
        //isHealing = false;
        invincible = false;
        //damage.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP < maxHP & canHealTime < Time.time)
        {
            healPlayer(2);
            canHealTime = Time.time + 0.1f;
            Debug.Log(currentHP);
        }
        if (invincibilityExpiry < Time.time)
        {
            invincible = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        if (inLight & !invincible)
        {
            damagePlayer(5, 0.2f);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox")
        {
            return;
        }

        if (other.gameObject.tag == "Enemy" & invincible == false)
        {
            Debug.Log(gameObject.tag);
            damagePlayer(100);
            Debug.Log(currentHP);
           
        }

        if (other.tag == "LightZone")
        {
            inLight = true;
            Debug.Log("In Light");
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

    private void damagePlayer(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            killPlayer();
            return;
        }
        goInvincible(3);
        canHealTime = Time.time + 6f;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        damage.Play();
    }

    private void damagePlayer(float amount, float invincibility)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            killPlayer();
            return;
        }
        goInvincible(invincibility);
        canHealTime = Time.time + 6f;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        damage.Play();
    }

    private void goInvincible(float invincibilityTime)
    {
        invincible = true;
        invincibilityExpiry = Time.time + invincibilityTime;
    }

    private void killPlayer()
    {
        damage.Play();
        Destroy(gameObject);
        Initiate.Fade("Corey_Scene", Color.black, 1.0f);
    }
}