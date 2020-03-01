using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadScene : MonoBehaviour
{
    GameObject pauseObject;
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
            Initiate.Fade(name, Color.black, 1.0f);
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

        return save;
    }

}
