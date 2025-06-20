using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    //Data For Multi-Scene;
    public float totalMoney;
    public float maxTotalMoney;
    public int walletLevel = 0;

    //Data For only Platforming Game:
    public bool[] platformItems;
   // public bool jumpBoots = false;
   //public bool sprintBoots = false;
   // public bool springSoles = false;
    //public bool climbGloves = false;

    //Data For only Cookie Clicker Game:
    public float ItemBaseIncome = 1.67f;
    public int ItemCount = 1;
    public int ItemMulti = 2;

}
