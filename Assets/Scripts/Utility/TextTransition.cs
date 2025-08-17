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

    [Header("Progress Dialogue")]
    [SerializeField] private bool requireInput = true; // If true, require input to continue. Otherwise, autostart.
    [SerializeField] private KeyCode advanceKey = KeyCode.Z;

    [Header("Hide Dialogue Elements")] // Optional elements; does not need to be all
    [SerializeField] private HideMode hideMode = HideMode.DisableRendererOnParent;
    [SerializeField] private GameObject specificTargetToHide; // disables specific GameObject; use if all elements need to be disabled
    [SerializeField] private Graphic uiGraphicToHide; // target only a Raw image or Sprite Renderer


    // Choose which transition per line
    public DialogueSettings lineSettings = new DialogueSettings();

    // Auto start on play
    public bool autoStart = true;
    private TextMeshProUGUI textComponent;
    private string[] dialogueLines;
    private Coroutine routine;
    public System.Action OnCutsceneComplete;
    private PlayerControler playerController;
    private PlatformerManager platformerManager;
    private string originalMoneyText = "";
    private string originalInteractText = "";

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        dialogueLines = textComponent.text.Split(new[] { '\n' }, System.StringSplitOptions.None);
        textComponent.text = "";
        textComponent.alpha = 1f;

        playerController = FindFirstObjectByType<PlayerControler>();
        platformerManager = FindFirstObjectByType<PlatformerManager>();
    }

    private void Start()
    {
        if (autoStart && dialogueLines != null && dialogueLines.Length > 0 && !string.IsNullOrEmpty(string.Join("", dialogueLines)))
            PlayCutscene();
    }


    public void PlayCutscene()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(PlayLines());
    }

    private IEnumerator PlayLines()
    {
        if (playerController != null)
        {
            //Debug.Log("Dialogue started. No movement allowed");
            playerController.DisableInput();
        }

        if (platformerManager != null)
        {
            if (platformerManager.requiredMoneyText != null)
            {
                var tmp = platformerManager.requiredMoneyText.GetComponent<TextMeshProUGUI>();
                if (tmp != null) originalMoneyText = tmp.text ?? "";
            }
            platformerManager.DisableInteractText();
        }

        for (int i = 0; i < dialogueLines.Length; i++)
        {
            string line = dialogueLines[i];

            switch (lineSettings.transition)
            {
                case TransitionType.Typewriter:
                    yield return StartCoroutine(ShowTypewriter(line, lineSettings.duration));
                    break;
                case TransitionType.Fade:
                    yield return StartCoroutine(ShowFade(line, lineSettings.duration));
                    break;
                default:
                    textComponent.text = line;
                    break;
            }

            if (requireInput)
            {
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(advanceKey));
            }

            textComponent.text = "";
            textComponent.alpha = 1f; // reset visibility if fade was used

        }

        if (playerController != null)
        {
            playerController.EnableInput();
            //Debug.Log("Dialogue ended. Movement enabled");
        }

        if (platformerManager != null)
        {
            platformerManager.EnableInteractText(originalMoneyText);
            Canvas.ForceUpdateCanvases();
        }

        OnCutsceneComplete?.Invoke();
        ApplyHideBehavior();

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

    private void ApplyHideBehavior()
    {
        switch (hideMode)
        {
            case HideMode.None:
                return;

            case HideMode.DisableThisGameObject:
                gameObject.SetActive(false);
                return;

            case HideMode.DisableParentGameObject:
                if (transform.parent != null) transform.parent.gameObject.SetActive(false);
                return;

            case HideMode.SpecificGameObject:
                if (specificTargetToHide != null) specificTargetToHide.SetActive(false);
                return;

            case HideMode.DisableRendererOnParent:
                // Prioritise direct reference
                if (uiGraphicToHide != null)
                {
                    uiGraphicToHide.enabled = false; return;
                }

                // Find component on parent if not referenced
                var parentGraphic = GetComponentInParent<Graphic>();
                if (parentGraphic != null)
                {
                    parentGraphic.enabled = false;
                    return;
                }

                var parentRenderer = GetComponentInParent<Renderer>();
                if (parentRenderer != null)
                {
                    parentRenderer.enabled = false;
                    return;
                }

                // If no element, disable parent
                if (transform.parent != null)
                {
                    transform.parent.gameObject.SetActive(false);
                }
                return;
        }
    }

    public void ResetTextAndPlay(string fullText)
    {
        // Stop any running playback
        if (routine != null)
            StopCoroutine(routine);

        // Reset for new lines (requires new text to be set externally)
        textComponent.text = fullText ?? "";
        dialogueLines = textComponent.text.Split(new[] { '\n' }, System.StringSplitOptions.None);

        // Reset visual state and start
        textComponent.text = "";
        textComponent.alpha = 1f;
        routine = StartCoroutine(PlayLines());
    }

    public enum TransitionType
    {
        None,
        Typewriter,
        Fade
    }

    public enum HideMode
    {
        None,
        DisableThisGameObject,
        DisableParentGameObject,
        SpecificGameObject,
        DisableRendererOnParent
    }
}
