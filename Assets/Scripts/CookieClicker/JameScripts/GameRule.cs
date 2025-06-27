using System;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    
    // Reference to the GameData passed by the GameManager
     
    //public float ItemIncome(int itemCount, int bonusMultiplier) => GameManager.Instance.GetGameData().ItemBaseIncome * itemCount * bonusMultiplier;
    
    public void AddItemLvl(int Lvl)
    {
        //GameManager.Instance.GetGameData().ItemCount += Lvl;
    }

    public void IncreaseScore()
    {
       //GameManager.Instance.GetGameData().totalMoney += ItemIncome(GameManager.Instance.GetGameData().ItemCount, GameManager.Instance.GetGameData().ItemMulti);
    }
}
