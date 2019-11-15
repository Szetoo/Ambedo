using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadByIndex(int sceneIndex) {

        Initiate.Fade("Demo Room 2", Color.black, 1.0f);
    }

}
