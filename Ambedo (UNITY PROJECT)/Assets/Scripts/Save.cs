using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Save
{
    // public List<(int,int)> livingTargetPositions = new List<(int,int)>();
    // public List<int> livingTargetsTypes = new List<int>();

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




}