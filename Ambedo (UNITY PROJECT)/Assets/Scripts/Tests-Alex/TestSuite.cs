using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSuite
{
    private Scene scene;
    bool clicked;
    GameObject buttonObject;

    // 1
    [UnityTest]
    public IEnumerator MainMenuToGame()
    {
        GameObject sceneObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Scenes/Main Menu"));
        return null;
        //EditorSceneManager.LoadScene("Assets/Scenes/Main Menu.unity");
        
        //scene = EditorSceneManager.GetActiveScene();
        //Debug.Log(message: "-Debug Current Scene-");
        //Debug.Log(scene.name);
        //Assert.True(scene.name == "Main Menu");

        //buttonObject = GameObject.Find("Continue");
        //Debug.Log(message: "-Debug Current Button-");
        //Debug.Log(buttonObject);
        //Button setupButton = buttonObject.GetComponent<Button>();
        //Assert.NotNull(setupButton);
        //clicked = false;
        //setupButton.onClick.AddListener(Clicked);
        //setupButton.onClick.Invoke();
        //Assert.True(clicked);

        //yield return new WaitForEndOfFrame();

        //scene = EditorSceneManager.GetActiveScene();
        //Debug.Log(message: "-Debug Current Scene-");
        //Debug.Log(scene.name);
        //Assert.True(scene.name == "Corey_Scene");
    }



    private void Clicked()
    {
        clicked = true;
    }

}

