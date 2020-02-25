using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionToLevel1 : MonoBehaviour
{
    LoadScene sceneToLoad = new LoadScene();
    // Start is called before the first frame update
    void Awake()
    {
        sceneToLoad.LoadSceneByName("Corey_Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
