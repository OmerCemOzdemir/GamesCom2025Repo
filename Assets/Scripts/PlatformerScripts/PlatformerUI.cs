
using System.Collections;
using TMPro;
using UnityEngine;

public class PlatformerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI lostMoneyText;
    [SerializeField] private GameObject keyImage;


    private void Awake()
    {
        UpdateMoneyText(0);
    }

    public void UpdateMoneyText(float money)
    {
        moneyText.text = "$" + GameManager.Instance.GetGameData().totalMoney;
    }

    private void EnableKeyImage()
    {
        keyImage.SetActive(true);
    }

    private void DisableKeyImage()
    {
        keyImage.SetActive(true);
    }

    private void UpdateLostMoney(float money)
    {
        StopAllCoroutines();
        lostMoneyText.color = new Color(1, 0, 0, 1f);
        lostMoneyText.gameObject.SetActive(true);
        lostMoneyText.text = "-$" + money;
        StartCoroutine(DecreaseTransparency(2));
    }

    IEnumerator DecreaseTransparency(float sec)
    {
        Color originalColor = lostMoneyText.color;
        float startAlpha = originalColor.a;
        float elapsed = 0f;

        while (elapsed < sec)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / sec);
            lostMoneyText.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null; // wait for next frame
        }

        // Ensure it's fully transparent at the end
        lostMoneyText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    private void OnEnable()
    {
        PlatformerManager.onMoneyChange += UpdateMoneyText;
        PlatformerManager.onMoneyChange += UpdateLostMoney;
        PlayerControler.onPlayerPickUp += EnableKeyImage;
    }

    private void OnDisable()
    {
        PlatformerManager.onMoneyChange -= UpdateMoneyText;
        PlatformerManager.onMoneyChange -= UpdateLostMoney;
        PlayerControler.onPlayerPickUp -= EnableKeyImage;

    }

}

/*
 
 
        int i = 0;
        while (i < totalSec)
        {

            yield return new WaitForSeconds(sec);
            i++;
        }

        for (int i = 0; i < sec; i++)
        {
            lostMoneyText.color -= new Color(0, 0, 0, 0.1f);
            yield return new WaitForSeconds(sec);
        }

 */