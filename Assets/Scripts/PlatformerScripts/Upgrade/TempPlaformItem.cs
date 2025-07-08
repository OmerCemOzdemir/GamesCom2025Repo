using UnityEngine;

public class TempPlaformItem : MonoBehaviour
{
    [SerializeField] private UpgradeItem item;

    public UpgradeItem Item { get => item; set => item = value; }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }

    public void OnPickUpItem()
    {
        Destroy(gameObject);
    }

}
