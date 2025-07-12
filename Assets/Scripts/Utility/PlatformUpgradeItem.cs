using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateClickerItem", menuName = "ScriptableObjects/CreatePlatformItem")]
public class PlatformUpgradeItem : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescription;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public float itemCost;
    [SerializeField] public PlatformItemType itemType = PlatformItemType.Permanent;

    [SerializeField] public bool hasItemEffectOnMoney = true;
    [SerializeField] public bool hasItemEffectOnMovement = false;

    [SerializeField] public CustomItemMoneyPair itemEffectOnMoney;
    [SerializeField] public CustomItemMovementPair itemEffectOnMovement;
    [SerializeField] public float itemEffectSpecial;


    [SerializeField] public bool hasTier = false;
    [SerializeField] public float[] effectTiersPercentage;
    [SerializeField] public float[] costTiers;

}

public enum PlatformItemType
{
    Permanent,
    Temporary,
    RepeatPurchase
}



#if UNITY_EDITOR

[CustomEditor(typeof(PlatformUpgradeItem))]
public class PlatformUpgradeItemCustomInspector : Editor
{
    SerializedProperty itemName;
    SerializedProperty itemDescription;
    SerializedProperty itemIcon;
    SerializedProperty itemCost;
    SerializedProperty itemType;

    SerializedProperty hasItemEffectOnMoney;
    SerializedProperty hasItemEffectOnMovement;
    SerializedProperty itemEffectOnMoney;
    SerializedProperty itemEffectOnMovement;
    SerializedProperty itemEffectSpecial;


    SerializedProperty hasTier;
    SerializedProperty effectTiers;
    SerializedProperty costTiers;


    private void OnEnable()
    {
        itemName = serializedObject.FindProperty("itemName");
        itemDescription = serializedObject.FindProperty("itemDescription");
        itemIcon = serializedObject.FindProperty("itemIcon");
        itemCost = serializedObject.FindProperty("itemCost");
        itemType = serializedObject.FindProperty("itemType");

        hasItemEffectOnMoney = serializedObject.FindProperty("hasItemEffectOnMoney");
        hasItemEffectOnMovement = serializedObject.FindProperty("hasItemEffectOnMovement");
        itemEffectOnMoney = serializedObject.FindProperty("itemEffectOnMoney");
        itemEffectOnMovement = serializedObject.FindProperty("itemEffectOnMovement");
        itemEffectSpecial = serializedObject.FindProperty("itemEffectSpecial");


        hasTier = serializedObject.FindProperty("hasTier");
        effectTiers = serializedObject.FindProperty("effectTiersPercentage");
        costTiers = serializedObject.FindProperty("costTiers");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(itemName);
        EditorGUILayout.PropertyField(itemDescription);
        EditorGUILayout.PropertyField(itemIcon);
        EditorGUILayout.PropertyField(itemCost);
        EditorGUILayout.PropertyField(itemType);
        EditorGUILayout.PropertyField(hasTier);
        EditorGUILayout.PropertyField(hasItemEffectOnMoney);
        EditorGUILayout.PropertyField(hasItemEffectOnMovement);
        
        //EditorGUILayout.PropertyField(itemEffectOnMoney);


        PlatformUpgradeItem platformUpgradeItem = (PlatformUpgradeItem)target;

        if (platformUpgradeItem.hasItemEffectOnMoney)
        {
            EditorGUILayout.PropertyField(itemEffectOnMoney);
        }

        if (platformUpgradeItem.hasItemEffectOnMovement)
        {
            EditorGUILayout.PropertyField(itemEffectOnMovement);
        }

        if (!platformUpgradeItem.hasItemEffectOnMovement && !platformUpgradeItem.hasItemEffectOnMoney)
        {
            EditorGUILayout.PropertyField(itemEffectSpecial);
        }


        if (platformUpgradeItem.hasTier)
        {
            EditorGUILayout.PropertyField(effectTiers);
            EditorGUILayout.PropertyField(costTiers);
        }

        serializedObject.ApplyModifiedProperties();
    }
}


#endif

/*
     
 
 */


