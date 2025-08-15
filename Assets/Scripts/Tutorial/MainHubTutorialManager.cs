using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHubTutorial : MonoBehaviour
{
    [SerializeField] private GameObject playerDialogueBox;
    [SerializeField] private TextTransition playerTutorialText;
    [SerializeField] private string firstEnterLine = "I need to go to the bus to start exploring the city";
    [SerializeField] private int mainHubBuildIndex = 2; // ENSURE MainHub is index 2 on the Build settings

    void Start()
    {
        Debug.Log($"Started MainHub tutorial at {SceneManager.GetActiveScene().buildIndex}.");

        // Only usable in MainHub scene
        if (SceneManager.GetActiveScene().buildIndex != mainHubBuildIndex) return;

        var data = GameManager.Instance.GetGameData();
        if (data.PlatformerTutorialPlayed)
        {
            Debug.Log($"Already played MainHubTutorial before.");
            return;
        }

        // Show the player’s dialogue (uses your existing TextTransition)
        if (playerDialogueBox != null)
        {
            playerDialogueBox.gameObject.SetActive(true);

            // If your TextTransition has ResetTextAndPlay (as used in TutorialTracker), use it:
            // playerTutorialText.ResetTextAndPlay(firstEnterLine);

            // If you added Show/Set helpers, use:
            // playerDialogueBox.ShowText(firstEnterLine);

            playerTutorialText.ResetTextAndPlay(firstEnterLine);
        }

        // Mark as shown and persist
        data.PlatformerTutorialPlayed = true;
        GameManager.Instance.SaveGame();
    }
}
