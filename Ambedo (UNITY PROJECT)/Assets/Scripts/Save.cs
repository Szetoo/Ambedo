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

    public Dictionary<string, bool> enemiesInLevel2 = new Dictionary<string, bool>(){
                                                {"Flyer1", true},
                                                {"Crab1", true},
                                                {"Octopus1", true},
                                                {"Flyer2", true},
                                                {"Statue", true}
                                            };


    public bool cameraPanHasBeenActivated;
    
    


    public static Save CreateSaveObject(int gameLevel, float xPosition, float yPosition, bool isWielding, Dictionary<string, bool> enemies, Dictionary<string, bool> enemies2, float currentEXP, int currentLevel, bool trigger)
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");
        save.saveFileLevel = gameLevel;
        save.xSpawnPosition = xPosition;
        save.ySpawnPosition = yPosition;
        save.isWielding = isWielding;
        save.enemiesInLevel1 = enemies;
        save.enemiesInLevel2 = enemies2;
        save.currentEXP = currentEXP;
        save.currentLevel = currentLevel;
        save.cameraPanHasBeenActivated = trigger;

        return save;
    }
}