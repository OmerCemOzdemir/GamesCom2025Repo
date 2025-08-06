using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TextTransition : MonoBehaviour
{
    [System.Serializable]
    public class DialogueSettings
    {
        public TransitionType transition = TransitionType.Typewriter;
        public float duration = 1.5f; // Typewriter letter delay or fade duration
    }

    // Choose which transition per line
    public DialogueSettings[] lineSettings;

    // Auto start on play
    public bool autoStart = true;

    private TextMeshProUGUI textComponent;
    private string[] dialogueLines;
    private Coroutine routine;
    public System.Action OnCutsceneComplete;

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
                    yield return StartCoroutine(ShowTypewriter(line, settings.duration));
                    break;
                case TransitionType.Fade:
                    yield return StartCoroutine(ShowFade(line, settings.duration));
                    break;
                default:
                    textComponent.text = line;
                    break;
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));

            textComponent.text = "";
            if (i == Mathf.Min(dialogueLines.Length, lineSettings.Length) - 1)
            {
                OnCutsceneComplete?.Invoke();
            }
        }

        textComponent.text = "";
    }

    private IEnumerator ShowTypewriter(string line, float letterDelay)
    {
        textComponent.text = "";
        int charCount = line.Length;
        float interval = (charCount > 0) ? letterDelay / charCount : 0.01f;

        for (int i = 0; i < charCount; i++)
        {
            textComponent.text += line[i];
            yield return new WaitForSeconds(interval);
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
    
    public enum TransitionType
    {
        None,
        Typewriter,
        Fade
    }
}
