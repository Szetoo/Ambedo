using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class LoadSceneTest
{
    Scene scene;
    GameObject buttonObject;
    bool clicked;

    void LoadLevel(string levelPath)
    {
        EditorSceneManager.OpenScene(levelPath);
    }

    private void Clicked()
    {
        clicked = true;
    }

    [Test]
    public void Test01()
    {
        // Test 'Continue' button changes scene from mainmenu to game
        LoadLevel("Assets/Scenes/Main Menu.unity");
        scene = EditorSceneManager.GetActiveScene();
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

        scene = EditorSceneManager.GetActiveScene();
        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Corey_Scene");
    }

    [Test]
    public void Test02()
    {
        // Test 'Quit to Main Menu' button changes scene from pause menu to game
        LoadLevel("Assets/Scenes/Cory_Scene.unity");
        scene = EditorSceneManager.GetActiveScene();
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

        scene = EditorSceneManager.GetActiveScene();
        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Main Menu");
    }
}