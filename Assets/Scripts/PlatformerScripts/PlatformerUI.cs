using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlatformerUI : MonoBehaviour
{
    [Header("General GUI Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI lostMoneyText;
    [SerializeField] private GameObject outOfMoneyPanel;
    [SerializeField] private Transform itemsPanel;
    [SerializeField] private GameObject itemIconPrefab;
    private GameObject[] itemIconArr;
    [Space(10)]

    private List<PlatformUpgradeItem> upgradeItems = new List<PlatformUpgradeItem>();


    private void Awake()
    {
        InitilizeScriptableObjects();
        UpdateMoneyText(0);
        UpdateItemIcons();
    }

    private void OnEnable()
    {
        PlatformerManager.onMoneyChange += UpdateLostMoney;
        PlatformerManager.onMoneyZero += OpenOutOfMoneyPanel;
    }

    private void OnDisable()
    {
        PlatformerManager.onMoneyChange -= UpdateLostMoney;
        PlatformerManager.onMoneyZero -= OpenOutOfMoneyPanel;

    }

    public void UpdateMoneyText(float money)
    {
        moneyText.text = "$" + GameManager.Instance.GetGameData().totalMoney;
    }


    private void InitilizeScriptableObjects()
    {
        //Assets/ScriptableObjects/PlatformItems
        string[] files;
        files = Directory.GetFiles("Assets/ScriptableObjects/PlatformItems");
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".meta"))
            {
                upgradeItems.Add(AssetDatabase.LoadAssetAtPath<PlatformUpgradeItem>(files[i]));
                //Debug.Log("Test: " + files[i]);
            }

        }

        foreach (var item in upgradeItems)
        {
            //Debug.Log("Test: " + item.name);
        }

    }

    private void UpdateItemIcons()
    {
        PlatformItemSaveData[] itemSaveData = GameManager.Instance.GetGameData().platformItems;
        itemIconArr = new GameObject[itemSaveData.Length];
        for (int i = 0; i < itemSaveData.Length; i++)
        {
            itemIconArr[i] = Instantiate(itemIconPrefab);
            itemIconArr[i].transform.SetParent(itemsPanel);

            if (itemSaveData[i].unlock)
            {
                itemIconArr[i].GetComponent<Image>().sprite = upgradeItems[i].itemIcon;

            }
            else
            {
                itemIconArr[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            //itemIcon.GetComponent<Image>().sprite = 
        }

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


}

/*
 

    public void CloseShop()
    {
        upgradeShopPanel.SetActive(false);
    }

    private void OpenShop()
    {
        upgradeShopPanel.SetActive(true);
    }


     private void DisableKeyImage()
    {
        keyImage.SetActive(true);
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




    [Header("Item Images")]
    [Tooltip("This Array holds the IMAGES of game upgradeItemsData:\n JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3")]
    [SerializeField] private GameObject[] itemSlotImages;
    [Space(10)]

    [Header("Item Buttons")]
    [Tooltip("This Array holds the BUTTONS of game upgradeItemsData:\n JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3")]
    [SerializeField] private GameObject[] itemSlotButtons;



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