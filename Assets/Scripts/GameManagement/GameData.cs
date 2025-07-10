[System.Serializable]
public class GameData
{
    //Data For Multi-Scene;
    public bool clickerNewGame = true;
    public bool platformNewGame = true;
    public double totalMoney;
    public float maxTotalMoney;
    public int walletLevel = 0;
    public LevelSaveData[] levelData;

    //Data For only Platforming Game:
    public bool[] _platformItems = new bool[4];
    public PlatformItemSaveData[] platformItems;

    //Data For only Cookie Clicker Game:
    public ClickerItemSaveData[] clickerItems;


}

/*
     public float ItemBaseIncome = 1.67f;
    public int ItemCount = 1;
    public int ItemMulti = 2;
 */