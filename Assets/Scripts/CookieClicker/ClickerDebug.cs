using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerDebug : MonoBehaviour
{

    private Type type;
    private FieldInfo[] fieldInfos;
    private ClickerItemSaveData clickerItem;
    private ClickerItemSaveData[] clickerItemArray;
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private ClickerManager clickerManager;
    [SerializeField] private TextMeshProUGUI debugText;
    private bool toggle = false;

    private void Start()
    {
        clickerItem = new ClickerItemSaveData();
        clickerItemArray = GameManager.Instance.GetGameData().clickerItems;
        //PrintArray();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (toggle)
            {
                OpenDebugPanel();
                toggle = false;
            }
            else
            {
                CloseDebugPanel();
                toggle = true;
            }
        }
    }

    public void OpenDebugPanel()
    {
        debugPanel.SetActive(true);
        UpdateDebugText();
    }

    public void CloseDebugPanel()
    {
        debugPanel.SetActive(false);
    }

    private void UpdateDebugText()
    {
        debugText.text = clickerManager.PrintFields();

    }


    private void PrintArray()
    {
        for (int i = 0; i < clickerItemArray.Length; i++)
        {
            clickerItem = clickerItemArray[i];
            //Debug.Log("Name: " + clickerItemArray[i].name);
            type = clickerItem.GetType();
            fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int j = 0; j < fieldInfos.Length; j++)
            {
                Debug.Log(fieldInfos[j].Name + ": " + fieldInfos[j].GetValue(clickerItem));
            }


        }
    }

}
/*
         
 */