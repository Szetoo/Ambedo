using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class TestSuite
{
    private Scene scene;

    // 1
    [UnityTest]
    public IEnumerator MainMenuToGame()
    {
        EditorSceneManager.LoadScene("Assets/Scenes/Main Menu.unity");
        yield return new WaitForEndOfFrame();
        scene = EditorSceneManager.GetActiveScene();
        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Main Menu");

        //buttonObject = GameObject.Find("Continue");
        //Debug.Log(message: "-Debug Current Button-");
        //Debug.Log(buttonObject);
        //Button setupButton = buttonObject.GetComponent<Button>();
        //Assert.NotNull(setupButton);
        //clicked = false;
        //setupButton.onClick.AddListener(Clicked);
        //setupButton.onClick.Invoke();
        //Assert.True(clicked);

        //scene = EditorSceneManager.GetActiveScene();
        //Debug.Log(message: "-Debug Current Scene-");
        //Debug.Log(scene.name);
        //Assert.True(scene.name == "Corey_Scene");
    }

}

