using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TextCutscenePlayer : MonoBehaviour
{
    public enum TransitionType
    {
        None,
        Typewriter,
        Fade
    }

    [System.Serializable]
    public class DialogueSettings
    {
        public TransitionType transition = TransitionType.Typewriter;
        public float speed = 0.03f; // Typewriter letter delay or fade duration
    }

    // Choose which transition per line
    public DialogueSettings[] lineSettings;

    // Auto start on play
    public bool autoStart = true;
    
    private TextMeshProUGUI textComponent;
    private string[] dialogueLines;
    private Coroutine routine;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        dialogueLines = textComponent.text.Split('\n');
        textComponent.text = "";
    }

    private void Start()
    {
        if (autoStart)
            PlayCutscene();
    }

    public void PlayCutscene()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(PlayLines());
    }

    private IEnumerator PlayLines()
    {
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            string line = dialogueLines[i];
            DialogueSettings settings = (i < lineSettings.Length) ? lineSettings[i] : new DialogueSettings();

            switch (settings.transition)
            {
                case TransitionType.Typewriter:
                    yield return StartCoroutine(ShowTypewriter(line, settings.speed));
                    break;
                case TransitionType.Fade:
                    yield return StartCoroutine(ShowFade(line, settings.speed));
                    break;
                default:
                    textComponent.text = line;
                    break;
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        textComponent.text = "";
    }

    private IEnumerator ShowTypewriter(string line, float letterDelay)
    {
        textComponent.text = "";
        foreach (char c in line)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    private IEnumerator ShowFade(string line, float fadeDuration)
    {
        textComponent.alpha = 0;
        textComponent.text = line;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            textComponent.alpha = timer / fadeDuration;
            yield return null;
        }

        textComponent.alpha = 1;
    }
}
