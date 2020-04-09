using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            //player.GetComponent<PlayerMovementController>().horizontalMovement(other.GetComponent<Rigidbody2D>(), 2);
            SaveToGame();

            /*
            Save save = CreateSaveGameObject();

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();
            */
            hasBeenTriggered = true;

        }
    }
    /*
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");

        save.xSpawnPosition = xCheckPointPosition;
        save.ySpawnPosition = yCheckPointPosition;
        save.isWielding = true;

        return save;
    }*/

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
        float xPosition = xCheckPointPosition;
        float yPosition = yCheckPointPosition;
        bool isWielding = true;
        Dictionary<string, bool> enemies = save.enemiesInLevel1;
        Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
        Dictionary<string, bool> enemies3 = save.enemiesInLevel3;
        float currentEXP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentEXP;
        int currentLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentLevel;
        bool trigger = save.cameraPanHasBeenActivated;






        //Save save2 = CreateSaveGameObject(gameLevel, xPosition, yPosition, isWielding, enemies, currentEXP, currentLevel, trigger);
        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2, enemies3, currentEXP, currentLevel, trigger);

        // 2
        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        bf.Serialize(file, save2);
        file.Close();

        //Unpause();


    }

  
}
