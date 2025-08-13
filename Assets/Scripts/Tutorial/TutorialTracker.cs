using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialTracker : MonoBehaviour
{
    // string = ID, bool = true if first time, false if revisit.
    public static event Action<string, bool> OnInteractableTriggered;

    [SerializeField] private GameObject tutorialDialogueBox;
    [SerializeField] private TextMeshProUGUI tutorialDialogueText;  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShowTutorialText(collision.tag, collision.gameObject.name);
    }

    private void ShowTutorialText(string tag, string objectName)
    {
        if (!Enum.TryParse(tag, out TutorialObjects _))
            return;

        var data = GameManager.Instance.GetGameData();

        // Scene-qualified ID avoids collisions across levels
        string id = $"{SceneManager.GetActiveScene().name}:{tag}:{objectName}";

        bool firstTime = !data.visitedInteractables.Contains(id);
        if (firstTime)
        {
            data.visitedInteractables.Add(id);

            if (tutorialDialogueBox != null && tutorialDialogueText != null)
            {
                tutorialDialogueBox.SetActive(true);
                tutorialDialogueText.text = $"This is a {tag}";
                Debug.Log($"Toggled Dialogue box");
            }
            else
            {
                Debug.Log($"No Box or Text detected via Tracker");
            }
            
            Debug.Log($"First time entered {objectName} via Tracker");
        }

        else
        {
            Debug.Log($"Has visited {objectName} via Tracker");
        }
    }
}


