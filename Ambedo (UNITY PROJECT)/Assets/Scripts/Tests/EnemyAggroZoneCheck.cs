using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class EnemyAggroZoneCheck
    {
    // A Test behaves as an ordinary method
    private GameObject player;
    private GameObject enemy;

    [SetUp]
	public void Setup()
	{
		enemy = Object.Instantiate(Resources.Load("Enemy2", typeof(GameObject))) as GameObject;
		player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
	}

	[TearDown]
	public void Teardown()
	{
		Object.Destroy(enemy);
		Object.Destroy(player);

	}

	[UnityTest]
	public IEnumerator CheckPlayerNerbyTrue()
	{

		bool expectedresult = true;

        //player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
        player.transform.SetPositionAndRotation(new Vector3(5,0, 0), new Quaternion(0, 0, 0, 0));
		enemy.transform.SetPositionAndRotation(new Vector3(9,0, 0), new Quaternion(0, 0, 0, 0));

		yield return null;
        //Object.Destroy(player);


        Assert.AreEqual(expectedresult, enemy.transform.GetChild(0).gameObject.GetComponent<EnemyAggroZone>().playerNearby);

    }


	[UnityTest]
	public IEnumerator CheckPlayerNerbyFalse()
	{

		bool expectedresult = false;

		//player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
		player.transform.SetPositionAndRotation(new Vector3(5, 0, 0), new Quaternion(0, 0, 0, 0));
		enemy.transform.SetPositionAndRotation(new Vector3(30, 0, 0), new Quaternion(0, 0, 0, 0));

		yield return new WaitForSeconds(1);
        //Object.Destroy(player);


        Assert.AreEqual(expectedresult, enemy.transform.GetChild(0).gameObject.GetComponent<EnemyAggroZone>().playerNearby);

    }

}
