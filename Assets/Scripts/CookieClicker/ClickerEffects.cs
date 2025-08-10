using System.Collections;
using UnityEngine;

public class ClickerEffects : MonoBehaviour
{
    [SerializeField] private GameObject moneyParticle;
    [Header("Effect Pref: ")]
    [SerializeField] private float basePosIncreament = 0.001f;
    [SerializeField] private float baseScaleDecrease = 0.003f;
    [Range(0, 1)]
    [SerializeField] private float minRandMultiplier = 1.0f;
    [Range(1, 2)]
    [SerializeField] private float maxRandMultiplier = 2.0f;


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

        float randMultiplierX = Random.Range(0, 0.04f);
        float randMultiplierY = Random.Range(0, 0.04f);
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
