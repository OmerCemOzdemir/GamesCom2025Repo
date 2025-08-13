using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private GameObject checkpointPrefab;
    private Vector3[] checkpointPositions;
    //public static event Action<Vector3[]> onCheckpointLoad;
    private List<string> levelPaths = new List<string>();
    private string[] levelName;


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
        int lenght = SceneManager.sceneCountInBuildSettings;
        //Debug.Log("Total Scene Count " + lenght);
        string[] files;
        files = Directory.GetFiles("Assets/Scenes/PlatformScenes");

        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".meta"))
            {
                levelPaths.Add(files[i].Replace('\\', '/'));
                //Debug.Log("The path: " + files[i]);
            }
        }

        levelName = new string[levelPaths.Count];

        for (int i = 0; i < levelName.Length; i++)
        {
            levelName[i] = levelPaths[i].Replace("Assets/Scenes/PlatformScenes/", "");
            levelName[i] = levelName[i].Replace(".unity", "");
        }
        //SceneUtility.GetBuildIndexByScenePath(levelPaths[levelIndex])

        LevelSaveData[] levelSaveData = new LevelSaveData[levelName.Length];

        for (int i = 0; i < levelSaveData.Length; i++)
        {
            Debug.Log("Save Level Data" + i);
            levelSaveData[i] = new LevelSaveData();
            levelSaveData[i].levelName = levelName[i];
            if (i == 0) { levelSaveData[0].firstLevel = true; }
            levelSaveData[i].unlock = false;
            levelSaveData[i].levelIndex = SceneUtility.GetBuildIndexByScenePath(levelPaths[i]);

        }

        //levelSaveData[0].unlock = true; 
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