using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<ClickerUpgradeItem, bool> onMouseOverUpgrade;
    private int index;
    public void GetClickIndex()
    {
        index = int.Parse(transform.name);
        transform.root.GetComponent<ClickerUpgrade>().ClickerIndex = index;
        transform.root.GetComponent<ClickerUpgrade>().Buy();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        index = int.Parse(transform.name);
        onMouseOverUpgrade?.Invoke(transform.root.GetComponent<ClickerUpgrade>().upgradeItems[index], true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        index = int.Parse(transform.name);
        onMouseOverUpgrade?.Invoke(transform.root.GetComponent<ClickerUpgrade>().upgradeItems[index], false);
    }
}
