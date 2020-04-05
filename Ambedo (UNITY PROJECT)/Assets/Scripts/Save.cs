using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Save
{
    // public List<(int,int)> livingTargetPositions = new List<(int,int)>();
    // public List<int> livingTargetsTypes = new List<int>();


    public int saveFileLevel;
    public float xSpawnPosition;
    public float ySpawnPosition;
    public bool isWielding;
    public int currentLevel;
    public float currentEXP;

    public Dictionary<string, bool> enemiesInLevel1 = new Dictionary<string, bool>(){
                                                {"Spider1", true},
                                                {"SwampMonster1", true},
                                                {"SwampMonster2", true},
                                                {"Spider2", true}
                                            };

    public bool cameraPanHasBeenActivated;
    
    


    public Save CreateSaveObject(int gameLevel, float xPosition, float yPosition, bool isWielding, Dictionary<string, bool> enemies, float currentEXP, int currentLevel, bool trigger)
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");
        save.saveFileLevel = gameLevel;
        save.xSpawnPosition = xPosition;
        save.ySpawnPosition = yPosition;
        save.isWielding = isWielding;
        save.enemiesInLevel1 = enemies;
        save.currentEXP = currentEXP;
        save.currentLevel = currentLevel;
        save.cameraPanHasBeenActivated = trigger;

        return save;
    }
}