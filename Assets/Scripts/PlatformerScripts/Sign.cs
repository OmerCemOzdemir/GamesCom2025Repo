using TMPro;
using UnityEditor;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] public bool nextLevel = true;
    [SerializeField] private int nextLevelIndex;

    private void Awake()
    {
        if (nextLevel)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Next Level";

        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Office";

        }
    }

    public void ChangeTextInInspector(string txt)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = txt;
    }


}


#if UNITY_EDITOR

[CustomEditor(typeof(Sign))]
public class SignCustomInspector : Editor
{
    SerializedProperty nextLevel;
    SerializedProperty nextLevelIndex;

    private void OnEnable()
    {
        nextLevel = serializedObject.FindProperty("nextLevel");
        nextLevelIndex = serializedObject.FindProperty("nextLevelIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(nextLevel);


        Sign sign = (Sign)target;
        if (sign.nextLevel)
        {
            sign.ChangeTextInInspector("Next Level");
        }
        else
        {
            sign.ChangeTextInInspector("Office");

        }


        serializedObject.ApplyModifiedProperties();
    }


}


#endif



