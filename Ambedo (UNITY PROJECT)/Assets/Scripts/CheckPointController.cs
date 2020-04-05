using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointController : MonoBehaviour { 

    public bool enabled;
    private float xCheckPointPosition;
    private float yCheckPointPosition;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enabled = true;
        xCheckPointPosition = gameObject.GetComponent<Transform>().position.x;
        yCheckPointPosition = gameObject.GetComponent<Transform>().position.y;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.gameObject.tag == "Player" & enabled == true)
    {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3


            int gameLevel = save.saveFileLevel;
            Dictionary<string, bool> enemies = save.enemiesInLevel1;
            float currentEXP = save.currentEXP;
            int currentLevel = save.currentLevel;
            Debug.Log("Checkpoint status: " + enabled);
            Debug.Log("Player has reached checkpoint, saving game");
            enabled = false;
            Debug.Log("Checkpoint status: " + enabled);
            Debug.Log("Player will spawn at x: " + xCheckPointPosition + " y: " + yCheckPointPosition);
            Debug.Log(Application.persistentDataPath);
            bool cameraPanTrigger = save.cameraPanHasBeenActivated;
            Save save2 = CreateSaveGameObject(gameLevel, xCheckPointPosition, yCheckPointPosition, player.GetComponent<PlayerMovementController>().isWielding, enemies, currentEXP, currentLevel, cameraPanTrigger);

            // 2
            file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            bf.Serialize(file, save2);
            file.Close();
        }
    }

    private Save CreateSaveGameObject(int level, float xPosition, float yPosition, bool isWielding, Dictionary<string, bool> enemies, float currentEXP, int currentLevel, bool trigger)
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");
        save.saveFileLevel = level;
        save.xSpawnPosition = xPosition;
        save.ySpawnPosition = yPosition;
        save.isWielding = isWielding;
        save.enemiesInLevel1 = enemies;
        save.currentEXP = currentEXP;
        save.currentLevel = currentLevel;
        save.cameraPanHasBeenActivated = trigger;

        return save;
    }
}
