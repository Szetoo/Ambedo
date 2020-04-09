using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Transition from intro scene to first level.
public class IntroductionToLevel1 : MonoBehaviour
{
    LoadScene sceneToLoad = new LoadScene();
    // Start is called before the first frame update
    void Awake()
    {
        sceneToLoad.LoadSceneByName("Level-alex-tim");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
