using System.Collections;
using UnityEngine;

public class ClickerEffects : MonoBehaviour
{
    [SerializeField] private GameObject moneyParticle;
    [Header("Effect Pref: ")]
    [Space(5)]

    [Header("Set Base Speed: ")]
    [SerializeField] private float basePosIncreament = 0.001f;
    [Header("Set Base Scale Decrease: ")]
    [SerializeField] private float baseScaleDecrease = 0.003f;
    [Space(10)]

    [Header("Set Random Spread: ")]
    [Range(0, 1)]
    [SerializeField] private float minRandMultiplier = 1.0f;
    [Range(0, 1)]
    [SerializeField] private float maxRandMultiplier = 1.0f;
    [Range(0, 0.1f)]
    [Header("Set Random X Speed: ")]
    [SerializeField] private float minRandMultiplierX = 0f;
    [Range(0, 0.1f)]
    [SerializeField] private float maxRandMultiplierX = 0.04f;
    [Header("Set Random Y Speed: ")]
    [Range(0, 0.1f)]
    [SerializeField] private float minRandMultiplierY = 0f;
    [Range(0, 0.1f)]
    [SerializeField] private float maxRandMultiplierY = 0.04f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnParticle();
        }
    }


    public void SpawnParticle()
    {
        GameObject newParticle = Instantiate(moneyParticle, transform.position, Quaternion.identity);
        newParticle.transform.SetParent(transform, true);
        newParticle.transform.localScale = Vector3.one;
        StartCoroutine(TransformParticle(newParticle));
    }

    private IEnumerator TransformParticle(GameObject particle)
    {

        float randMultiplierX = Random.Range(minRandMultiplierX, maxRandMultiplierX);
        float randMultiplierY = Random.Range(minRandMultiplierY, maxRandMultiplierY);
        float randMultiplier = Random.Range(maxRandMultiplier, minRandMultiplier);


        int randDirection = Random.Range(0, 2);
        while (particle.transform.localScale.y >= 0 && particle.transform.localScale.x >= 0)
        {
            if (randDirection == 0)
            {
                particle.transform.position += new Vector3(-(randMultiplier * (basePosIncreament + randMultiplierX)), Mathf.Pow(basePosIncreament, 2) + randMultiplierY, 0);
            }
            else
            {
                particle.transform.position += new Vector3((randMultiplier * (basePosIncreament + randMultiplierX)), Mathf.Pow(basePosIncreament, 2) + randMultiplierY, 0);
            }
            particle.transform.localScale -= new Vector3(baseScaleDecrease, baseScaleDecrease, 0);
            //Debug.Log(" particle: Pos:" + particle.transform.position + " Scale: " + particle.transform.localScale);
            yield return null;
        }

        Destroy(particle);
    }


}
