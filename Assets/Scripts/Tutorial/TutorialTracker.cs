using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        tutorialDialogueBox.SetActive(false);
        tutorialDialogueText.text = $"";
        Debug.Log($"Exit interactable");
    }


    private void ShowTutorialText(string tag, string objectName)
    {
        if (!Enum.TryParse(tag, out TutorialObjects tutorialObj))
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

                string tutorialText;
                switch (tutorialObj)
                {
                    case TutorialObjects.Shop:
                        tutorialText = "This is a Shop.\nYou can buy upgrades here.";
                        break;
                    case TutorialObjects.Sign:
                        tutorialText = "This is a Sign.\nIt can bring me back to the office.";
                        break;
                    case TutorialObjects.Bus:
                        tutorialText = "This is a Bus.\nIt will take me to a checkpoint.";
                        break;
                    default:
                        tutorialText = $"This is a {tag}.\n I should have more info about this.";
                        break;
                }

                var transition = tutorialDialogueText.GetComponent<TextTransition>();
                if (transition != null)
                {
                    transition.ResetTextAndPlay(tutorialText);
                }
                //Debug.Log($"Toggled Dialogue box with tutorial for {tutorialObj}");
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

public enum TutorialObjects
{
    Shop,
    Sign,
    Bus
}


