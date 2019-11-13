[System.Serializable]
public class GameData
{
    //Player Data
    public int playerHealth;
    public bool sword;
    public bool weild;
    public float[] player_pos;

    //Savepoint Data
    public int sp_Num;
    public float[] sp_pos;
    public int sp_playerHealth;

    //Game Data
    public int level;
    public bool enemy1_Spawn;
    public bool enemy2_Spawn;
    public bool boss_Spawn;

    public GameData(int playerHealth_in, bool sword_in, bool weild_in, float[] player_pos_in, int sp_Num_in, float[] sp_pos_in, int sp_playerHealth_in, int level_in, bool enemy1_Spawn_in, bool enemy2_Spawn_in, bool boss_Spawn_in)
    {
        //Player Data
        playerHealth = playerHealth_in;
        sword = sword_in;
        weild = weild_in;
        player_pos = player_pos_in;

        //Savepoint Data
        sp_Num = sp_Num_in;
        sp_pos = sp_pos_in;
        sp_playerHealth = sp_playerHealth_in;

        //Game Data
        level = level_in;
        enemy1_Spawn = enemy1_Spawn_in;
        enemy2_Spawn = enemy2_Spawn_in;
        boss_Spawn = boss_Spawn_in;
    }
}