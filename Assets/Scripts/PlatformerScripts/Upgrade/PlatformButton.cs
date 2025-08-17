using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<PlatformUpgradeItem, PlatformItemSaveData, bool> onMouseOverUpgradePlatform;
    int index;

    public void GetClickIndex()
    {
        index = int.Parse(transform.name);
        transform.root.GetComponent<PlatformUpgradeShop>().PlatformItemIndex = index;
        transform.root.GetComponent<PlatformUpgradeShop>().UpdateShopTexts();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseOverUpgradePlatform?.Invoke(transform.root.GetComponent<PlatformUpgradeShop>().UpgradeItems[index],
            transform.root.GetComponent<PlatformUpgradeShop>().UpgradeItemsData[index], true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseOverUpgradePlatform?.Invoke(transform.root.GetComponent<PlatformUpgradeShop>().UpgradeItems[index],
            transform.root.GetComponent<PlatformUpgradeShop>().UpgradeItemsData[index], false);

    }
}
