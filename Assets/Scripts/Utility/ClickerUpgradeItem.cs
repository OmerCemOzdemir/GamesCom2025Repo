using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateClickerItem", menuName = "ScriptableObjects/CreateClickerItem")]
public class ClickerUpgradeItem : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescription;
    [SerializeField] public Sprite[] itemIcon;

    [SerializeField] public bool itemUnlocked = true;
    [SerializeField] public int maxTier = 0;
    [SerializeField] public ClickerItemEffetors itemEffector;
    [SerializeField] public Operations itemOperationOnItemEffector;
    [SerializeField] public CustomItemTier[] itemTiers;

}

public enum ClickerItemEffetors
{
    baseActiveMoneyIncrement,
    baseActiveMoneyMultiplier,
    baseIdleMoneyIncrement,
    baseIdleMoneyMultiplier,
    baseIdleTime,
    walletLevel
}

public enum Operations
{
    Multiply,
    Divide,
    Add,
    Subtract,
    Null
}



#if UNITY_EDITOR

[CustomEditor(typeof(ClickerUpgradeItem))]
public class ClickerUpgradeItemCustomInspector : Editor
{
    SerializedProperty itemName;
    SerializedProperty itemDescription;
    SerializedProperty itemIcon;

    SerializedProperty itemUnlocked;
    SerializedProperty maxTier;
    SerializedProperty itemEffector;
    SerializedProperty itemOperationOnItemEffector;
    SerializedProperty itemTiers;




    private void OnEnable()
    {
        itemName = serializedObject.FindProperty("itemName");
        itemDescription = serializedObject.FindProperty("itemDescription");
        itemIcon = serializedObject.FindProperty("itemIcon");

        itemUnlocked = serializedObject.FindProperty("itemUnlocked");
        maxTier = serializedObject.FindProperty("maxTier");
        itemEffector = serializedObject.FindProperty("itemEffector");
        itemOperationOnItemEffector = serializedObject.FindProperty("itemOperationOnItemEffector");
        itemTiers = serializedObject.FindProperty("itemTiers");


    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(itemName);
        EditorGUILayout.PropertyField(itemDescription);
        EditorGUILayout.PropertyField(itemIcon);

        EditorGUILayout.PropertyField(itemUnlocked);
        EditorGUILayout.PropertyField(itemEffector);
        EditorGUILayout.PropertyField(itemOperationOnItemEffector);
        EditorGUILayout.PropertyField(itemTiers);

        ClickerUpgradeItem item = (ClickerUpgradeItem)target;

        item.maxTier = item.itemTiers.Length;
        EditorGUILayout.LabelField("Max Tier Size: " + item.maxTier);


        serializedObject.ApplyModifiedProperties();
    }
}


#endif



/*
 
    //public float baseItemEffect;
    //public float baseItemCost;
    public float[] tierMultiplier;
    public float[] costMultiplier;
 
    [SerializeField] private float baseActiveMoneyIncrement = 1; //Defualt is 1
    [SerializeField] private float baseActiveMoneyMultiplier = 1; //Defualt is 1
    [SerializeField] private float baseIdleMoneyIncrement = 0; //Defualt is 0
    [SerializeField] private float baseIdleMoneyMultiplier = 1; //Defualt is 1


    [Tooltip("Increase this to longer the time of idle money")]
    [SerializeField] private float baseIdleTime = 1;
    [SerializeField] private float baseActiveTime = 1;
 
 */