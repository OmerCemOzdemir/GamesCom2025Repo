using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private GameData gameData;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //_instance = FindFirstObjectByType<GameManager>();
                GameObject newObj = new GameObject();
                newObj.name = "Game Manager";
                _instance = newObj.AddComponent<GameManager>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }

    }

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (File.Exists(Application.dataPath + "/SaveData.json"))
        {
            LoadGame();
        }
        else
        {
            gameData = new GameData();
            SaveGame();
        }

    }
    //Go next level by giving level build index
    public void NextLevel(int level)
    {
        SceneManager.LoadSceneAsync(level);
    }

    //Save Game by calling this function
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(Application.dataPath + "/SaveData.json", json);
        Debug.Log("Game Saved");
    }

    //Load Game by calling this function
    public void LoadGame()
    {
        string json = File.ReadAllText(Application.dataPath + "/SaveData.json");
        gameData = JsonUtility.FromJson<GameData>(json);
    }
    //For setting the game data in Game Manager from a instance of a local game data object.
    public void SetGameData(GameData newGameData)
    {
        gameData = newGameData;
    }

    //For getting the initilized game data in Game Manager
    public GameData GetGameData()
    {
        return gameData;
    }

    //For reseting the game data in JSON and runtime game data.
    public void ResetGameData()
    {
        gameData = new GameData();
        SaveGame();
        LoadGame();
    }

}
