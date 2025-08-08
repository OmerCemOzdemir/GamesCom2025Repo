using UnityEngine;

public class ClickerEffect : MonoBehaviour
{
    private ParticleSystem clickerEffect;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        clickerEffect = GetComponent<ParticleSystem>();
    }

    public void IncreaseClickEffect(float rate)
    {
        var emission = clickerEffect.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
    }

    public void StartEffect()
    {
        clickerEffect.Play();
    }

    public void StopEffect()
    {
        clickerEffect.Pause();

    }

}
