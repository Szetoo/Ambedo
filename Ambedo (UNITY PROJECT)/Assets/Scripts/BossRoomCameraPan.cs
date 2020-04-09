using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.IO;

//This script is used for the camera pan in the first level
//This script highlights the player's available choices which is extremely important for our game experience.
public class BossRoomCameraPan : MonoBehaviour
{

    GameObject player;
    public bool hasBeenTriggered;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
 
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            hasBeenTriggered = save.cameraPanHasBeenActivated;
            if (hasBeenTriggered == true)
            {
                Destroy(gameObject);
            }
            
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

        }
    }

    //Saving to the file if it has been triggered before so that the player will not trigger it again.
    private IEnumerator SaveToGame(bool trigger)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();


        int gameLevel = save.saveFileLevel;
        float xPosition = save.xSpawnPosition;
        float yPosition = save.ySpawnPosition;
        bool isWielding = save.isWielding;
        Dictionary<string, bool> enemies = save.enemiesInLevel1;
        Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
        Dictionary<string, bool> enemies3 = save.enemiesInLevel3;
        float curEXP = save.currentEXP;
        int curLevel = save.currentLevel;


        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2, enemies3, curEXP, curLevel, true);


        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        bf.Serialize(file, save2);
        file.Close();
        gameObject.GetComponent<PlayableDirector>().Play();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;



        yield return null;


    }

    
}
