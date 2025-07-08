using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "CreateItem", menuName = "ScriptableObjects/CreateItem")]
public class UpgradeItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public bool itemUnlocked = true;
    public int maxTier = 0;
    public Sprite itemIcon;
    public ItemEffetors itemEffector;
    public Operations itemOperationOnItemEffector;
    public float baseItemEffect;
    public float[] tierMultiplier;
    public float baseItemCost;
    public float[] costMultiplier;

}

public enum ItemEffetors
{
    baseActiveMoneyIncrement,
    baseActiveMoneyMultiplier,
    baseIdleMoneyIncrement,
    baseIdleMoneyMultiplier,
    baseIdleTime
}

public enum Operations
{
    Multiply,
    Divide,
    Add,
    Subtract,
}

/*
 
 
    [SerializeField] private float baseActiveMoneyIncrement = 1; //Defualt is 1
    [SerializeField] private float baseActiveMoneyMultiplier = 1; //Defualt is 1
    [SerializeField] private float baseIdleMoneyIncrement = 0; //Defualt is 0
    [SerializeField] private float baseIdleMoneyMultiplier = 1; //Defualt is 1


    [Tooltip("Increase this to longer the time of idle money")]
    [SerializeField] private float baseIdleTime = 1;
    [SerializeField] private float baseActiveTime = 1;
 
 */