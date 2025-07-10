using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public void GetClickIndex()
    {
        int index = int.Parse(transform.name);
        transform.root.GetComponent<PlatformUpgradeShop>().PlatformItemIndex = index;
        transform.root.GetComponent<PlatformUpgradeShop>().UpdateShopTexts();

    }
}
