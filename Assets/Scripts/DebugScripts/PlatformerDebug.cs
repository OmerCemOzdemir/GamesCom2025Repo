using UnityEngine;

public class PlatformerDebug : MonoBehaviour
{
    [SerializeField] private PlatformerUI platformerUI;

    public void GiveMoney()
    {
        GameManager.Instance.GetGameData().totalMoney += 1000;
        platformerUI.UpdateMoneyText();
        //PlatformerManager.moneyBelowZero = false;
    }

        

}
