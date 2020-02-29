using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.Playables;

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

    private bool inLight;
    private bool invincible;
    private bool isLevelingUp;

    // Use this for initialization
    
     void Awake()
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

            currentEXP = save.currentEXP;
            currentLevel = save.currentLevel;
            maxHP = (currentLevel + 2) * 100;
            currentHP = maxHP;
            invincible = false;
            isLevelingUp = false;



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
       // currentEXP = 0;
        maxEXP = 20;
       // currentHP = maxHP;
        //invincible = false;
        CalculateHPCanvas();
        CalculateEXPCanvas();
        //currentLevel = 1;
       // isLevelingUp = false;
 
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentEXP);
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
        //gameObject.GetComponent<SpriteRenderer>().sprite = transformations[currentLevel - 1];
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
            currentEXP += 10;
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
        currentLevel += 1;
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
        gameObject.GetComponent<SpriteRenderer>().sprite = transformations[currentLevel-1];
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
        Initiate.Fade("Level-alex-tim", Color.black, 1.0f);
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

        float xPosition = save.xSpawnPosition;
        float yPosition = save.ySpawnPosition;
        bool isWielding = save.isWielding;
        Dictionary<string, bool> enemies = save.enemiesInLevel1;
        float curEXP = currentEXP;
        int curLevel = currentLevel;






        Save save2 = CreateSaveGameObject(xPosition, yPosition, isWielding, enemies, curEXP, curLevel);

        // 2
        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        bf.Serialize(file, save2);
        file.Close();

        //Unpause();


    }

    private Save CreateSaveGameObject(float xPosition, float yPosition, bool isWielding, Dictionary<string, bool> enemies, float currentEXP, int currentLevel)
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");

        save.xSpawnPosition = xPosition;
        save.ySpawnPosition = yPosition;
        save.isWielding = isWielding;
        save.enemiesInLevel1 = enemies;
        save.currentEXP = currentEXP;
        save.currentLevel = currentLevel;

        return save;
    }
}