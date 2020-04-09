using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


//This script manages which level to load next upon reaching the end of a level
//This script sets the correct starting point of the next level
public class EndOfLevelCutscene : MonoBehaviour
{
    private bool hasBeenTriggered;
    public int nextLevelNumber;
    private string sceneToLoadName;

    private float newStartingX;
    private float newStartingY;
    LoadScene sceneToLoad = new LoadScene();
    // Start is called before the first frame update
    void Start()
    {
        hasBeenTriggered = false;
        //LoadScene sceneToLoad = new LoadScene();
        if (nextLevelNumber == 2)
        {
            Debug.Log("Player will now spawn at: ");
            sceneToLoadName = "level 2";
            newStartingX = 128.5f;
            newStartingY = 34.1f;
            Debug.Log("X: " + newStartingX);
            Debug.Log("Y: " + newStartingY);
        }
        else if (nextLevelNumber == 3)
        {
            Debug.Log("Player will now spawn at: ");
            sceneToLoadName = "level 3";
            newStartingX = 150.32f;
            newStartingY = -26.01f;
            Debug.Log("X: " + newStartingX);
            Debug.Log("Y: " + newStartingY);
        }
        else if(nextLevelNumber == 4)
        {
            sceneToLoadName = "Orphanage";
        }

    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player" && !hasBeenTriggered)
            {
            SaveToGame();
                hasBeenTriggered = true;
                sceneToLoad.LoadSceneByName(sceneToLoadName);
            }
        }
    

private void SaveToGame()
{


    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    Save save = (Save)bf.Deserialize(file);
    file.Close();

        // 3

        int gameLevel = nextLevelNumber;
        float xPosition = newStartingX;
        float yPosition = newStartingY;
    bool isWielding = save.isWielding;
    Dictionary<string, bool> enemies = save.enemiesInLevel1;
       Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
        Dictionary<string, bool> enemies3 = save.enemiesInLevel3;
        float currentEXP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentEXP;
    int currentLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentLevel;
    bool trigger = save.cameraPanHasBeenActivated;




        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2,enemies3, currentEXP, currentLevel, trigger);


        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    bf.Serialize(file, save2);
    file.Close();
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Level is now: " + nextLevelNumber);



}



}