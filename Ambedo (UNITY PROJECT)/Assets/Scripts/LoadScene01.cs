using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene01 : MonoBehaviour
{
    public void LoadByIndex(int sceneIndex) {

        Initiate.Fade("scene_01_tim", Color.black, 1.0f);
    }

}
