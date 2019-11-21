using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePauseMenu : MonoBehaviour
{
    private Transform camera;
    private Transform pauseMenu;
    float x, y, z;

    void Start()
    {
       
       
    }
    private void Update()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        pauseMenu = GameObject.FindGameObjectWithTag("ShowOnPause").transform;
        pauseMenu.position = new Vector3 (camera.position.x, camera.position.y, camera.position.z);
        Debug.Log(camera.position + "-----" + pauseMenu.position);
    }
}
