using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Sprite checkpointOnSprite;
    private bool toggle = true;

    private void Awake()
    {
        CheckForToggle();
    }

    private void EnableCheckpoint()
    {
        LevelSaveData[] levelSaveData = GameManager.Instance.GetGameData().levelData;
        int index = 0;
        for (int i = 0; i < levelSaveData.Length; i++)
        {
            if (levelSaveData[i].levelName == SceneManager.GetActiveScene().name)
            {

                for (int j = 0; j < levelSaveData[i].checkpointX.Length; j++)
                {
                    if (levelSaveData[i].checkpointX[j] == transform.position.x)
                    {
                        index = j;
                        Debug.Log("index: " + index);
                    }
                }

                for (int j = 0; j < levelSaveData[i].checkpointY.Length; j++)
                {
                    if (levelSaveData[i].checkpointY[j] == transform.position.y)
                    {
                        index = j;
                        Debug.Log("index: " + index);
                    }
                }

                levelSaveData[i].unlockCheckpoint[index] = true;
                Debug.Log("Checkpoint pos: "
                    + " " + levelSaveData[i].checkpointX[index] + " "
                    + " " + levelSaveData[i].checkpointY[index] + " "
                    + " " + levelSaveData[i].checkpointZ[index] + " "
                    + "unlock: " + levelSaveData[i].unlockCheckpoint[index] + " "
                    );

            }
        }
        GameManager.Instance.GetGameData().levelData = levelSaveData;
        GetComponent<SpriteRenderer>().sprite = checkpointOnSprite;
    }

    private void CheckForToggle()
    {
        LevelSaveData[] levelSaveData = GameManager.Instance.GetGameData().levelData;
        int index = 0;
        for (int i = 0; i < levelSaveData.Length; i++)
        {
            if (levelSaveData[i].levelName == SceneManager.GetActiveScene().name)
            {

                for (int j = 0; j < levelSaveData[i].checkpointX.Length; j++)
                {
                    if (levelSaveData[i].checkpointX[j] == transform.position.x)
                    {
                        index = j;
                        Debug.Log("index: " + index);
                    }
                }

                for (int j = 0; j < levelSaveData[i].checkpointY.Length; j++)
                {
                    if (levelSaveData[i].checkpointY[j] == transform.position.y)
                    {
                        index = j;
                        Debug.Log("index: " + index);
                    }
                }

                if (levelSaveData[i].unlockCheckpoint[index])
                {
                    toggle = false;
                    GameManager.Instance.GetGameData().levelData = levelSaveData;
                    GetComponent<SpriteRenderer>().sprite = checkpointOnSprite;
                }

            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (toggle)
            {
                EnableCheckpoint();
                toggle = false;
            }
        }
    }

}

/*
                 for (int j = 0; j < levelSaveData[i].checkpointZ.Length; j++)
                {
                    if (levelSaveData[i].checkpointZ[j] == transform.position.z)
                    {
                        index = j;
                    }
                }
 
 */