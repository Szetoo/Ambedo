using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//This script manages the behaviour for when the player first collects the sword.
//The player will ALWAYS have the sword after this point and isWielding is always true now
//so that the player always has the sword. This gets saved to the save file.
public class SwordCutscene : MonoBehaviour { 

    GameObject player;
    GameObject pedestal;
    private float xCheckPointPosition;
    private float yCheckPointPosition;
    private bool hasBeenTriggered;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pedestal = GameObject.FindGameObjectWithTag("Pedestal");
        if (player.GetComponent<PlayerMovementController>().isWielding == true)
        {
            pedestal.SetActive(false);
            gameObject.SetActive(false);
            //pedestal.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player =  GameObject.FindGameObjectWithTag("Player");
        pedestal = GameObject.FindGameObjectWithTag("Pedestal");
        xCheckPointPosition = gameObject.GetComponent<Transform>().position.x;
        yCheckPointPosition = gameObject.GetComponent<Transform>().position.y;
        hasBeenTriggered = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" & !hasBeenTriggered)
        {
            gameObject.GetComponent<PlayableDirector>().Play();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            player.GetComponent<PlayerMovementController>().isWielding = true;

            SaveToGame();

         
            hasBeenTriggered = true;

        }
    }
    

    private void SaveToGame()
    {


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();

        // 3
        int gameLevel = save.saveFileLevel;
        float xPosition = xCheckPointPosition;
        float yPosition = yCheckPointPosition;
        bool isWielding = true;
        Dictionary<string, bool> enemies = save.enemiesInLevel1;
        Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
        Dictionary<string, bool> enemies3 = save.enemiesInLevel3;
        float currentEXP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentEXP;
        int currentLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentLevel;
        bool trigger = save.cameraPanHasBeenActivated;






        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2, enemies3, currentEXP, currentLevel, trigger);

        // 2
        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        bf.Serialize(file, save2);
        file.Close();



    }

  
}
