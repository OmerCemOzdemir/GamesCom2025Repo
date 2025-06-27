using System;
using UnityEngine;

public class ClickerButton : MonoBehaviour
{
    public void GetClickIndex()
    {
        int index = int.Parse(transform.name);
        transform.root.GetComponent<ClickerUpgrade>().ClickerIndex = index;
        transform.root.GetComponent<ClickerUpgrade>().Buy();
    }

}
