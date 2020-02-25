using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Playables;

public class PlayerHealthController : MonoBehaviour
{

    public float maxHP = 200;
    public float currentHp;
    private float currentEXP;
    private float maxEXP;
    private GameObject cutscene;
    private int currentLevel;

    // private bool isHealing;
    private bool invincible;

    private float invincibilityTime = 3;
    private float invincibilityExpiry;
    private float canHealTime;
    

    private float amountToHeal;
    public AudioSource damage;
    public AudioSource healthGain;
    public AudioSource absorbSound;
    public Image[] hearts;
    public Sprite[] transformations;
    public Sprite fullHeart;
    public Sprite emptyHeart;



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
        maxEXP = 200;
        currentEXP = 0;
        currentHp = maxHP;
        //isHealing = false;
        invincible = false;
        //damage.Play();
        CalculateHPCanvas();
        CalculateEXPCanvas();
        currentLevel = 1;
    }

    private void CalculateHPCanvas()
    {
        for (int i =0; i < hearts.Length; i++)
        {
            if (i < currentHp / 100)
            {
                hearts[i].sprite = fullHeart;
            } else
            {
                hearts[i].sprite = emptyHeart;
            }


            if (i < maxHP / 100)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void CalculateEXPCanvas()
    {
        GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(10).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(currentEXP/maxEXP,1,1);
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
        if (currentEXP >= maxEXP)
        {
            StartCoroutine(LevelUp());
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
            CalculateHPCanvas();
        }

        if (other.gameObject.tag == "Orb")
        {
            absorbSound.Play();
            currentEXP += 10;
            CalculateEXPCanvas();
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" & gameObject.tag == "Player" & invincible == false)
        {
            playerTakesDamage(100);
            CalculateHPCanvas();
        }

    }

    private void healPlayer(float amount)
    {
        healthGain.Play();
        currentHp += amount;
        CalculateHPCanvas();
    }

    public IEnumerator LevelUp()
    {
        currentLevel += 1;
        currentEXP = 0;
        Debug.Log("Play Boss Death and Player Transformation Cutscene");
        cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
        cutscene.GetComponent<PlayableDirector>().Play();


        yield return new WaitForSeconds(9);
        gameObject.GetComponent<SpriteRenderer>().sprite = transformations[currentLevel-1];
        gameObject.GetComponent<PlayerHealthController>().maxHP += 100;
        gameObject.GetComponent<PlayerHealthController>().currentHp += 100;
        CalculateEXPCanvas();
        CalculateHPCanvas();
        //Destroy(gameObject);
    }
}