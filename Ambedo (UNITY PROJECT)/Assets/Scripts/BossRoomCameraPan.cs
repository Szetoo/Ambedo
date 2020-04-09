using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.IO;

public class BossRoomCameraPan : MonoBehaviour
{



    GameObject player;
    public bool hasBeenTriggered;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            //Debug.Log("Reading Save File");
            // 2
            // player = GameObject.FindGameObjectWithTag("Player");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            //Debug.Log(save.cameraPanHasBeenActivated);

            hasBeenTriggered = save.cameraPanHasBeenActivated;
            if (hasBeenTriggered == true)
            {
                Destroy(gameObject);
            }
            



            //Unpause();
        }
        else
        {
            hasBeenTriggered = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //hasBeenTriggered = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" & !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            StartCoroutine(SaveToGame(true));
            hasBeenTriggered = true;
            //SaveToGame(true);
            //gameObject.GetComponent<PlayableDirector>().Play();
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //hasBeenTriggered = true;
            //SaveToGame(hasBeenTriggered);

        }
    }

    private IEnumerator SaveToGame(bool trigger)
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
        float curEXP = save.currentEXP;
        int curLevel = save.currentLevel;
        //bool cameraPanBool = trigger;
        //Debug.Log("camera pan value");
       // Debug.Log(trigger);






        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2, enemies3, curEXP, curLevel, true);

        // 2
        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        bf.Serialize(file, save2);
        file.Close();
        gameObject.GetComponent<PlayableDirector>().Play();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;


        //Unpause();
        yield return null;

        //Destroy(gameObject);
    }

    
}
