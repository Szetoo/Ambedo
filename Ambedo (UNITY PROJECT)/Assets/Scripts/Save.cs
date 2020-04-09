using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


//Save file for the game and has all the important values for each player's save file. 
//Also has a function that can be used to save the information to the save file.
public class Save
{


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

    public Dictionary<string, bool> enemiesInLevel3 = new Dictionary<string, bool>(){
                                                {"Child Ghost (Enemy)", true},
                                                {"Octopus", true},
                                                {"Spider", true},
                                                {"SwampMonster", true},
                                                {"Flyer", true}
                                            };


    public bool cameraPanHasBeenActivated;
    
    


    public static Save CreateSaveObject(int gameLevel, float xPosition, float yPosition, bool isWielding, Dictionary<string, bool> enemies, Dictionary<string, bool> enemies2, Dictionary<string, bool> enemies3, float currentEXP, int currentLevel, bool trigger)
    {
        Save save = new Save();
        //player = GameObject.FindGameObjectWithTag("Player");
        save.saveFileLevel = gameLevel;
        save.xSpawnPosition = xPosition;
        save.ySpawnPosition = yPosition;
        save.isWielding = isWielding;
        save.enemiesInLevel1 = enemies;
        save.enemiesInLevel2 = enemies2;
        save.enemiesInLevel3 = enemies3;
        save.currentEXP = currentEXP;
        save.currentLevel = currentLevel;
        save.cameraPanHasBeenActivated = trigger;

        return save;
    }
}