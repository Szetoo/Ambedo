using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

//This script saves the position of the checkpoint to the save file so that the player will
//spawn at the most recent checkpoint.
public class CheckPointController : MonoBehaviour { 

    public bool enabled;
    private float xCheckPointPosition;
    private float yCheckPointPosition;
    private GameObject player;

    void Start()
    {
        enabled = true;
        xCheckPointPosition = gameObject.GetComponent<Transform>().position.x;
        yCheckPointPosition = gameObject.GetComponent<Transform>().position.y;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.gameObject.tag == "Player" & enabled == true)
    {
            //Saving checkpoint position to save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            int gameLevel = save.saveFileLevel;
            Dictionary<string, bool> enemies = save.enemiesInLevel1;
            Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
            Dictionary<string, bool> enemies3 = save.enemiesInLevel3;
            float currentEXP = save.currentEXP;
            int currentLevel = save.currentLevel;
            Debug.Log("Checkpoint status: " + enabled);
            Debug.Log("Player has reached checkpoint, saving game");
            enabled = false;
            Debug.Log("Checkpoint status: " + enabled);
            Debug.Log("Player will spawn at x: " + xCheckPointPosition + " y: " + yCheckPointPosition);
            Debug.Log(Application.persistentDataPath);
            bool cameraPanTrigger = save.cameraPanHasBeenActivated;
            //Save save2 = CreateSaveGameObject(gameLevel, xCheckPointPosition, yCheckPointPosition, player.GetComponent<PlayerMovementController>().isWielding, enemies,enemies2, currentEXP, currentLevel, cameraPanTrigger);
            Save save2 = Save.CreateSaveObject(gameLevel, xCheckPointPosition, yCheckPointPosition, player.GetComponent<PlayerMovementController>().isWielding, enemies, enemies2, enemies3, currentEXP, currentLevel, cameraPanTrigger);

            // 2
            file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            bf.Serialize(file, save2);
            file.Close();
        }
    }

   
}
