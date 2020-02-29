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
            Save save = CreateSaveGameObject();

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();
            hasBeenTriggered = true;

        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");

        save.xSpawnPosition = xCheckPointPosition;
        save.ySpawnPosition = yCheckPointPosition;
        save.isWielding = true;

        return save;
    }
}
