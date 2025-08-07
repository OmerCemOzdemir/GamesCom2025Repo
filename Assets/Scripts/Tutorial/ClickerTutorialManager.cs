using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
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

    [Header("Popup GameObjects")]
    [SerializeField] private GameObject welcomeToTutorial;
    [SerializeField] private GameObject clickerButton1;
    [SerializeField] private GameObject moneyTo100;
    [SerializeField] private GameObject moneyReached100;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject moneyTo1000;
    [SerializeField] private GameObject moneyReached1000;
    [SerializeField] private GameObject enterMainHub;

    [Header("Clicker Interactive Elements")]
    [SerializeField] private GameObject clickerButton;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject exitButton;

    [Header("Tutorial Text Listeners")]
    [SerializeField] private TextTransition welcomeTextTransition;

    private TextTransition activeTextTransition;
    private bool canProceedText = false;

    private void Start()
    {
        GoToStep(TutorialStep.WelcomeToTutorial);
        welcomeTextTransition.OnCutsceneComplete += OnWelcomeFinished;
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
            GoToStep(TutorialStep.MoneyReached100);

        if (currentStep == TutorialStep.MoneyTo1000 && money >= 1000)
            GoToStep(TutorialStep.MoneyReached1000);
    }

    private void GoToStep(TutorialStep step)
    {
        DisableAllPopups();
        DisableAllButtons();
        currentStep = step;

        switch (step)
        {
            case TutorialStep.WelcomeToTutorial:
                welcomeToTutorial.SetActive(true);
                canProceedText = true;
                break;

            case TutorialStep.ClickerButton1:
                clickerButton.SetActive(true);
                clickerButton1.SetActive(true);
                canProceedText = true;
                break;

            case TutorialStep.MoneyTo100:
                moneyTo100.SetActive(true);
                break;

            case TutorialStep.MoneyReached100:
                moneyReached100.SetActive(true);
                canProceedText = true;
                break;

            case TutorialStep.UpgradeButton:
                upgradeButton.SetActive(true);
                canProceedText = true;
                break;

            case TutorialStep.MoneyTo1000:
                moneyTo1000.SetActive(true);
                break;

            case TutorialStep.MoneyReached1000:
                moneyReached1000.SetActive(true);
                canProceedText = true;
                break;

            case TutorialStep.EnterMainHub:
                enterMainHub.SetActive(true);
                canProceedText = true;
                break;

            case TutorialStep.Complete:
                GameManager.Instance.GetGameData().clickerTutorialPlayed = true;
                GameManager.Instance.SaveGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
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
        clickerButton.SetActive(false);
        upgradePanel.SetActive(false);
        exitButton.SetActive(false);
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
                clickerButton.SetActive(true);
                GoToStep(TutorialStep.MoneyTo100);
                break;

            case TutorialStep.MoneyReached100:
                GoToStep(TutorialStep.UpgradeButton);
                break;

            case TutorialStep.UpgradeButton:
                upgradePanel.SetActive(true); // or newUpgradeButton.SetActive(true);
                GoToStep(TutorialStep.MoneyTo1000);
                break;

            case TutorialStep.MoneyReached1000:
                GoToStep(TutorialStep.EnterMainHub);
                break;

            case TutorialStep.EnterMainHub:
                exitButton.SetActive(true);
                GoToStep(TutorialStep.Complete);
                break;
        }
    }
    
    private void OnWelcomeFinished()
    {
        welcomeTextTransition.OnCutsceneComplete -= OnWelcomeFinished;
        welcomeToTutorial.SetActive(false);
        GoToStep(TutorialStep.ClickerButton1);
    }
}


