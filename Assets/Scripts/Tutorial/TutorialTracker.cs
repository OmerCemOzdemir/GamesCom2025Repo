using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialTracker : MonoBehaviour
{
    // string = ID, bool = true if first time, false if revisit.
    public static event Action<string, bool> OnInteractableTriggered;

    public GameObject tutorialDialogueBox;
    public TextMeshProUGUI tutorialDialogueText;  

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
            data.visitedInteractables.Add(id);

        // Use player’s debug event via proxy
        if (firstTime)
            Debug.Log($"First time entered {objectName} in Tracker");
         else
            Debug.Log($"Has visited {objectName} in Tracker");
    }
}


