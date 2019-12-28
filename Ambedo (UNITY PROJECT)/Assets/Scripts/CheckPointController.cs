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

        //Enabled on load with position of checkpoint being tracked
        enabled = true;
        xCheckPointPosition = gameObject.GetComponent<Transform>().position.x;
        yCheckPointPosition = gameObject.GetComponent<Transform>().position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

    //If player touches the enabled checkpoint, save to file and disable the checkpoint
    if (other.gameObject.tag == "Player" & enabled == true)
    {
            Debug.Log("Checkpoint status: " + enabled);
            Debug.Log("Player has reached checkpoint, saving game");
            enabled = false;
            Debug.Log("Checkpoint status: " + enabled);
            Debug.Log("Player will spawn at x: " + xCheckPointPosition + " y: " + yCheckPointPosition);
            Debug.Log(Application.persistentDataPath);

            Save save = CreateSaveGameObject();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();
        }
    }

    private Save CreateSaveGameObject()
    {
        //Write to save file the x, y position of checkpoint
        Save save = new Save();
        save.xSpawnPosition = xCheckPointPosition;
        save.ySpawnPosition = yCheckPointPosition;
        return save;
    }
}
