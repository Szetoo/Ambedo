using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerHealthController : MonoBehaviour
{

    private float maxHP = 200;
    public float currentHp;

    // private bool isHealing;
    private bool invincible;

    private float invincibilityTime = 3;
    private float invincibilityExpiry;
    private float canHealTime;

    private float amountToHeal;
    public AudioSource damage;


    // Use this for initialization
    private void Awake()
    {
        /*
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
        }*/
    }
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
        if (currentHp < 1)
        {
            damage.Play();

            Destroy(gameObject);
            Initiate.Fade("Corey_Scene", Color.black, 1.0f);
        }
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

    private void playerTakesDamage(int damageTaken)
    {
        currentHp = currentHp - damageTaken;
        Debug.Log(currentHp);
        invincible = true;
        invincibilityExpiry = Time.time + invincibilityTime;
        canHealTime = invincibilityExpiry + 6;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        damage.Play();



    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" & gameObject.tag == "Player" & invincible == false)
        {
            playerTakesDamage(100);
        }
     
    }

    private void healPlayer(float amount)
    {
        currentHp += amount;
    }
}