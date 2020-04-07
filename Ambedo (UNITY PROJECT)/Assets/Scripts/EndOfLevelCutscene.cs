using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class EndOfLevelCutscene : MonoBehaviour
{
    private bool hasBeenTriggered;
    public int nextLevelNumber;
    private string sceneToLoadName;
    LoadScene sceneToLoad = new LoadScene();
    // Start is called before the first frame update
    void Start()
    {
        hasBeenTriggered = false;
        //LoadScene sceneToLoad = new LoadScene();
        if (nextLevelNumber == 2)
        {
            sceneToLoadName = "Level 2";
        }
        else if (nextLevelNumber == 3)
        {
            sceneToLoadName = "Level 3";
        }
        //sceneToLoad.LoadSceneByName(sceneToLoadName);
    }

    // Update is called once per frame
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

    // Debug.Log("Reading Save File");
    // 2
    // player = GameObject.FindGameObjectWithTag("Player");
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    Save save = (Save)bf.Deserialize(file);
    file.Close();

        // 3

        int gameLevel = nextLevelNumber;
    float xPosition = save.xSpawnPosition;
    float yPosition = save.ySpawnPosition;
    bool isWielding = save.isWielding;
    Dictionary<string, bool> enemies = save.enemiesInLevel1;
       Dictionary<string, bool> enemies2 = save.enemiesInLevel2;
    float currentEXP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentEXP;
    int currentLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>().currentLevel;
    bool trigger = save.cameraPanHasBeenActivated;



        //Save save2 = save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, currentEXP, currentLevel, trigger);


        //Save save2 = CreateSaveGameObject(gameLevel, xPosition, yPosition, isWielding, enemies, currentEXP, currentLevel, trigger);
        Save save2 = Save.CreateSaveObject(gameLevel, xPosition, yPosition, isWielding, enemies, enemies2, currentEXP, currentLevel, trigger);

        // 2
        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    bf.Serialize(file, save2);
    file.Close();
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Level is now: " + nextLevelNumber);
    //Unpause();


}



}