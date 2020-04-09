using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.Playables;

//This script manages all player health related behaviours such as Level, HP, MaxHP, Current Level, EXP and death.
public class PlayerHealthController : MonoBehaviour
{

    public int maxHP;
    public int lightDamage;
    public int healAmount;
    public float invDuration;
    public float lightInvDuration;
    public float currentEXP;
    private float maxEXP;
    private GameObject cutscene;
    public int currentLevel;

    public AudioSource damage;
    public AudioSource healthGain;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public AudioSource absorbSound;
    public Sprite[] transformations;

    [HideInInspector]
    public float currentHP;

    private const float healRate = 0.1f;
    private float invincibilityExpiry;
    private float canHealTime;
    private float amountToHeal;
    private int currentGameLevel;

    private bool inLight;
    private bool invincible;
    private bool isLevelingUp;

    // Use this for initialization
    
     void Awake()
    {
        
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            Debug.Log(save.currentLevel);
            currentGameLevel = save.saveFileLevel;
            currentEXP = save.currentEXP;
            currentLevel = save.currentLevel;
            maxHP = (save.currentLevel + 2) * 100;
            currentHP = maxHP;
            Debug.Log(maxHP);
            invincible = false;
            isLevelingUp = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = transformations[currentLevel];



            //Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
            currentLevel = 1;
            currentHP = maxHP;
            invincible = false;
            isLevelingUp = false;
        }
        
    }
    void Start()
    {

        maxEXP = 100;

        CalculateHPCanvas();
        CalculateEXPCanvas();

 
    }



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
        if (currentEXP >= maxEXP & !isLevelingUp)
        {
            isLevelingUp = true;
            StartCoroutine(LevelUp());
            
        }

    }

    private void CalculateHPCanvas()
    {

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHP / 100)
            {
                hearts[i].sprite = fullHeart;
            }
            else
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
        GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(10).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(currentEXP / maxEXP, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Orb")
        {
            
            Destroy(other.gameObject);
            absorbSound.Play();
            //Destroy(other.gameObject);
            currentEXP += 5;
            SaveToGame();
            CalculateEXPCanvas();
        }

        if (other.gameObject.tag == "PlayerAttackHitbox")
        {
            return;
        }

        else if ((other.gameObject.tag == "Enemy" | other.gameObject.tag == "Boss") & invincible == false)
        {
           // Debug.Log(gameObject.tag);
            damagePlayer(100);  //Constant 100 for now, will change depending on which enemy is doing damage
           // Debug.Log(currentHP);
            CalculateHPCanvas();


        }

        else if (other.tag == "LightZone")
        {
            damage.Play();
            inLight = true;
           // Debug.Log("Enter Light");
            CalculateHPCanvas();
        }
       


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox")
        {
            return;
        }

        if ((other.gameObject.tag == "Enemy" | other.gameObject.tag == "Boss") & invincible == false)
        {
           // Debug.Log(gameObject.tag);
            damagePlayer(100);  //Constant 100 for now, will change depending on which enemy is doing damage
           // Debug.Log(currentHP);
            CalculateHPCanvas();


        }

        if (other.tag == "LightZone")
        {
            damage.Play();
            inLight = true;
         //   Debug.Log("Enter Light");
            CalculateHPCanvas();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "LightZone")
        {
            inLight = false;
           // Debug.Log("Exit Light");
        }

    }
    public IEnumerator LevelUp()
    {
        Debug.Log(currentLevel);
        currentLevel++;
        gameObject.GetComponent<PlayerHealthController>().maxHP += 100;
        gameObject.GetComponent<PlayerHealthController>().currentHP += 100;
        float leftOverEXP = currentEXP - maxEXP;
        currentEXP = maxEXP - 0.1f;
        //float leftOverEXP = currentEXP - maxEXP;
        
        CalculateEXPCanvas();
        currentEXP = leftOverEXP;
        //Debug.Log("Play Boss Death and Player Transformation Cutscene");
        cutscene = GameObject.FindGameObjectWithTag("BossDeathCutscene");
        cutscene.GetComponent<PlayableDirector>().time = 0;
        cutscene.GetComponent<PlayableDirector>().Play();
        //cutscene.GetComponent<PlayableDirector>().playableAsset


        yield return new WaitForSeconds(8);
        Debug.Log(currentLevel);
        gameObject.GetComponent<SpriteRenderer>().sprite = transformations[currentLevel];
        //gameObject.GetComponent<PlayerHealthController>().maxHP += 100;
        //gameObject.GetComponent<PlayerHealthController>().currentHP += 100;
        //gameObject.GetComponent<SpriteRenderer>().sprite = transformations[currentLevel];
        CalculateEXPCanvas();
        CalculateHPCanvas();
        isLevelingUp = false;
        SaveToGame();
        //cutscene.GetComponent<PlayableDirector>().
        //Destroy(gameObject);
    }

    private void healPlayer(float amount)
    {

        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        CalculateHPCanvas();
    }

    public void damagePlayer(float amount)
    {
        if (!invincible)
        {
            currentHP = currentHP - amount;
            canHealTime = Time.time + 6f;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            goInvincible(invDuration);
            damage.Play();
            
        }
    
        
        if (currentHP <= 0)
        {
            killPlayer();
            return;
        }
        CalculateHPCanvas();


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
        CalculateHPCanvas();
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
        if (currentGameLevel == 1)
        {
            Initiate.Fade("Level-alex-tim", Color.black, 1.0f);
        }
        else if (currentGameLevel == 2)
        {
            Initiate.Fade("level 2", Color.black, 1.0f);
        }
        else if (currentGameLevel == 3)
        {
            Initiate.Fade("level 3", Color.black, 1.0f);
        }
        else if (currentGameLevel == 4)
        {
            Initiate.Fade("Orphanage", Color.black, 1.0f);
        }


        //Initiate.Fade("Level-alex-tim", Color.black, 1.0f);
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
        float curEXP = currentEXP;
        int curLevel = currentLevel;
        bool trigger = save.cameraPanHasBeenActivated;






        //Save save2 = CreateSaveGameObject(gameLevel, xPosition, yPosition, isWielding, enemies, curEXP, curLevel);
        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2, enemies3, curEXP, curLevel, trigger);

        // 2
        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        bf.Serialize(file, save2);
        file.Close();

        //Unpause();


    }

   
}