using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData_IO : MonoBehaviour
{

    //Player Data
    public int playerHealth = 0;
    public bool sword = false;
    public bool weild = false;
    public float[] player_pos = new float[2] { 0,0 };

    //Savepoint Data
    public int sp_Num = 0;
    public float[] sp_pos = new float[2] { 0, 0 };
    public int sp_playerHealth = 0;

    //Game Data
    public int level = 0;
    public bool enemy1_Spawn = true;
    public bool enemy2_Spawn = true;
    public bool boss_Spawn = true;


    void Start()
    {
        SaveFile();
        LoadFile();
    }

    public void SaveFile()
    {
        string destination = "Assets/Scripts" + "/GameData.dat";
        FileStream file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
        GameData data = new GameData(playerHealth, sword, weild, player_pos, sp_Num, sp_pos, sp_playerHealth, level, enemy1_Spawn, enemy2_Spawn, boss_Spawn);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = "Assets/Scripts" + "/GameData.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            //Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        playerHealth = data.playerHealth;
        sword = data.sword;
        weild = data.weild;
        player_pos = data.player_pos;
        sp_Num = data.sp_Num;
        sp_pos = data.sp_pos;
        sp_playerHealth = data.sp_playerHealth;
        level = data.level;
        enemy1_Spawn = data.enemy1_Spawn;
        enemy2_Spawn = data.enemy2_Spawn;
        boss_Spawn = data.boss_Spawn;

    //Debug.Log(data.name);
    //Debug.Log(data.score);
    //Debug.Log(data.timePlayed);
}

}