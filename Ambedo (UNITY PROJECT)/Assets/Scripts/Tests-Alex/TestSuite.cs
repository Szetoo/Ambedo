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
        SceneManager.LoadScene("Main Menu");
        yield return new WaitForSeconds(4);
        scene = SceneManager.GetSceneByName("Main Menu");
        SceneManager.SetActiveScene(scene);
        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Main Menu");

        buttonObject = GameObject.Find("Continue");
        Debug.Log(message: "-Debug Current Button-");
        Debug.Log(buttonObject);
        Button setupButton = buttonObject.GetComponent<Button>();
        Assert.NotNull(setupButton);
        clicked = false;
        setupButton.onClick.AddListener(Clicked);
        setupButton.onClick.Invoke();
        Assert.True(clicked);

        yield return new WaitForEndOfFrame();

        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene("Corey_Scene");
        yield return new WaitForSeconds(4);
        scene = SceneManager.GetSceneByName("Corey_Scene");
        SceneManager.SetActiveScene(scene);

        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Corey_Scene");
    }
    
    // 2
    [UnityTest]
    public IEnumerator GameToMainMenu()
    {
        // Test 'Quit to Main Menu' button changes scene from pause menu to game
        SceneManager.LoadScene("Corey_Scene");
        yield return new WaitForSeconds(4);
        scene = SceneManager.GetSceneByName("Corey_Scene");
        SceneManager.SetActiveScene(scene);
        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Corey_Scene");

        buttonObject = GameObject.Find("Quit To Main Menu");
        Debug.Log(message: "-Debug Current Button-");
        Debug.Log(buttonObject);
        Button setupButton = buttonObject.GetComponent<Button>();
        Assert.NotNull(setupButton);
        clicked = false;
        setupButton.onClick.AddListener(Clicked);
        setupButton.onClick.Invoke();
        Assert.True(clicked);

        yield return new WaitForEndOfFrame();

        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene("Main Menu");
        yield return new WaitForSeconds(4);
        scene = SceneManager.GetSceneByName("Main Menu");
        SceneManager.SetActiveScene(scene);

        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Main Menu");
        
    }

    private void Clicked()
    {
        clicked = true;
    }

}

