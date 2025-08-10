using UnityEngine;

public class ClickerParticleEffect : MonoBehaviour
{
    private ParticleSystem clickerEffect;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];

    private void Awake()
    {
        clickerEffect = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnParticle();
        }
    }

    public void IncreaseClickEffect(float rate)
    {
        var emission = clickerEffect.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
    }

    public void SpawnParticle()
    {
        clickerEffect.SetParticles(particles, particles.Length);
    }

    public void StartEffect()
    {
        clickerEffect.Play();
    }

    public void PauseEffect()
    {
        clickerEffect.Pause();
    }

    public void StopEffect()
    {
        clickerEffect.Stop();
    }


}
