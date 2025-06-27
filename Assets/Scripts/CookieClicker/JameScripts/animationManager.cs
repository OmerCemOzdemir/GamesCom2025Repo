using Unity.Mathematics;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float lastClickTime = 0.0f;
    private float clickInterval = 0.0f;

    [SerializeField]
    public float baseAnimationSpeed = 1.0f;
    public float clickSpeedMultiplier = 1.0f;

    private void OnEnable()
    {
        ClickerManager.onActiveClick += playAnimation;
    }

    private void OnDisable()
    {
        ClickerManager.onActiveClick -= playAnimation;
    }

    private void Awake()
    {
        lastClickTime = Time.time;
    }

    private void animationSpeedCul()
    {
        if (Time.time - lastClickTime > 1.0f)
        {

            clickSpeedMultiplier = baseAnimationSpeed;

        }

        clickInterval = Time.time - lastClickTime;
        lastClickTime = Time.time;

        // Calculate multiplier 
        clickSpeedMultiplier = 1.0f / clickInterval;
        //Consider clamping
        clickSpeedMultiplier = Mathf.Clamp(clickSpeedMultiplier, 1.0f, 2.0f);

        animator.SetFloat("Speed", clickSpeedMultiplier);

       

    }

    private void playAnimation()
    {
        if(animator.enabled == false)
        {
            animator.enabled = true;
        }
        animationSpeedCul();
        animator.Play("Click2", 0, 0f);
    }
}
