using System;
using UnityEditor;
using UnityEngine;

public class Taxi : MonoBehaviour
{

    //[SerializeField] private Level[] levels;
    [SerializeField] private GameObject checkpointPrefab;
    private Vector3[] checkpointPositions;
    public static event Action<Vector3[]> onCheckpointLoad;

    private void Awake()
    {
        checkpointPositions = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            checkpointPositions[i] = transform.GetChild(i).gameObject.transform.position;
        }
        //printArr();
    }

    private void Start()
    {
        try
        {
            onCheckpointLoad?.Invoke(checkpointPositions);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error when invoking onCheckpointLoad: " + ex);
        }
    }

    private void printArr()
    {
        foreach (var item in checkpointPositions)
        {
            Debug.Log(item);
        }
    }

    public void CreateCheckpoint()
    {

        GameObject obj = Instantiate(checkpointPrefab);
        obj.transform.SetParent(transform);
        obj.transform.position = Vector3.zero;

    }


}

#if UNITY_EDITOR

[CustomEditor(typeof(Taxi))]
public class TaxiInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Taxi taxi = (Taxi)target;
        EditorGUILayout.LabelField("Create CheckpointButton Field: ");
        if (GUILayout.Button("Add CheckpointButton", GUILayout.Width(360f)))
        {
            taxi.CreateCheckpoint();
            Debug.Log("Taxi Inspector Button Works");
        }

    }


}


#endif



/*


        foreach (var item in checkpoints)
        {
            Debug.Log(item);
        }



 */