using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.Playables;

public class FinalCutsceneController : MonoBehaviour
{
    
    private GameObject cutscene;
    private int currentLevel;
    private GameObject player;
    public GameObject badCutscene;
    public GameObject goodCutscene;
   
   
    public Sprite[] transformations;

    

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.Log("Reading Save File");
            // 2
            // player = GameObject.FindGameObjectWithTag("Player");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            Debug.Log(save.currentLevel);
            
            currentLevel = save.currentLevel;
            
           
            player.GetComponent<SpriteRenderer>().sprite = transformations[currentLevel];

            if (currentLevel > 2)
            {
                badCutscene.SetActive(true);
                badCutscene.GetComponent<PlayableDirector>().Play();
            }
            else
            {
                Debug.Log("playing good cutscene");
                goodCutscene.SetActive(true);
                goodCutscene.GetComponent<PlayableDirector>().Play();
            }

            //Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
            currentLevel = 1;
            
        }

    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
