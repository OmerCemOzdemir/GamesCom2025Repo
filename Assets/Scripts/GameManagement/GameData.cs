[System.Serializable]
public class GameData
{
    //Data For Multi-Scene;
    public bool clickerNewGame = true;
    public bool platformNewGame = true;
    public bool levelNewGame = true;
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


}

/*
     public float ItemBaseIncome = 1.67f;
    public int ItemCount = 1;
    public int ItemMulti = 2;
 */