﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class EnemyHealth : MonoBehaviour
{

    public int maxHP;
    public int enemyDamageFromPlayer;
    public int healAmount;
    public float invDuration;
    public float timeUntilHeal;
    public GameObject orb;
    public int numberOfOrbs;
    private int currentGameLevel;
    [HideInInspector]
    public float currentHP;

    private bool invincible;
    private float invincibilityExpiry;
    private float canHealTime;
    private bool orbsHaveBeenSpawned;
    


    private const float healRate = 0.1f;

    
    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            currentGameLevel = save.saveFileLevel;
            //Debug.Log(currentGameLevel);
            Dictionary<string, bool> enemies = save.enemiesInLevel1;
            Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
            Dictionary<string, bool> enemies3 = save.enemiesInLevel3;
            string enemyName = gameObject.name;
            //Debug.Log(enemyName);
            //Debug.Log("Here are the enemies in Level 2: ");
            //Debug.Log(enemies2);
            foreach(KeyValuePair<string, bool> kvp in enemies2)
            {
               // Debug.Log( kvp.Key + kvp.Value);
            }
            if (currentGameLevel == 1) {
                if (enemies[enemyName] == false)
                {
                    gameObject.SetActive(false);
                }
            }
            else if(currentGameLevel == 2)
            {
                //Debug.Log("We are in level two and we are checking for:" + enemyName);
               
                if (enemies2[enemyName] == false)
                {
                    gameObject.SetActive(false);
                }
            }
            else if (currentGameLevel == 3)
            {
               // Debug.Log("We are in level two and we are checking for:" + enemyName);

                if (enemies3[enemyName] == false)
                {
                    gameObject.SetActive(false);
                }
            }

            //Dictionary<string, bool> enemies = save.enemiesInLevel1;
            /*
            string enemyName = gameObject.name;
            Debug.Log(enemyName);
            if (enemies[enemyName] == false)
            {
                gameObject.SetActive(false);
            }*/
        }
        else
        {
            Debug.Log("No game saved!");
        }

    }

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
        SaveToGame();
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

    private void SaveToGame()
    {
        
            // Debug.Log("Reading Save File");
            // 2
            // player = GameObject.FindGameObjectWithTag("Player");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

        // 3
       
        
            int gameLevel = save.saveFileLevel;
            float xPosition = save.xSpawnPosition;
            float yPosition = save.ySpawnPosition;
            bool isWielding = save.isWielding;
            Dictionary<string, bool> enemies = save.enemiesInLevel1;
            Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
            Dictionary<string, bool> enemies3 = save.enemiesInLevel3;
            float currentEXP = save.currentEXP;
            int currentLevel = save.currentLevel;
            bool trigger = save.cameraPanHasBeenActivated; 

        if (currentGameLevel == 1)
        {
            string enemyName = gameObject.name;
            enemies[enemyName] = false;
        }
        else if(currentGameLevel == 2)
        {
            string enemyName = gameObject.name;
            enemies2[enemyName] = false;
        }
        else if(currentGameLevel == 3)
        {
            string enemyName = gameObject.name;
            enemies3[enemyName] = false;
        }
        


           
            //gameObject.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, 0);
            Debug.Log("Game Loaded");

        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2, enemies3, currentEXP, currentLevel, trigger);

        // 2
        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        bf.Serialize(file, save2);
        file.Close();

        //Unpause();


    }
    /*private Save CreateSaveGameObject(int level, float xPosition, float yPosition, bool isWielding, Dictionary<string, bool> enemies, Dictionary<string, bool> enemies2, float currentEXP, int currentLevel)
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");
        save.saveFileLevel = level;
        save.xSpawnPosition = xPosition;
        save.ySpawnPosition = yPosition;
        save.isWielding = isWielding;
        save.enemiesInLevel1 = enemies;
        save.enemiesInLevel2 = enemies2;
        save.currentEXP = currentEXP;
        save.currentLevel = currentLevel;

        return save;
    }*/
}
