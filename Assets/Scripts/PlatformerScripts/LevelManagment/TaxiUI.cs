using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TaxiUI : MonoBehaviour
{
    public static event Action<Vector3, float> onPlayerTravel;

    [Header("Gameobject Setup: ")]
    [SerializeField] private Transform checkpointButtonParent;
    [SerializeField] private GameObject checkpointButtonPrefab;
    [SerializeField] private Transform levelButtonParent;
    [SerializeField] private GameObject levelButtonPrefab;
    [Space(10)]
    [Header("UI: ")]
    [SerializeField] private GameObject taxiPanel;
    [SerializeField] private TextMeshProUGUI moneyRequired;
    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private GameObject checkpointsPanel;
    [Space(10)]
    [Header("Stats: ")]
    [SerializeField] private float baseTravelCostLevel = 10000;
    [SerializeField] private float baseTravelCostCheckpoint = 1000;

    private PlatformerUI platformerUI;

    //Checkpoint Var:
    private Button[] checkpointButtons;
    private Vector3[] checpointPositions;
    private int checkpointIndex = 0;
    public int CheckpointIndex { get => checkpointIndex; set => checkpointIndex = value; }

    //Level Var:
    private Button[] levelButtons;
    private List<string> levelPaths = new List<string>();
    private int levelIndex = 0;
    private string[] levelName;
    public int LevelIndex { get => levelIndex; set => levelIndex = value; }

    //Dictionary<int, Button> checkpointButtons = new Dictionary<int, Button> { };

    private void OnEnable()
    {
        PlayerControler.onPlayerGetInTaxi += OpenTaxi;
        Taxi.onCheckpointLoad += SetupCheckpoints;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerGetInTaxi -= OpenTaxi;
        Taxi.onCheckpointLoad -= SetupCheckpoints;
    }


    private void Awake()
    {
        platformerUI = GetComponent<PlatformerUI>();
        GetSceneInArray();
        SetupLevels();
    }

    #region UI

    private void OpenTaxi()
    {
        taxiPanel.SetActive(true);
    }

    public void CloseTaxi()
    {
        taxiPanel.SetActive(false);
    }

    public void OpenLevelsPanel()
    {
        levelsPanel.SetActive(true);
        CloseCheckpointsPanel();
    }

    public void OpenCheckpointsPanel()
    {
        checkpointsPanel.SetActive(true);
        CloseLevelsPanel();
    }

    public void CloseLevelsPanel()
    {
        levelsPanel.SetActive(false);
    }

    public void CloseCheckpointsPanel()
    {
        checkpointsPanel.SetActive(false);
    }

    #endregion

    #region ButtonSetup
    private void SetupCheckpoints(Vector3[] checkpointsPos)
    {
        checkpointButtons = new Button[checkpointsPos.Length];
        checpointPositions = checkpointsPos;
        for (int i = 0; i < checkpointsPos.Length; i++)
        {
            checkpointButtons[i] = Instantiate(checkpointButtonPrefab).GetComponent<Button>();
            checkpointButtons[i].gameObject.transform.SetParent(checkpointButtonParent);
            checkpointButtons[i].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "" + i;
            checkpointButtons[i].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().gameObject.name = "" + i;
            checkpointButtons[i].onClick.AddListener(() =>
            {
                UpdateTexts(baseTravelCostCheckpoint * (checkpointIndex + 1));
            });

        }
    }
    private void GetSceneInArray()
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
            levelName[i] = levelName[i].Replace(".unity","");
        }
    }
    private void SetupLevels()
    {

        levelButtons = new Button[levelPaths.Count];

        for (int i = 0; i < levelPaths.Count; i++)
        {
            levelButtons[i] = Instantiate(levelButtonPrefab.gameObject.GetComponent<Button>());
            levelButtons[i].transform.SetParent(levelButtonParent);
            levelButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelName[i];
            levelButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().gameObject.name = "" + i;
            levelButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (i + 1);
            levelButtons[i].onClick.AddListener(() =>
            {
                UpdateTexts(baseTravelCostLevel * (levelIndex + 1));
            });

        }
    }
    #endregion

    private void UpdateTexts(float money)
    {
        moneyRequired.text = "$" + money;
        levelTitle.text = "Level " + SceneManager.GetActiveScene().name;
    }

    public void Travel()
    {
        if (checkpointsPanel.activeSelf)
        {
            double moneyReduced = GameManager.Instance.GetGameData().totalMoney - baseTravelCostCheckpoint * (checkpointIndex + 1);
            if (moneyReduced < 0)
            {
                Debug.Log("Not enought money");
            }
            else
            {
                GameManager.Instance.GetGameData().totalMoney = moneyReduced;
                onPlayerTravel?.Invoke(checpointPositions[checkpointIndex], 0);
                CloseTaxi();
            }
        }
        else
        {
            double moneyReduced = GameManager.Instance.GetGameData().totalMoney - baseTravelCostLevel * (levelIndex + 1);
            if (moneyReduced < 0)
            {
                Debug.Log("Not enought money");
            }
            else
            {
                GameManager.Instance.GetGameData().totalMoney = moneyReduced;
                GameManager.Instance.NextLevel(SceneUtility.GetBuildIndexByScenePath(levelPaths[levelIndex]));
            }


            //Debug.Log("Selected Scene Index: " + SceneUtility.GetBuildIndexByScenePath(levelNames[levelIndex]));
            //Assets/Scenes/PlatformScenes/PlatformScene.unity
            //Assets/Scenes/PlatformScenes\PlatformScene.unity
            //Debug.Log("Selected Scene: " + levelNames[levelIndex]);

            //Debug.Log("Selected Scene: " + levelNames[levelIndex] + " Index: " + levelIndex);

            //GameManager.Instance.NextLevel(selectedScene.buildIndex);
        }


    }

}

/*
             checkpointButtons.Add(index, Instantiate(checkpointButtonPrefab).GetComponent<Button>());



  index = i;
            checkpointButtons.Add(index, Instantiate(checkpointButtonPrefab).GetComponent<Button>());
            checkpointButtons[i].gameObject.transform.SetParent(checkpointButtonParent);
            checkpointButtons[i].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "" + i;
            checkpointButtons[i].onClick.AddListener(() =>
            {
                //Debug.Log("Index: " + index);
                SelectCheckpoint();
            });
 
        
            if (SceneManager.GetSceneByName(levelNames[levelIndex]) != null)
            {
                Debug.Log("Selected Scene: " + levelNames[levelIndex] + " Index: " + levelIndex);
                Debug.Log("Scene is: " + SceneManager.GetSceneByName(levelNames[levelIndex]).name);
                Debug.Log("Scene index:  " + SceneManager.GetSceneByName(levelNames[levelIndex]).buildIndex);

            }
 for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".meta"))
            {
                string name = files[i].Replace("Assets/Scenes/PlatformScenes\\", "");
                name = name.Replace(".unity", ""); ;
                levelNames.Add(name);
            }
        }
            foreach (var item in allScenes)
            {
                //Debug.Log("Scenes: " + item.name);
            }


            for (int i = 0; i < allScenes.Length; i++)
            {
                if (allScenes[i].name == levelNames[levelIndex])
                {
                    //selectedScene = allScenes[i];
                }
            }
            Scene[] allScenes = new Scene[levelNames[levelIndex].Length];
            // Scene selectedScene = SceneUtility.GetBuildIndexByScenePath("");
            Debug.Log("Path: " + levelNames[levelIndex]);
            for (int i = 0; i < levelNames[levelIndex].Length; i++)
            {
                allScenes[i] = ;
            }

 */