using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private GameObject blackOutPanel;

    private void OnEnable()
    {
        Door.onDoorTravel += StartFadeEffect;
    }

    private void OnDisable()
    {
        Door.onDoorTravel -= StartFadeEffect;
    }


    private void StartFadeEffect(Vector3 pos, float sec)
    {
        float halfSec = sec / 2;
        Debug.Log("halfSec: " + halfSec);
        StartCoroutine(FadeIn(halfSec));
        Debug.Log("FadeEffectRoutine");
    }


    IEnumerator FadeIn(float sec)
    {
        Color originalColor = blackOutPanel.GetComponent<Image>().color;
        float elapsed = 0f;

        while (elapsed < sec)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsed / sec);
            blackOutPanel.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null; // wait for next frame
        }

        // Ensure it's fully transparent at the end
        blackOutPanel.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        EndFadeEffect(sec);
    }

    private void EndFadeEffect(float sec)
    {
        Debug.Log("Stop FadeEffectRoutine");
        StartCoroutine(FadeOut(sec));

    }

    IEnumerator FadeOut(float sec)
    {
        Color originalColor = blackOutPanel.GetComponent<Image>().color;
        float startAlpha = originalColor.a;
        float elapsed = 0f;

        while (elapsed < sec)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / sec);
            blackOutPanel.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null; // wait for next frame
        }

        // Ensure it's fully transparent at the end
        blackOutPanel.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }




    //Will be done
    IEnumerator FadeEffectRoutine(float sec)
    {
        Color currentColor = blackOutPanel.GetComponent<Image>().color;
        Color addColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.0001f);
        bool toggle = false;
        float timePassed = 0;
        while (timePassed < sec)
        {

            if (1f <= blackOutPanel.GetComponent<Image>().color.a)
            {
                Debug.Log("toggle: " + toggle);

                toggle = true;
            }
            //Debug.Log("timePassed: " + timePassed + "sec: " + sec);

            if (toggle)
            {
                //Debug.Log("timePassed in toggle true: " + timePassed);

                currentColor -= addColor;
                blackOutPanel.GetComponent<Image>().color = currentColor;
            }
            else
            {
                // Debug.Log("timePassed in toggle true: " + timePassed);

                currentColor += addColor;
                blackOutPanel.GetComponent<Image>().color = currentColor;
            }

            timePassed += Time.fixedDeltaTime;
            yield return null; // wait for next frame
        }

        //float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / halfSec);
        //float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / halfSec);
        // Ensure it's fully transparent at the end
        blackOutPanel.GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
    }





}
/*
 
    //, float startAlpha, float endAlpha
    IEnumerator FadeEffectRoutine(float timer, GameObject obj)
    {



        obj.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
            obj.GetComponent<Image>().color -= new Color(0, 0, 0, timer / 2);
        }
        obj.GetComponent<Image>().color = new Color(0, 0, 0, 0f);

    }

    IEnumerator FadeText(int timer, GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.black;
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1f);
            obj.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
            obj.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.1f);
        }

        yield return new WaitForSeconds(2f);

        Destroy(obj);
        obj = null;

    }



    //Will be done
    IEnumerator FadeEffect(float sec)
    {
        Color currentColor = blackOutPanel.GetComponent<Image>().color;
        Color addColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.1f);
        bool toggle = false;
        float timePassed = 0;
        while (timePassed < sec)
        {

            if (1f <= blackOutPanel.GetComponent<Image>().color.a)
            {
                Debug.Log("toggle: " + toggle);

                toggle = true;
            }
            Debug.Log("timePassed: " + timePassed + "sec: " + sec);

            if (toggle)
            {
                Debug.Log("timePassed in toggle true: " + timePassed);

                currentColor -= addColor;
                blackOutPanel.GetComponent<Image>().color = currentColor;
            }
            else
            {
                Debug.Log("timePassed in toggle true: " + timePassed);

                currentColor += addColor;
                blackOutPanel.GetComponent<Image>().color = currentColor;
            }

            timePassed += Time.deltaTime;
            yield return null; // wait for next frame
        }

        //float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / halfSec);
        //float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / halfSec);
        // Ensure it's fully transparent at the end
        blackOutPanel.GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
    }



 
 
 */