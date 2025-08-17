using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickerTutorialManager : MonoBehaviour
{
    private enum TutorialStep
    {
        WelcomeToTutorial,
        ClickerButton1,
        MoneyTo100,
        MoneyReached100,
        UpgradeButton,
        MoneyTo1000,
        MoneyReached1000,
        EnterMainHub,
        Complete
    }

    private TutorialStep currentStep;
    private ClickerManager clickerManager;

    [Header("Popup GameObjects")]
    [SerializeField] private GameObject welcomeToTutorial;
    [SerializeField] private GameObject clickerButton1;
    [SerializeField] private GameObject moneyTo100;
    [SerializeField] private GameObject moneyReached100;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject moneyTo1000;
    [SerializeField] private GameObject moneyReached1000;
    [SerializeField] private GameObject enterMainHub;
    [Space(10)]

    [Header("Clicker Interactive Elements")]
    [SerializeField] private GameObject clickerButtonObj;
    private CheckMousePos clickerMousePosScript;
    private animationManager clickerButtonAnimation;
    [SerializeField] private Button upgradeButtonObj;
    [SerializeField] private GameObject exitButton;
    [Space(10)]

    [Header("Tutorial Effects")]
    [SerializeField] private ParticleSystem tutorialConfetiEffect;


    // Tutorial Text Listeners

    private GameObject currentPopup;
    private TextTransition currentTextTransition;
    private InputSystem playerInputAction;


    private TextTransition activeTextTransition;
    private bool canProceedText = false;


    private void Awake()
    {
        playerInputAction = new InputSystem();

    }

    private void OnEnable()
    {
        playerInputAction.PlayerCookie.Enable();
        playerInputAction.PlayerCookie.IterateTutorial.performed += IterateTutorial;
    }

    private void OnDisable()
    {
        playerInputAction.PlayerCookie.Disable();
        playerInputAction.PlayerCookie.IterateTutorial.performed -= IterateTutorial;

    }

    private void Start()
    {
        clickerManager = FindObjectOfType<ClickerManager>();


        DisableAllButtons();
        upgradeButton.gameObject.SetActive(false);

        clickerMousePosScript = clickerButtonObj.GetComponent<CheckMousePos>();
        clickerButtonAnimation = clickerButtonObj.GetComponent<animationManager>();

        GoToStep(TutorialStep.WelcomeToTutorial);
        //welcomeTextTransition.OnCutsceneComplete += OnWelcomeFinished;
    }

    private void Update()
    {
        double money = GameManager.Instance.GetGameData().totalMoney;

        if (Input.GetKeyDown(KeyCode.Z) && CanAdvanceStep())
        {
            GoToNextStep();
        }

        if (currentStep == TutorialStep.ClickerButton1 && money >= 1)
            GoToStep(TutorialStep.MoneyTo100);

        if (currentStep == TutorialStep.MoneyTo100 && money >= 100)
        {
            tutorialConfetiEffect.Play();
            GoToStep(TutorialStep.MoneyReached100);
        }


        if (currentStep == TutorialStep.MoneyTo1000 && money >= 200)
        {
            tutorialConfetiEffect.Play();
            GoToStep(TutorialStep.MoneyReached1000);
        }
    }

    //Get user input to iterate to the next step of the Tutorial, Used for GoToStep Func
    private void IterateTutorial(InputAction.CallbackContext context)
    {

    }

    private void GoToStep(TutorialStep step)
    {
        DisableAllPopups();
        UnsubscribeFromPreviousText();

        currentStep = step;
        currentPopup = GetPopupForStep(step);

        if (currentPopup != null)
        {
            currentPopup.SetActive(true);
            currentTextTransition = currentPopup.GetComponentInChildren<TextTransition>();

            if (currentTextTransition != null)
            {
                currentTextTransition.OnCutsceneComplete += OnTextComplete;
            }

            // disable clicker button when tutorial popup is active
            SetClickerButtonInteractivity(false);
        }

        // Handle unlockable elements directly
        if (step == TutorialStep.ClickerButton1)
        {
            clickerButtonObj.SetActive(true);
        }
        else if (step == TutorialStep.UpgradeButton)
        {
            upgradeButtonObj.gameObject.SetActive(true);
            upgradeButtonObj.interactable = false;
            upgradeButtonObj.onClick.AddListener(OnUpgradeTutorialClicked);
        }
        else if (step == TutorialStep.EnterMainHub)
        {
            exitButton.SetActive(true);
        }
    }

    private void OnTextComplete()
    {
        UnsubscribeFromPreviousText();

        if (currentPopup != null)
            currentPopup.SetActive(false);

        if (currentStep == TutorialStep.UpgradeButton)
        {
            upgradeButtonObj.gameObject.SetActive(true);
            upgradeButtonObj.interactable = true;
            upgradeButtonObj.onClick.RemoveListener(OnUpgradeTutorialClicked); // reset from previous popup
            upgradeButtonObj.onClick.AddListener(OnUpgradeTutorialClicked);
            return; // wait for button click
        }

        switch (currentStep)
        {
            case TutorialStep.ClickerButton1:
            case TutorialStep.MoneyTo100:
                SetClickerButtonInteractivity(true);
                break;
            case TutorialStep.MoneyReached100:
            case TutorialStep.MoneyTo1000:
                SetClickerButtonInteractivity(true);
                break;
            default:
                SetClickerButtonInteractivity(false);
                break;
        }

        // Only auto-proceed if this step has no prerequisite
        if (!StepHasPrerequisite(currentStep))
            GoToNextStep();
    }

    private void UnsubscribeFromPreviousText()
    {
        if (currentTextTransition != null)
            currentTextTransition.OnCutsceneComplete -= OnTextComplete;

        currentTextTransition = null;
    }

    private bool CanAdvanceStep()
    {
        switch (currentStep)
        {
            case TutorialStep.WelcomeToTutorial:
            case TutorialStep.ClickerButton1:
                return false;
            case TutorialStep.MoneyTo100:
                return false;
            case TutorialStep.MoneyReached100:
            case TutorialStep.UpgradeButton:
                return false;
            case TutorialStep.MoneyTo1000:
                return false;
            case TutorialStep.MoneyReached1000:
            case TutorialStep.EnterMainHub:
                return canProceedText;

            default:
                return false;
        }
    }
    private bool StepHasPrerequisite(TutorialStep step)
    {
        return step switch
        {
            TutorialStep.ClickerButton1 => true,  // wait for $1
            TutorialStep.MoneyTo100 => true,      // wait for $100
            TutorialStep.UpgradeButton => true,   // wait for upgrade
            TutorialStep.MoneyTo1000 => true,     // wait for $1000
            _ => false
        };
    }

    private void DisableAllPopups()
    {
        welcomeToTutorial.SetActive(false);
        clickerButton1.SetActive(false);
        moneyTo100.SetActive(false);
        moneyReached100.SetActive(false);
        upgradeButton.SetActive(false);
        moneyTo1000.SetActive(false);
        moneyReached1000.SetActive(false);
        enterMainHub.SetActive(false);
    }

    private void DisableAllButtons()
    {
        clickerButtonObj.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        exitButton.SetActive(false);
    }

    private void SetClickerButtonInteractivity(bool enabled)
    {
        if (clickerMousePosScript != null)
            clickerMousePosScript.enabled = enabled;

        if (clickerButtonAnimation != null && clickerButtonAnimation.TryGetComponent(out Animator animator))
            animator.enabled = enabled;

        if (clickerManager != null)
            clickerManager.SetClickingEnabled(enabled);

        if (enabled && clickerMousePosScript != null && currentPopup == null)
        {
            clickerMousePosScript.ForceHover(true);
        }
        else if (!enabled && clickerMousePosScript != null)
        {
            clickerMousePosScript.ForceHover(false);
        }
    }

    // === Triggers ===
    public void OnUpgradeBought()
    {
        if (currentStep == TutorialStep.MoneyTo1000)
            return;

        if (currentStep == TutorialStep.UpgradeButton)
        {
            GoToStep(TutorialStep.MoneyTo1000);
        }
    }
    public void OnUpgradeTutorialClicked()
    {
        var data = GameManager.Instance.GetGameData();
        var clickerManager = FindObjectOfType<ClickerManager>();

        if (clickerManager != null)
        {
            clickerManager.ImplementTutorialActiveIncrement(1f);
            // data.clickerItems[0].tier = 1; // if using scriptable index to upgrade
            clickerManager.SetUpData();
            clickerManager.PrintFields(); // debug
        }

        upgradeButtonObj.onClick.RemoveListener(OnUpgradeTutorialClicked);
        upgradeButtonObj.interactable = false;

        GoToStep(TutorialStep.MoneyTo1000);
    }

    public void OnExitClicked()
    {
        if (currentStep == TutorialStep.EnterMainHub)
            GoToStep(TutorialStep.Complete);
    }

    private void GoToNextStep()
    {
        DisableAllPopups();
        canProceedText = false;

        switch (currentStep)
        {
            case TutorialStep.WelcomeToTutorial:
                GoToStep(TutorialStep.ClickerButton1);
                break;

            case TutorialStep.ClickerButton1:
                clickerButtonObj.SetActive(true);
                GoToStep(TutorialStep.MoneyTo100);
                break;

            case TutorialStep.MoneyReached100:
                GoToStep(TutorialStep.UpgradeButton);
                break;

            case TutorialStep.UpgradeButton:
                //upgradeButtonObj.gameObject.SetActive(true);
                GoToStep(TutorialStep.MoneyTo1000);
                break;

            case TutorialStep.MoneyReached1000:
                GoToStep(TutorialStep.EnterMainHub);
                break;

            case TutorialStep.EnterMainHub:
                exitButton.SetActive(true);
                GoToStep(TutorialStep.Complete);
                if (clickerManager != null)
                {
                    clickerManager.SetClickingEnabled(true);
                }
                break;
        }
    }

    private GameObject GetPopupForStep(TutorialStep step)
    {
        return step switch
        {
            TutorialStep.WelcomeToTutorial => welcomeToTutorial,
            TutorialStep.ClickerButton1 => clickerButton1,
            TutorialStep.MoneyTo100 => moneyTo100,
            TutorialStep.MoneyReached100 => moneyReached100,
            TutorialStep.UpgradeButton => upgradeButton,
            TutorialStep.MoneyTo1000 => moneyTo1000,
            TutorialStep.MoneyReached1000 => moneyReached1000,
            TutorialStep.EnterMainHub => enterMainHub,
            _ => null,
        };
    }
}


