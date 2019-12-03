using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BossHealthCameraSaveTestSuite
{
    //private Game game;

    [UnityTest]
    public IEnumerator TestSaveCheckRespawnPosition()
    {
        //Check if the player respawns at the position named in the save file

        //GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        //game = gameGameObject.GetComponent<Game>();

        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

        playerObject.GetComponent<PlayerHealthController>().currentHp = 0;

        Debug.Log("Reading Save File");
        // 2
        // player = GameObject.FindGameObjectWithTag("Player");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();

        playerObject.GetComponent<Transform>().position = new Vector3(save.xSpawnPosition, save.ySpawnPosition);

        Assert.AreEqual(save.xSpawnPosition, playerObject.GetComponent<Transform>().position.x, 0.0001);
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

        GameObject checkpointObject = Object.Instantiate(Resources.Load<GameObject>("Checkpoint"));

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(checkpointObject.GetComponent<CheckPointController>().enabled);

        Object.Destroy(checkpointObject);

        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestCheckPointControllerCheckPlayerInteractsWithSavePoint()
    {
        // Check if the checkpoint is no longer enabled once a player gets close

        GameObject checkpointObject = Object.Instantiate(Resources.Load<GameObject>("Checkpoint"));

        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

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

        GameObject checkpointObject = Object.Instantiate(Resources.Load<GameObject>("Checkpoint"));
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

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

        GameObject cameraObject = Object.Instantiate(Resources.Load<GameObject>("TestingCamera"));
        //Component cameraScript = cameraObject.GetComponent<CustomCamera.CameraMovement>().lef;

        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().leftBound);
        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().rightBound);
        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().upperBound);
        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().lowerBound);

        Object.Destroy(cameraObject);


        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestCameraControllerCheckDeadZoneBounds()
    {
        //Check that the dead zone bounds are correct for some camera object

        GameObject cameraObject = Object.Instantiate(Resources.Load<GameObject>("TestingCamera"));
       // cameraScript = cameraObject.GetComponent<CameraController>();

        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().leftDeadBound);
        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().rightDeadBound);
        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().upperDeadBound);
        Assert.AreEqual(0, cameraObject.GetComponent<CustomCamera.CameraMovement>().lowerDeadBound);

        Object.Destroy(cameraObject);

        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestCameraControllerCheckInitialVelIs0()
    {
        // Check that the camera's initial velocity is 0

        GameObject cameraObject = Object.Instantiate(Resources.Load<GameObject>("TestingCamera"));
        //cameraScript = cameraObject.GetComponent<CameraController>();

        Assert.AreEqual(cameraObject.GetComponent<CustomCamera.CameraMovement>().velocity, Vector3.zero);

        Object.Destroy(cameraObject);

        yield return new WaitForEndOfFrame();

    }


    [UnityTest]
    public IEnumerator TestCameraControllerCheckPlayerMovingInDeadZoneDoesNotAffectPosition()
    {
        // Check that the player moving around in the dead zone does not change the camera's position

        GameObject cameraObject = Object.Instantiate(Resources.Load<GameObject>("TestingCamera"));
        //cameraScript = cameraObject.GetComponent<CameraController>();
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

        //Player is in the middle of camera view
        playerObject.GetComponent<Transform>().SetPositionAndRotation(cameraObject.GetComponent<Transform>().position, Quaternion.identity);

        Vector3 posBefore = cameraObject.GetComponent<Transform>().position;

        //Emulate player movement by slowly increasing x position, up to 1 + original position
        float i = 0.05f;
        while (i < 1)
        {
            Vector3 newPos = new Vector3(playerObject.GetComponent<Transform>().position.x + i, playerObject.GetComponent<Transform>().position.y);
            playerObject.GetComponent<Transform>().SetPositionAndRotation(newPos, Quaternion.identity);
            i += 0.05f;
            yield return new WaitForEndOfFrame();
        }

        Vector3 posAfter = cameraObject.GetComponent<Transform>().position;

        Assert.AreEqual(posBefore, posAfter);

        Object.Destroy(cameraObject);

        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestCameraControllerCheckPlayerReachingBoundsChangesPosition()
    {
        // Check that the player moving outside the dead zone moves the camera

        GameObject cameraObject = Object.Instantiate(Resources.Load<GameObject>("TestingCamera"));
        //cameraScript = cameraObject.GetComponent<CameraController>();
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

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

        yield return new WaitForEndOfFrame();


    }

    [UnityTest]
    public IEnumerator TestBossHealthCheckStartingHealth()
    {
        // Check that the boss' starting health is equal to its current health

        GameObject bossObject = Object.Instantiate(Resources.Load<GameObject>("Boss"));

        Assert.AreEqual(bossObject.GetComponent<BossHealth>().maxHP, bossObject.GetComponent<BossHealth>().currentHp);

        Object.Destroy(bossObject);
        yield return new WaitForEndOfFrame();

    }


    [UnityTest]
    public IEnumerator TestBossHealthCheckInvincibility()
    {
        // Check that the boss is invincible after taking damage

        GameObject bossObject = Object.Instantiate(Resources.Load<GameObject>("Boss"));
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

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

        GameObject bossObject = Object.Instantiate(Resources.Load<GameObject>("Boss"));
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       bossObject.GetComponent<Transform>().position.x - 1,
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

        GameObject bossObject = Object.Instantiate(Resources.Load<GameObject>("Boss"));
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       bossObject.GetComponent<Transform>().position.x - 1,
                                                                       bossObject.GetComponent<Transform>().position.y),
                                                                       Quaternion.identity);

        float healthBefore = bossObject.GetComponent<BossHealth>().currentHp;
        playerObject.GetComponent<PlayerMovementController>().attack();
        yield return new WaitForSeconds(5);
        float healthAfter = bossObject.GetComponent<BossHealth>().currentHp;

        Assert.Less(healthAfter, healthBefore);

        Object.Destroy(bossObject);
        Object.Destroy(playerObject);
        yield return new WaitForEndOfFrame();

    }

}
