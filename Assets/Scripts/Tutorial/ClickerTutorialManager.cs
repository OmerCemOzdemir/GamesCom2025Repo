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

    [Header("Interactable Elements")]
    [SerializeField] private GameObject clickerButton;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject exitButton;

    private void Start()
    {
        GoToStep(TutorialStep.WelcomeToTutorial);
    }

    private void GoToStep(TutorialStep step)
    {
        DisableAllPopups();
        currentStep = step;

        switch (step)
        {
            case TutorialStep.WelcomeToTutorial:
                welcomeToTutorial.SetActive(true);
                DisableAllButtons();
                break;

            case TutorialStep.ClickerButton1:
                clickerButton1.SetActive(true);
                clickerButton.SetActive(true);
                break;

            case TutorialStep.MoneyTo100:
                moneyTo100.SetActive(true);
                break;

            case TutorialStep.MoneyReached100:
                moneyReached100.SetActive(true);
                clickerButton.SetActive(false);
                break;

            case TutorialStep.UpgradeButton:
                upgradeButton.SetActive(true);
                upgradePanel.SetActive(true);
                break;

            case TutorialStep.MoneyTo1000:
                moneyTo1000.SetActive(true);
                clickerButton.SetActive(true);
                break;

            case TutorialStep.MoneyReached1000:
                moneyReached1000.SetActive(true);
                clickerButton.SetActive(false);
                break;

            case TutorialStep.EnterMainHub:
                enterMainHub.SetActive(true);
                exitButton.SetActive(true);
                break;

            case TutorialStep.Complete:
                GameManager.Instance.GetGameData().clickerTutorialPlayed = true;
                GameManager.Instance.SaveGame();
                GameManager.Instance.NextLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
                break;
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
    public void OnPopupNext_Welcome()
    {
        GoToStep(TutorialStep.ClickerButton1);
    }

    public void OnReached100Money()
    {
        if (currentStep == TutorialStep.MoneyTo100)
            GoToStep(TutorialStep.MoneyReached100);
    }

    public void OnUpgradeBought()
    {
        if (currentStep == TutorialStep.UpgradeButton)
            GoToStep(TutorialStep.MoneyTo1000);
    }

    public void OnReached1000Money()
    {
        if (currentStep == TutorialStep.MoneyTo1000)
            GoToStep(TutorialStep.MoneyReached1000);
    }

    public void OnExitClicked()
    {
        if (currentStep == TutorialStep.EnterMainHub)
            GoToStep(TutorialStep.Complete);
    }

    private void Update()
    {
        double money = GameManager.Instance.GetGameData().totalMoney;

        if (currentStep == TutorialStep.ClickerButton1 && money >= 1)
            GoToStep(TutorialStep.MoneyTo100);

        if (currentStep == TutorialStep.MoneyTo100 && money >= 100)
            OnReached100Money();

        if (currentStep == TutorialStep.MoneyTo1000 && money >= 1000)
            OnReached1000Money();
    }
}


