using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level3PuzzleCode : MonoBehaviour
{
    private int[] password;
    public int BossOrLevel;
    private int index = 0;
    public GameObject Boss;
    private GameObject Summoner;
    public GameObject position;


    private 

    // Start is called before the first frame update
    void Start()
    {
        if (BossOrLevel == 1)  password = new int[] { 2, 1, 3, 1, 3, 2, 2};
        else password = new int[] { 2,2,1,3 };
    }

    void Update()
    {
        if (BossOrLevel == 2) if (Boss == null) Destroy(gameObject.transform.GetChild(0));
    }


    public void CheckPassword(int code )
    {
        if (password[index] != code)
        {
            index = 0;
            if (BossOrLevel == 1)
            {
                Summoner = Instantiate(Boss, position.transform.position, Quaternion.Euler(0, 0f, 0f));
                Destroy(Summoner, 5f);
            }
        }
        else
        {
            index++;
            if (index == password.Length) passFunction();
        }
        Debug.Log(index);
        Debug.Log(code);
    }

    private void passFunction()
    {
        if (BossOrLevel == 1) gameObject.transform.GetChild(0).gameObject.SetActive(true);
        else if(Boss != null) gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }



}
