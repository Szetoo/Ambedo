using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                hasBeenTriggered = true;
                sceneToLoad.LoadSceneByName(sceneToLoadName);
            }
        }
    }
