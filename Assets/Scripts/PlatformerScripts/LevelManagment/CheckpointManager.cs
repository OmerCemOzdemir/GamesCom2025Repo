using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private GameObject checkpointPrefab;
    [SerializeField] private LevelEntry[] levels;
    private Vector3[] checkpointPositions;
    //public static event Action<Vector3[]> onCheckpointLoad;
    private List<string> levelPaths = new List<string>();
    private string[] levelName;

    [System.Serializable]
    public struct LevelEntry {
        public string sceneName;   // must match the Scene name in Build Settings
        public int sceneIndex;     // build index from Build Settings
        public bool firstLevel;    // true only for the very first level
    }
    

    private void Awake()
    {
        InitializeCheckpoints();
        InitilizeSaveData();
    }

    private void InitializeCheckpoints()
    {
        checkpointPositions = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            checkpointPositions[i] = transform.GetChild(i).gameObject.transform.position;
        }
    }


    private void InitilizeSaveData()
    {
        //Debug.Log("Current LevelButton: " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "MainHubScene")
        {
            if (GameManager.Instance.GetGameData().levelNewGame)
            {
                Debug.Log(" InitilizeLevelData ");
                InitilizeLevelData();
                GameManager.Instance.GetGameData().levelNewGame = false;
            }
            //PrintArr();
        }
        else
        {
            LevelSaveData[] levelSaveData = GameManager.Instance.GetGameData().levelData;
            for (int i = 0; i < levelSaveData.Length; i++)
            {
                if (levelSaveData[i].levelName == SceneManager.GetActiveScene().name)
                {
                    levelSaveData[i].unlock = true;
                    levelSaveData[i].checkpointX = new float[checkpointPositions.Length];
                    for (int j = 0; j < levelSaveData[i].checkpointX.Length; j++)
                    {
                        levelSaveData[i].checkpointX[j] = checkpointPositions[j].x;
                    }

                    levelSaveData[i].checkpointY = new float[checkpointPositions.Length];
                    for (int j = 0; j < levelSaveData[i].checkpointY.Length; j++)
                    {
                        levelSaveData[i].checkpointY[j] = checkpointPositions[j].y;

                    }

                    levelSaveData[i].checkpointZ = new float[checkpointPositions.Length];
                    for (int j = 0; j < levelSaveData[i].checkpointZ.Length; j++)
                    {
                        levelSaveData[i].checkpointZ[j] = checkpointPositions[j].z;

                    }
                    levelSaveData[i].unlockCheckpoint = new bool[checkpointPositions.Length];
                    for (int j = 0; j < levelSaveData[i].checkpointZ.Length; j++)
                    {
                        levelSaveData[i].unlockCheckpoint[j] = false;

                    }

                }
            }
            //PrintArr();
            GameManager.Instance.GetGameData().levelData = levelSaveData;
        }
    }

    private void InitilizeLevelData()
    {
        var data = GameManager.Instance.GetGameData().levelData;
        
        if (data != null && data.Length > 0)
            return;

        LevelSaveData[] levelSaveData = new LevelSaveData[levels.Length];
        for (int i = 0; i < levelSaveData.Length; i++)
        {
            levelSaveData[i] = new LevelSaveData();
            levelSaveData[i].levelName = levels[i].sceneName;
            levelSaveData[i].levelIndex = levels[i].sceneIndex;
            levelSaveData[i].firstLevel = i == 0 || levels[i].firstLevel;
            
            levelSaveData[i].unlock = i == 0 || levels[i].firstLevel; // force unlock first level
        }
        GameManager.Instance.GetGameData().levelData = levelSaveData;
        GameManager.Instance.SaveGame();
    }


    private void PrintArr()
    {
        LevelSaveData[] levelSaveData = GameManager.Instance.GetGameData().levelData;
        foreach (var item in levelSaveData)
        {
            //Debug.Log("LevelButton Data: " + item.levelName + " " + item.levelIndex);
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

[CustomEditor(typeof(CheckpointManager))]
public class CustomCheckpointManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CheckpointManager checkpointManager = (CheckpointManager)target;
        EditorGUILayout.LabelField("Create CheckpointButton Field: ");
        if (GUILayout.Button("Add CheckpointButton", GUILayout.Width(360f)))
        {
            checkpointManager.CreateCheckpoint();
            Debug.Log("Taxi Inspector Button Works");
        }

    }


}


#endif

/*
         try
        {
            onCheckpointLoad?.Invoke(checkpointPositions);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error when invoking onCheckpointLoad: " + ex);
        }
 
    private void printArr()
    {
        foreach (var item in checkpointPositions)
        {
            Debug.Log(item);
        }
    }

 */