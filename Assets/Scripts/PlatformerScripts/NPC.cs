using TMPro;
using UnityEditor;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Information: ")]
    [SerializeField] public string nameNPC;
    [TextArea]
    [SerializeField] public string dialogNPC;

    [SerializeField] public bool itemDrop = false;
    [SerializeField] public GameObject item;


}



#if UNITY_EDITOR

[CustomEditor(typeof(NPC))]
public class NPCCustomInspector : Editor
{
    SerializedProperty nameNPC;
    SerializedProperty dialogNPC;
    SerializedProperty itemDrop;
    SerializedProperty item;



    private void OnEnable()
    {
        nameNPC = serializedObject.FindProperty("nameNPC");
        dialogNPC = serializedObject.FindProperty("dialogNPC");
        itemDrop = serializedObject.FindProperty("itemDrop");
        item = serializedObject.FindProperty("item");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(nameNPC);
        EditorGUILayout.PropertyField(dialogNPC);
        EditorGUILayout.PropertyField(itemDrop);

        NPC npc = (NPC)target;
        if (npc.itemDrop)
        {
            EditorGUILayout.PropertyField(item);
        }

        serializedObject.ApplyModifiedProperties();
    }


}


#endif


/*
 

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    private void Start()
    {
        nameText.text = nameNPC;
        dialogText.text = dialogNPC;
    }


     private void EnableText()
    {
        dialogBox.SetActive(true);
    }

    private void DisableText()
    {
        dialogBox.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { EnableText(); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { DisableText(); }

    }
 
 */