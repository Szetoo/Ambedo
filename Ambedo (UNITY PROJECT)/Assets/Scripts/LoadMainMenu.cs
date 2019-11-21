using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    public void LoadByIndex(int sceneIndex)
    {

        Initiate.Fade("Main Menu", Color.black, 1.0f);
    }

}