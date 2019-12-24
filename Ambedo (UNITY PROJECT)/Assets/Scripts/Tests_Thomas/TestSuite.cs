using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TestSuite
{
    //private Game game;

    private Scene scene;
    bool clicked;
    GameObject buttonObject;
    
    [UnityTest]
    public IEnumerator MainMenuToGame()
    {
        SceneManager.LoadScene("Main Menu");
        yield return new WaitForSeconds(4);
        scene = SceneManager.GetSceneByName("Main Menu");
        SceneManager.SetActiveScene(scene);
        //EditorSceneManager.LoadScene("Assets/Scenes/Main Menu.unity");


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

        yield return new WaitForSeconds(4);
        scene = SceneManager.GetActiveScene();
        /*
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene("Corey_Scene");
        yield return new WaitForSeconds(4);
        scene = SceneManager.GetSceneByName("Corey_Scene");
        SceneManager.SetActiveScene(scene);
        */
        Debug.Log(message: "-Debug Current Scene-");
        Debug.Log(scene.name);
        Assert.True(scene.name == "Corey_Scene");
    }

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

    [UnityTest]
    public IEnumerator TestSaveCheckRespawnPosition()
    {
        //Check if the player respawns at the position named in the save file

        //GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        //game = gameGameObject.GetComponent<Game>();
        
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        playerObject.GetComponent<PlayerHealthController>().currentHp = 0;

        Debug.Log("Reading Save File");
        // 2
        // player = GameObject.FindGameObjectWithTag("Player");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();

        playerObject.GetComponent<Transform>().position = new Vector3(save.xSpawnPosition, save.ySpawnPosition);

        Assert.AreEqual(save.xSpawnPosition, playerObject.GetComponent<Transform>().position.x,0.0001);
        Assert.AreEqual(save.ySpawnPosition, playerObject.GetComponent<Transform>().position.y, 0.0001);

        //Object.Destroy(game);
        Object.Destroy(playerObject);

        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestSaveCheckSaveFileNotNull()
    {
        //Check if the save file is not null

        Debug.Log("Reading Save File");
        // 2
        // player = GameObject.FindGameObjectWithTag("Player");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();

        Assert.IsNotNull(save);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestCheckpointControllerCheckEnabledIsTrue()
    {
        //Check if a newly spawned checkpoint is active

        GameObject checkpointObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Checkpoint"));

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(checkpointObject.GetComponent<CheckPointController>().enabled);

        Object.Destroy(checkpointObject);

        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestCheckPointControllerCheckPlayerInteractsWithSavePoint()
    {
        // Check if the checkpoint is no longer enabled once a player gets close

        GameObject checkpointObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Checkpoint"));

        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3
                                                                      (checkpointObject.GetComponent<Transform>().position.x,
                                                                      checkpointObject.GetComponent<Transform>().position.y),
                                                                      Quaternion.identity);

        

        Assert.IsFalse(checkpointObject.GetComponent<CheckPointController>().enabled);
        

        Object.Destroy(checkpointObject);

        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestCheckPointControllerCheckSavePositionIsCorrect()
    {
        // Check if a player's save point and their respawn point are the same

        GameObject checkpointObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Checkpoint"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3
                                                                      (checkpointObject.GetComponent<Transform>().position.x,
                                                                      checkpointObject.GetComponent<Transform>().position.y),
                                                                      Quaternion.identity);

        yield return new WaitForSeconds(0.1f);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();

        Assert.AreEqual(checkpointObject.GetComponent<Transform>().position.x, save.xSpawnPosition);
        Assert.AreEqual(checkpointObject.GetComponent<Transform>().position.y, save.ySpawnPosition);


        Object.Destroy(checkpointObject);

        yield return new WaitForEndOfFrame();

    }
    
    [UnityTest]
    public IEnumerator TestCameraControllerCheckRegularBounds()
    {
        // Check that the regular bounds are correct for some camera object

        GameObject cameraObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TestingCamera"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        CameraMovement cameraScript = cameraObject.GetComponent<CameraMovement>();
        cameraScript.target = playerObject.GetComponent<Transform>();

        Assert.AreEqual(0, cameraScript.leftBound);
        Assert.AreEqual(0, cameraScript.rightBound);
        Assert.AreEqual(0, cameraScript.upperBound);
        Assert.AreEqual(0, cameraScript.lowerBound);

        Object.Destroy(cameraObject);
        Object.Destroy(playerObject);


        yield return new WaitForEndOfFrame();

    }
    
    [UnityTest]
    public IEnumerator TestCameraControllerCheckDeadZoneBounds()
    {
        //Check that the dead zone bounds are correct for some camera object

        GameObject cameraObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TestingCamera"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        CameraMovement cameraScript = cameraObject.GetComponent<CameraMovement>();
        cameraScript.target = playerObject.GetComponent<Transform>();

        Assert.AreEqual(0, cameraScript.leftDeadBound);
        Assert.AreEqual(0, cameraScript.rightDeadBound);
        Assert.AreEqual(0, cameraScript.upperDeadBound);
        Assert.AreEqual(0, cameraScript.lowerDeadBound);

        Object.Destroy(cameraObject);
        Object.Destroy(playerObject);

        yield return new WaitForEndOfFrame();

    }
    
    [UnityTest]
    public IEnumerator TestCameraControllerCheckInitialVelIs0()
    {
        // Check that the camera's initial velocity is 0

        GameObject cameraObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TestingCamera"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        CameraMovement cameraScript = cameraObject.GetComponent<CameraMovement>();
        cameraScript.target = playerObject.GetComponent<Transform>();
        

        Assert.AreEqual(cameraScript.velocity, Vector3.zero);

        Object.Destroy(cameraObject);
        Object.Destroy(playerObject);

        yield return new WaitForEndOfFrame();

    }
    

    [UnityTest]
    public IEnumerator TestCameraControllerCheckPlayerMovingInDeadZoneDoesNotAffectPosition()
    {
        // Check that the player moving around in the dead zone does not change the camera's position

        GameObject cameraObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TestingCamera"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        CameraMovement cameraScript = cameraObject.GetComponent<CameraMovement>();
        cameraScript.target = playerObject.GetComponent<Transform>();

        //Player is in the middle of camera view
        playerObject.GetComponent<Transform>().SetPositionAndRotation(cameraObject.GetComponent<Transform>().position, Quaternion.identity);

        Vector3 posBefore = cameraObject.GetComponent<Transform>().position;

        Debug.Log(posBefore.ToString() + " (CAMERA)");
        Debug.Log(playerObject.GetComponent<Transform>().position.ToString() + " (PLAYER)");

        //Emulate player movement to the left by directly changing x position to 1 - starting position
        Vector3 newPos = new Vector3(playerObject.GetComponent<Transform>().position.x - 1f, playerObject.GetComponent<Transform>().position.y);
        playerObject.GetComponent<Transform>().SetPositionAndRotation(newPos, Quaternion.identity);

        Vector3 posAfter = cameraObject.GetComponent<Transform>().position;
        Assert.AreEqual(posBefore.x, posAfter.x);
        

        //Emulate player movement to the right by slowly increasing x position, stopping short of original position

        float i = 0.05f;
        while (i < 0.5)
        {
            newPos = new Vector3(playerObject.GetComponent<Transform>().position.x + 0.05f, playerObject.GetComponent<Transform>().position.y);
            Debug.Log(newPos);
            playerObject.GetComponent<Transform>().SetPositionAndRotation(newPos, Quaternion.identity);
            i += 0.05f;
            yield return new WaitForEndOfFrame();
        }

        posAfter = cameraObject.GetComponent<Transform>().position;
        Debug.Log(posAfter);

        Assert.AreEqual(posBefore.x, posAfter.x, 0.1f);

        Object.Destroy(cameraObject);
        Object.Destroy(playerObject);

        yield return new WaitForEndOfFrame();

    }
    
    [UnityTest]
    public IEnumerator TestCameraControllerCheckPlayerReachingBoundsChangesPosition()
    {
        // Check that the player moving outside the dead zone moves the camera

        GameObject cameraObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TestingCamera"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        CameraMovement cameraScript = cameraObject.GetComponent<CameraMovement>();
        cameraScript.target = playerObject.GetComponent<Transform>();

        playerObject.GetComponent<Transform>().SetPositionAndRotation(cameraObject.GetComponent<Transform>().position, Quaternion.identity);

        Vector3 posBefore = cameraObject.GetComponent<Transform>().position;

        //Emulate player movement by slowly increasing x position, up to 10 + original position
        float i = 1f;
        while (i <= 10)
        {
            Vector3 newPos = new Vector3(playerObject.GetComponent<Transform>().position.x + i, playerObject.GetComponent<Transform>().position.y);
            playerObject.GetComponent<Transform>().SetPositionAndRotation(newPos, Quaternion.identity);
            i += 1f;
            yield return new WaitForEndOfFrame();
        }

        Vector3 posAfter = cameraObject.GetComponent<Transform>().position;

        Assert.AreNotEqual(posBefore, posAfter);

        Object.Destroy(cameraObject);
        Object.Destroy(playerObject);

        yield return new WaitForEndOfFrame();


    }
    
    [UnityTest]
    public IEnumerator TestBossHealthCheckStartingHealth()
    {
        // Check that the boss' starting health is equal to its current health

        GameObject bossObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Boss"));

        Assert.AreEqual(bossObject.GetComponent<BossHealth>().maxHP, bossObject.GetComponent<BossHealth>().currentHp);

        Object.Destroy(bossObject);
        yield return new WaitForEndOfFrame();

    }


    [UnityTest]
    public IEnumerator TestBossHealthCheckInvincibility()
    {
        // Check that the boss is invincible after taking damage

        GameObject bossObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Boss"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       bossObject.GetComponent<Transform>().position.x - 1,
                                                                       bossObject.GetComponent<Transform>().position.y),
                                                                       Quaternion.identity);

        playerObject.GetComponent<PlayerMovementController>().attack();

        float healthBefore = bossObject.GetComponent<BossHealth>().currentHp;
        yield return new WaitForSeconds(0.8f);
        playerObject.GetComponent<PlayerMovementController>().attack();

        float healthAfter = bossObject.GetComponent<BossHealth>().currentHp;

        Assert.AreEqual(healthBefore, healthAfter);

        Object.Destroy(bossObject);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestBossHealthCheckTakesDamage()
    {
        // Check that the boss takes damage upon being hit

        GameObject bossObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Boss"));
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       bossObject.GetComponent<Transform>().position.x,
                                                                       bossObject.GetComponent<Transform>().position.y),
                                                                       Quaternion.identity);

        float healthBefore = bossObject.GetComponent<BossHealth>().currentHp;
        playerObject.GetComponent<PlayerMovementController>().attack();
        
        float healthAfter = bossObject.GetComponent<BossHealth>().currentHp;

        Assert.Less(healthAfter, healthBefore);

        Object.Destroy(bossObject);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestBossHealthCheckHealthRegen()
    {
        // Check that the boss regenerates health after several seconds

        GameObject bossPrefab = Resources.Load<GameObject>("Prefabs/Boss");
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        Assert.IsNotNull(bossPrefab);
        Assert.IsNotNull(playerPrefab);

        GameObject bossObject = MonoBehaviour.Instantiate(bossPrefab);
        GameObject playerObject = MonoBehaviour.Instantiate(playerPrefab);

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       bossObject.GetComponent<Transform>().position.x,
                                                                       bossObject.GetComponent<Transform>().position.y),
                                                                       Quaternion.identity);

        float healthBefore = bossObject.GetComponent<BossHealth>().currentHp;
        playerObject.GetComponent<PlayerMovementController>().attack();
        float healthAfter = bossObject.GetComponent<BossHealth>().currentHp;

        Assert.Less(healthAfter, healthBefore);

        yield return new WaitForSeconds(5);
        healthAfter = bossObject.GetComponent<BossHealth>().currentHp;

        Assert.AreEqual(healthAfter, healthBefore);

        Object.Destroy(bossObject);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerMovementCheckFalling()
    {
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        GameObject testingPlatform = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/stationary_platform"));

        //Spawn player slightly above platform
        testingPlatform.GetComponent<Transform>().position = Vector2.zero;
        playerObject.GetComponent<Transform>().position = new Vector2(0f, 5f);

        Vector2 posBefore = playerObject.GetComponent<Transform>().position;

        yield return new WaitForSeconds(2);

        Vector2 posAfter = playerObject.GetComponent<Transform>().position;

        Assert.Less(posAfter.y, posBefore.y);

        Object.Destroy(testingPlatform);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerMovementCheckMovementLeft()
    {
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        GameObject testingPlatform = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/stationary_platform"));

        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();

        //Spawn player slightly above platform
        testingPlatform.GetComponent<Transform>().position = Vector2.zero;
        playerObject.GetComponent<Transform>().position = new Vector2(0f, 5f);
        
        yield return new WaitForSeconds(2);

        Vector2 posBefore = playerObject.GetComponent<Transform>().position;

        for (int i = 0; i < 30; i++)
        {
            playerObject.GetComponent<PlayerMovementController>().horizontalMovement(rb, -1);
            //yield return new WaitForEndOfFrame();
        }

        Vector2 posAfter = playerObject.GetComponent<Transform>().position;

        Assert.Less(posAfter.x, posBefore.x);

        Object.Destroy(testingPlatform);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerMovementCheckMovementRight()
    {
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        GameObject testingPlatform = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/stationary_platform"));

        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();

        //Spawn player slightly above platform
        testingPlatform.GetComponent<Transform>().position = Vector2.zero;
        playerObject.GetComponent<Transform>().position = new Vector2(0f, 5f);

        yield return new WaitForSeconds(2);

        Vector2 posBefore = playerObject.GetComponent<Transform>().position;

        for (int i = 0; i < 30; i++)
        {
            playerObject.GetComponent<PlayerMovementController>().horizontalMovement(rb, 1);
            yield return new WaitForEndOfFrame();
        }

        Vector2 posAfter = playerObject.GetComponent<Transform>().position;

        Assert.Greater(posAfter.x, posBefore.x);

        Object.Destroy(testingPlatform);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerMovementCheckShortJump()
    {
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        GameObject testingPlatform = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/stationary_platform"));

        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();

        //Spawn player slightly above platform
        testingPlatform.GetComponent<Transform>().position = Vector2.zero;
        playerObject.GetComponent<Transform>().position = new Vector2(0f, 5f);

        yield return new WaitForSeconds(2);

        float initialY = playerObject.GetComponent<Transform>().position.y; 
        float maxY = playerObject.GetComponent<Transform>().position.y;

        //Get highest point of short jump 
        playerObject.GetComponent<PlayerMovementController>().jump(rb, true);
        for (int i = 0; i < 60; i++)
        {
            if (playerObject.GetComponent<Transform>().position.y > maxY) {
                maxY = playerObject.GetComponent<Transform>().position.y;
            }
            yield return new WaitForEndOfFrame();
        }

        Assert.Less(initialY, maxY);

        Object.Destroy(testingPlatform);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerMovementCheckTallJump()
    {
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        GameObject testingPlatform = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/stationary_platform"));

        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();

        //Spawn player slightly above platform
        testingPlatform.GetComponent<Transform>().position = Vector2.zero;
        playerObject.GetComponent<Transform>().position = new Vector2(0f, 5f);

        yield return new WaitForSeconds(2);

        float initialY = playerObject.GetComponent<Transform>().position.y;
        float maxYShort = playerObject.GetComponent<Transform>().position.y;
        float maxYTall = playerObject.GetComponent<Transform>().position.y;

        //Get highest point of short jump
        playerObject.GetComponent<PlayerMovementController>().jump(rb, true);
        for (int i = 0; i < 60; i++)
        {
            if (playerObject.GetComponent<Transform>().position.y > maxYShort)
            {
                maxYShort = playerObject.GetComponent<Transform>().position.y;
            }
            yield return new WaitForEndOfFrame();
        }

        //Get highest point of tall jump
        for (int i = 0; i < 60; i++)
        {
            playerObject.GetComponent<PlayerMovementController>().jump(rb, true); 
            if (playerObject.GetComponent<Transform>().position.y > maxYTall)
            {
                maxYTall = playerObject.GetComponent<Transform>().position.y;
            }
            yield return new WaitForEndOfFrame();
        }

        Assert.Less(initialY, maxYShort);
        Assert.Less(maxYShort, maxYTall);

        Object.Destroy(testingPlatform);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerMovementCheckAttack()
    {
        GameObject playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        GameObject testingPlatform = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/stationary_platform"));

        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();

        //Spawn player slightly above platform
        testingPlatform.GetComponent<Transform>().position = Vector2.zero;
        playerObject.GetComponent<Transform>().position = new Vector2(0f, 5f);

        yield return new WaitForSeconds(2);

        playerObject.GetComponent<PlayerMovementController>().attack();
        Transform attackHitbox = playerObject.GetComponentInChildren<Transform>();
        Assert.IsNotNull(attackHitbox);
        yield return new WaitForSeconds(0.5f);
        attackHitbox = playerObject.GetComponentInChildren<Transform>();
        Assert.IsNull(attackHitbox);

        Object.Destroy(testingPlatform);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();
    }
    

}
