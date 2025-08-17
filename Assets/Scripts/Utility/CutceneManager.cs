using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSceneManager : MonoBehaviour
{
    [Tooltip("Reference to the cutscene player.")]
    public TextTransition cutscenePlayer;

    [Tooltip("Scene name to load after the cutscene. Leave empty to use build index instead.")]
    public string nextSceneName;

    [Tooltip("Build index to load if Scene Name is not used.")]
    public int nextSceneIndex = -1;

    [Tooltip("Delay (seconds) before changing scenes after final line is confirmed.")]
    public float transitionDelay = 0.5f;

    private void OnEnable()
    {
        if (cutscenePlayer != null)
        {
            cutscenePlayer.OnCutsceneComplete += HandleCutsceneFinished;
        }
    }

    private void OnDisable()
    {
        if (cutscenePlayer != null)
        {
            cutscenePlayer.OnCutsceneComplete -= HandleCutsceneFinished;
        }
    }

    private void HandleCutsceneFinished()
    {
        StartCoroutine(WaitThenChangeScene());
    }

    private System.Collections.IEnumerator WaitThenChangeScene()
    {
        // Wait for player input again to confirm cutscene end
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        yield return new WaitForSeconds(transitionDelay);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (nextSceneIndex >= 0)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No valid scene specified for transition.");
        }
    }
}
