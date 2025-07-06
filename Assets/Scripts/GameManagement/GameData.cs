[System.Serializable]
public class GameData
{
    //Data For Multi-Scene;
    public bool newGame = true;
    public double totalMoney;
    public float maxTotalMoney;
    public int walletLevel = 0;

    //Data For only Platforming Game:
    public bool[] platformItems = new bool[4];

    //Data For only Cookie Clicker Game:
    public ClickerItemSaveData[] clickerItems;


}

/*
     public float ItemBaseIncome = 1.67f;
    public int ItemCount = 1;
    public int ItemMulti = 2;
 */