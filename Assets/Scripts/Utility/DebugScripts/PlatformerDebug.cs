using System.Collections;
using TMPro;
using UnityEngine;

public class PlatformerDebug : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private PlatformerUI platformerUI;
    [SerializeField] private bool infiMoney;
    public void GiveMoney()
    {
        GameManager.Instance.GetGameData().totalMoney += 1000;
        platformerUI.UpdateMoneyText(0);
        //PlatformerManager.moneyBelowZero = false;
    }

    private void Start()
    {
        if (infiMoney)
        {
            GameManager.Instance.GetGameData().totalMoney += 999999;
        }
    }

    private void OnEnable()
    {
        PlayerControler.onPlayerDebug += UpdateDebugText;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerDebug -= UpdateDebugText;
    }

    private void UpdateDebugText(string txt)
    {
        StopAllCoroutines();
        debugPanel.gameObject.SetActive(true);
        debugPanel.GetComponentInChildren<TextMeshProUGUI>().text = txt;
        StartCoroutine(CloseOnDelay(10));
    }

    IEnumerator CloseOnDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        debugPanel.gameObject.SetActive(false);
    }

}
