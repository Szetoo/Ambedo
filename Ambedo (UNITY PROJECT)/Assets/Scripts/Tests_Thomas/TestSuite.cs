using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    //private Game game;
    
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

}
