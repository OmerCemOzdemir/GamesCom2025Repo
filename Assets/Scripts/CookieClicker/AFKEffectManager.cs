using UnityEngine;

public class AFKEffectManager : MonoBehaviour
{
    [SerializeField] private ClickerEffects AFK_1_effects;
    [SerializeField] private ClickerEffects AFK_2_effects;
    [SerializeField] private ClickerEffects AFK_3_effects;
    [SerializeField] private ClickerEffects AFK_4_effects;

    int index = 0;

    public void ActivateEffects()
    {
        switch (index)
        {
            case 0:
                AFK_1_effects.gameObject.SetActive(true);
                break;
            case 1:
                AFK_2_effects.gameObject.SetActive(true);
                break;
            case 2:
                AFK_3_effects.gameObject.SetActive(true);
                break;
            case 3:
                AFK_4_effects.gameObject.SetActive(true);
                break;
        }

        index++;

    }

    public void SpawnEffects()
    {
        if (AFK_1_effects.gameObject.activeSelf)
        {
            AFK_1_effects.SpawnParticle();

        }
        if (AFK_2_effects.gameObject.activeSelf)
        {
            AFK_2_effects.SpawnParticle();

        }
        if (AFK_3_effects.gameObject.activeSelf)
        {
            AFK_3_effects.SpawnParticle();

        }

    }


}
