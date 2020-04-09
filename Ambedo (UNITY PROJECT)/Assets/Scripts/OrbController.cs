using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//This controller controls the movement of the EXP orbs when they are spawned.
//Upon contact with the player, they will increase the player's EXP and save to the save file.
public class OrbController : MonoBehaviour

{
    int speed = 4;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
            collider.enabled = false;

        

            Destroy(gameObject);
        }

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