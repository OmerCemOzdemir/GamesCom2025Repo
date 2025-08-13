using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    //Data For Multi-Scene;
    public bool startNewGame = true;
    public bool clickerNewGame = true;
    public bool platformNewGame = true;
    public bool levelNewGame = true;
    public bool IntroCutscenePlayed = false;
    public bool clickerTutorialPlayed = false;
    public bool PlatformerTutorialPlayed = false;
    public double totalMoney;
    public double maxTotalMoney;
    public int walletLevel = 0;
    public LevelSaveData[] levelData;

    //Data For only Platforming Game:
    public PlatformItemSaveData[] platformItems;
    public float checkpointX;
    public float checkpointY;
    public float checkpointZ;
    public bool checkpointEnable = false;

    //Data For only Cookie Clicker Game:
    public ClickerItemSaveData[] clickerItems;

    // Tutorial Data
    public List<string> visitedInteractables = new List<string>();

}

/*
     public float ItemBaseIncome = 1.67f;
    public int ItemCount = 1;
    public int ItemMulti = 2;
 */