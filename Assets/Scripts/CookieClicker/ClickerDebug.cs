using System;
using System.Reflection;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickerDebug : MonoBehaviour
{

    private Type type;
    private FieldInfo[] fieldInfos;
    private ClickerItem clickerItem;
    private ClickerItem[] clickerItemArray;

    private void Start()
    {
        clickerItem = new ClickerItem();
        clickerItemArray = GameManager.Instance.GetGameData().clickerItems;
        //PrintArray();
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