using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadScene : MonoBehaviour
{
    
    GameObject pauseObject;
    private int levelToLoad;
    private string currentLevelName;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.Log("Reading Save File");
            // 2
            // player = GameObject.FindGameObjectWithTag("Player");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            levelToLoad = save.saveFileLevel;
            Debug.Log("Level is now: " + levelToLoad);
            if (levelToLoad == 1)
            {
                currentLevelName = "Level-alex-tim";
            }
            else if (levelToLoad == 2)
            {
                currentLevelName = "level 2";
            }
            else if (levelToLoad == 3)
            {
                currentLevelName = "level 3";
            }
            else if (levelToLoad == 4)
            {
                currentLevelName = "Orphanage";
            }
            // 3
            Debug.Log(save.saveFileLevel);
        }
        else
        {
            levelToLoad = 0;
            Debug.Log("No game saved!");
 
        }
    }
    public void LoadSceneByName(string name)
    {
        if (name == "continue")
        {
            Time.timeScale = 1;
            pauseObject = GameObject.FindGameObjectWithTag("ShowOnPause");
            pauseObject.SetActive(false);
        }

        else if (Time.timeScale == 0) { 
            Time.timeScale = 1;
            pauseObject = GameObject.FindGameObjectWithTag("ShowOnPause");
            pauseObject.SetActive(false);
            if (name == "Main Menu")
            {
                Initiate.Fade(name, Color.black, 1.0f);
            }
            else if (levelToLoad != 0)
            {
                Initiate.Fade(currentLevelName, Color.black, 1.0f);
            }
            else
            {
                Initiate.Fade(name, Color.black, 1.0f);
            }
        }
        else if (Time.timeScale == 1)
        {
            if (name == "Introduction")
            {
                File.Delete(Application.persistentDataPath + "/gamesave.save");
                Save save = CreateSaveGameObject();

                // 2
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
                bf.Serialize(file, save);
                file.Close();
                Initiate.Fade(name, Color.black, 1.0f);
            }
            else if (name == "Main Menu")
            {
                Initiate.Fade(name, Color.black, 1.0f);
            }
            else if (levelToLoad != 0)
            {
                Initiate.Fade(currentLevelName, Color.black, 1.0f);
            }
            else
            {
                Initiate.Fade(name, Color.black, 1.0f);
            }
        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");
        save.saveFileLevel = 1;

        return save;
    }

   



}
