using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator doorAnimator;
    private bool isDoorOpen = false;

    private void Awake()
    {
        doorAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlatformerManager.onDoorEnter += DoorOpenAnimPlay;
        PlatformerManager.onDoorExit += DoorCloseAnimPlay;
    }

    private void OnDisable()
    {
        PlatformerManager.onDoorEnter -= DoorOpenAnimPlay;
        PlatformerManager.onDoorExit -= DoorCloseAnimPlay;
    }

    private void DoorOpenAnimPlay()
    {
        if (!isDoorOpen)
        {
            doorAnimator.SetTrigger("DoorOpen");
            isDoorOpen = true;
        }
    }

    private void DoorCloseAnimPlay()
    {
        if (isDoorOpen)
        {
            doorAnimator.SetTrigger("DoorClose");
            isDoorOpen = false;
        }

    }



}
