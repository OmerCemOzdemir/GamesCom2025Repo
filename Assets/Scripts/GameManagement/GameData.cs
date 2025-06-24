[System.Serializable]
public class GameData
{
    //Data For Multi-Scene;
    public float totalMoney;
    public float maxTotalMoney;
    public int walletLevel = 0;

    //Data For only Platforming Game:
    public bool[] platformItems = new bool[4];

    //Data For only Cookie Clicker Game:
    public float ItemBaseIncome = 1.67f;
    public int ItemCount = 1;
    public int ItemMulti = 2;
}
