using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TaxiUI : MonoBehaviour
{
    //public static event Action<Vector3, float> onPlayerTravel;

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
    [SerializeField] private GameObject travelButton;
    [SerializeField] private GameObject firstLevelButton;

    [Space(10)]
    [Header("Stats: ")]
    [SerializeField] private float baseTravelCostLevel = 100;
    [SerializeField] private float baseTravelCostCheckpoint = 10;

    //Checkpoint Var:
    private Button[] checkpointButtons;
    private Vector3[][] checpointPositions;
    private int checkpointIndex = 0;
    public int CheckpointIndex { get => checkpointIndex; set => checkpointIndex = value; }

    //Level Var:
    private Button[] levelButtons;
    private int levelIndex = 0;
    public int LevelIndex { get => levelIndex; set => levelIndex = value; }

    //Dictionary<int, Button> checkpointButtons = new Dictionary<int, Button> { };

    private void OnEnable()
    {
        PlayerControler.onPlayerGetInTaxi += OpenTaxi;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerGetInTaxi -= OpenTaxi;
    }


    private void Awake()
    {
        //GetSceneInArray();
        //printLevelData();
    }

    private void Start()
    {
        SetUpData();

    }

    private void SetUpData()
    {
        Debug.Log("Run SetUpData");
        LevelSaveData[] levelData = GameManager.Instance.GetGameData().levelData;
        //printLevelData(levelData);
        Vector3[][] checkpointsPos = new Vector3[levelData.Length][];
        //Force unlock the first level
        levelData[0].unlock = true;

        for (int i = 0; i < levelData.Length; i++)
        {
            if (levelData[i].unlock)
            {
                if (checkpointsPos[i] != null)
                {
                    checkpointsPos[i] = new Vector3[levelData[i].checkpointX.Length];
                    for (int j = 0; j < levelData[i].checkpointX.Length; j++)
                    {
                        checkpointsPos[i][j] = new Vector3(levelData[i].checkpointX[j], levelData[i].checkpointY[j], levelData[i].checkpointZ[j]);
                    }
                }
                else
                {

                }

            }
        }
        checpointPositions = checkpointsPos;
        SetupLevels(levelData);


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
    private void SetupCheckpoints(Vector3[][] checkpointsPos, LevelSaveData[] levelData)
    {
        checkpointButtons = new Button[checkpointsPos[levelIndex].Length];
        for (int i = 0; i < checkpointButtons.Length; i++)
        {
            checkpointButtons[i] = Instantiate(checkpointButtonPrefab).GetComponent<Button>();
            checkpointButtons[i].gameObject.transform.SetParent(checkpointButtonParent);
            checkpointButtons[i].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "" + i;
            checkpointButtons[i].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().gameObject.name = "" + i;
            checkpointButtons[i].onClick.AddListener(() =>
            {
                UpdateTexts(baseTravelCostCheckpoint * (checkpointIndex + 1));
            });

            if (levelData[levelIndex].unlockCheckpoint[i])
            {
                checkpointButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                checkpointButtons[i].GetComponent<Button>().enabled = true;

            }
            else
            {
                checkpointButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                checkpointButtons[i].GetComponent<Button>().enabled = false;

            }

        }
    }

    private void SetupLevels(LevelSaveData[] levelData)
    {
        printLevelData(levelData);
        levelButtons = new Button[levelData.Length];

        for (int i = 0; i < levelData.Length; i++)
        {
            levelButtons[i] = Instantiate(levelButtonPrefab.gameObject.GetComponent<Button>());
            levelButtons[i].transform.SetParent(levelButtonParent);
            levelButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelData[i].levelName;
            levelButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().gameObject.name = "" + i;
            levelButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (i + 1);

            levelButtons[i].onClick.AddListener(() =>
            {
                UpdateTexts(baseTravelCostLevel * (levelIndex + 1));
            });
            // Debug.Log(levelData[i].levelName + " " + GameManager.Instance.GetGameData().levelData[i].unlock);
            //Debug.Log(levelData[i].levelName + " " + levelData[i].unlock);


            if (levelData[i].unlock)
            {
                levelButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                levelButtons[i].GetComponent<Button>().enabled = true;
                //Debug.Log(levelButtons[i].name + " true");
            }
            else
            {
                //Debug.Log(levelButtons[i].name + " false");
                levelButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                levelButtons[i].GetComponent<Button>().enabled = false;

            }
        }


        levelButtons[levelData.Length - 1].transform.GetChild(2).gameObject.SetActive(false);
        float newWidth = (
            (levelButtons[0].GetComponent<RectTransform>().rect.width
             + levelButtons[0].transform.GetChild(2).GetComponent<RectTransform>().rect.width)
             * levelData.Length) - 450;
        float newHeight = levelButtonParent.GetComponent<RectTransform>().rect.height;

        levelButtonParent.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, newHeight);


    }

    public void FirstLevel()
    {
        travelButton.SetActive(false);
        firstLevelButton.SetActive(true);
    }


    public void TravelFirstLevel()
    {
        LevelSaveData[] levelData = GameManager.Instance.GetGameData().levelData;
        GameManager.Instance.NextLevel(levelData[0].levelIndex);

    }

    public void UpdateCheckpoints()
    {
        if (checkpointButtonParent.childCount == 0)
        {
            SetupCheckpoints(checpointPositions, GameManager.Instance.GetGameData().levelData);
        }
        else
        {
            for (int i = 0; i < checkpointButtonParent.childCount; i++)
            {
                Destroy(checkpointButtonParent.GetChild(i).gameObject);
            }
            SetupCheckpoints(checpointPositions, GameManager.Instance.GetGameData().levelData);

        }
    }

    #endregion

    private void UpdateTexts(float money)
    {
        moneyRequired.text = "$" + money;
        //levelTitle.text = "LevelButton " + SceneManager.GetActiveScene().name;
    }

    private void printLevelData(LevelSaveData[] levelData)
    {
        //  = GameManager.Instance.GetGameData().levelData;

        for (int i = 0; i < levelData.Length; i++)
        {
            Debug.Log("" + levelData[i].levelName
                + "levelData[i].unlock: " + levelData[i].unlock
                );
        }

    }

    public void Travel()
    {
        LevelSaveData[] levelData = GameManager.Instance.GetGameData().levelData;
        if (checkpointsPanel.activeSelf)
        {
            double moneyReduced = GameManager.Instance.GetGameData().totalMoney - baseTravelCostLevel * (levelIndex + 1);
            if (moneyReduced < 0)
            {
                Debug.Log("Not enought money");
            }
            else
            {
                GameManager.Instance.GetGameData().totalMoney = moneyReduced;
                GameManager.Instance.GetGameData().checkpointX = levelData[levelIndex].checkpointX[checkpointIndex];
                GameManager.Instance.GetGameData().checkpointY = levelData[levelIndex].checkpointY[checkpointIndex];
                GameManager.Instance.GetGameData().checkpointZ = levelData[levelIndex].checkpointZ[checkpointIndex];
                GameManager.Instance.GetGameData().checkpointEnable = true;

                Debug.Log("Level Index: " + levelIndex + " Checkpoint Index: " + checkpointIndex);
                Debug.Log("New Pos: " + levelData[levelIndex].checkpointX[checkpointIndex]
                 + " " + levelData[levelIndex].checkpointY[checkpointIndex]
                 + " " + levelData[levelIndex].checkpointZ[checkpointIndex]);
                GameManager.Instance.NextLevel(levelData[levelIndex].levelIndex);
                //GameManager.Instance.NextLevel(SceneUtility.GetBuildIndexByScenePath(levelPaths[levelIndex]));
            }

        }



    }

}

/*            //Debug.Log("Selected Scene Index: " + SceneUtility.GetBuildIndexByScenePath(levelNames[levelIndex]));
            //Assets/Scenes/PlatformScenes/PlatformScene.unity
            //Assets/Scenes/PlatformScenes\PlatformScene.unity
            //Debug.Log("Selected Scene: " + levelNames[levelIndex]);

            //Debug.Log("Selected Scene: " + levelNames[levelIndex] + " Index: " + levelIndex);

            //GameManager.Instance.NextLevel(selectedScene.buildIndex);
 * 
 *             double moneyReduced = GameManager.Instance.GetGameData().totalMoney - baseTravelCostCheckpoint * (checkpointIndex + 1);
            if (moneyReduced < 0)
            {
                Debug.Log("Not enought money");
            }
            else
            {
                GameManager.Instance.GetGameData().totalMoney = moneyReduced;
                //onPlayerTravel?.Invoke(checpointPositions[checkpointIndex], 0);
                CloseTaxi();
            }
 * 
 * 
 * 
    private void UpdateLevels(LevelSaveData[] levelData)
    {

        for (int i = 0; i < levelData.Length; i++)
        {
            if (levelData[i].unlock)
            {
                levelButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                levelButtons[i].GetComponent<Button>().enabled = true;

            }
            else
            {
                levelButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                levelButtons[i].GetComponent<Button>().enabled = false;

            }
        }

    }

 * 
 *     private void GetSceneInArray()
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
    }
   
 * 
 * 
 * 
 * 
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