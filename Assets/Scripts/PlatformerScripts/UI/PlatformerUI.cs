using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlatformerUI : MonoBehaviour
{
    [Header("General GUI Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI lostMoneyText;
    [SerializeField] private GameObject keyImage;
    [SerializeField] private GameObject upgradeShopPanel;
    [SerializeField] private GameObject taxiPanel;
    [SerializeField] private GameObject outOfMoneyPanel;
    [Space(10)]

    [Header("Item Images")]
    [Tooltip("This Array holds the IMAGES of game itemsData:\n JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3")]
    [SerializeField] private GameObject[] itemSlotImages;
    [Space(10)]

    [Header("Item Buttons")]
    [Tooltip("This Array holds the BUTTONS of game itemsData:\n JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3")]
    [SerializeField] private GameObject[] itemSlotButtons;

    private void Awake()
    {
        UpdateMoneyText(0);
    }

    private void OnEnable()
    {
        PlatformerManager.onMoneyChange += UpdateLostMoney;
        PlayerControler.onPlayerPickUpKey += EnableKeyImage;
        PlayerControler.onPlayerOpenShop += OpenShop;
        UpgradeShop.onItemExchange += UpdateItemSlotImages;
        PlatformerManager.onMoneyZero += OpenOutOfMoneyPanel;
        PlayerControler.onPlayerGetInTaxi += OpenTaxi;
    }

    private void OnDisable()
    {
        PlatformerManager.onMoneyChange -= UpdateLostMoney;
        PlayerControler.onPlayerPickUpKey -= EnableKeyImage;
        PlayerControler.onPlayerOpenShop -= OpenShop;
        UpgradeShop.onItemExchange -= UpdateItemSlotImages;
        PlatformerManager.onMoneyZero -= OpenOutOfMoneyPanel;
        PlayerControler.onPlayerGetInTaxi -= OpenTaxi;

    }

    public void UpdateMoneyText(float money)
    {
        moneyText.text = "$" + GameManager.Instance.GetGameData().totalMoney;
    }

    private void DisableKeyImage()
    {
        keyImage.SetActive(true);
    }

    public void CloseShop()
    {
        upgradeShopPanel.SetActive(false);
    }

    private void OpenShop()
    {
        upgradeShopPanel.SetActive(true);
    }

    private void OpenTaxi()
    {
        taxiPanel.SetActive(true);
    }

    public void CloseTaxi()
    {
        taxiPanel.SetActive(false);
    }

    public void UpdateLostMoney(float money)
    {
        StopAllCoroutines();
        lostMoneyText.color = new Color(1, 0, 0, 1f);
        lostMoneyText.gameObject.SetActive(true);
        lostMoneyText.text = "-$" + money;
        StartCoroutine(DecreaseTransparency(2));
        UpdateMoneyText(money);
    }

    IEnumerator DecreaseTransparency(float sec)
    {
        Color originalColor = lostMoneyText.color;
        float startAlpha = originalColor.a;
        float elapsed = 0f;

        while (elapsed < sec)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / sec);
            lostMoneyText.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null; // wait for next frame
        }

        // Ensure it's fully transparent at the end
        lostMoneyText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    private void OpenOutOfMoneyPanel()
    {
        outOfMoneyPanel.SetActive(true);
    }

    public void Grind()
    {
        GameManager.Instance.NextLevel(1);
    }

    private void UpdateItemSlotImages(bool[] itemSlots)
    {
        for (int i = 0; itemSlots.Length > i; i++)
        {
            itemSlotImages[i].SetActive(itemSlots[i]);
            itemSlotButtons[i].GetComponent<Button>().interactable = !itemSlots[i];
            if (itemSlots[i])
            {
                itemSlotButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
        }
    }

    private void EnableKeyImage()
    {
        keyImage.SetActive(true);
    }

}

/*
 
 
        int i = 0;
        while (i < totalSec)
        {

            yield return new WaitForSeconds(sec);
            i++;
        }

        for (int i = 0; i < sec; i++)
        {
            lostMoneyText.color -= new Color(0, 0, 0, 0.1f);
            yield return new WaitForSeconds(sec);
        }


    IEnumerator FadeText(int timer, GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.black;
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1f);
            obj.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
            obj.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.1f);
        }

        yield return new WaitForSeconds(2f);

        Destroy(obj);
        obj = null;

    }



 */