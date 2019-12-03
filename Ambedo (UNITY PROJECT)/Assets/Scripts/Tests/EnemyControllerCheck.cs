using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class EnemyControllerCheck
{
    private GameObject enemy;
    private GameObject player;
    public float speed = 3;

    [SetUp]
    public void Setup()
    {
        enemy = Object.Instantiate(Resources.Load("Enemy2", typeof(GameObject))) as GameObject;
       // player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(enemy);
       // Object.Destroy(player);

    }


    [UnityTest]
    public IEnumerator EnemyRestTest()
    {
        Vector2 expectedspeed = Vector2.zero;
        Assert.AreEqual(expectedspeed, enemy.GetComponent<EnemyController>().rbdy.velocity);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EnemyMovetoPlayerTest()
    {

        bool expectedresult;
        float currentX;
        float afterX;
 

        

        player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;


        player.transform.SetPositionAndRotation(new Vector3(5, 0, 0), new Quaternion(0, 0, 0, 0));
        enemy.transform.SetPositionAndRotation(new Vector3(9, 0, 0), new Quaternion(0, 0, 0, 0));
        currentX = enemy.transform.position.x;
        yield return new WaitForSeconds(1);
        Object.Destroy(player);
        afterX = enemy.transform.position.x;
        expectedresult = System.Math.Abs(currentX - afterX) <= 3;


        Assert.AreEqual(expectedresult, true);
          
    }




}

