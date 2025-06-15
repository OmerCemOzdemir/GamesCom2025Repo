using UnityEngine;

public class PlatformerDebug : MonoBehaviour
{
    [SerializeField] private PlatformerUI platformerUI;
    [SerializeField] private bool infiMoney;
    public void GiveMoney()
    {
        GameManager.Instance.GetGameData().totalMoney += 1000;
        platformerUI.UpdateMoneyText();
        //PlatformerManager.moneyBelowZero = false;
    }

    private void Start()
    {
        if (infiMoney)
        {
            GameManager.Instance.GetGameData().totalMoney += 999999;
        }
    }

}
