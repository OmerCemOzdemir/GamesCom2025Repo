using UnityEngine;

public class TempPlaformItem : MonoBehaviour
{
    [SerializeField] private ClickerUpgradeItem item;

    public ClickerUpgradeItem Item { get => item; set => item = value; }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }

    public void OnPickUpItem()
    {
        Destroy(gameObject);
    }

}
