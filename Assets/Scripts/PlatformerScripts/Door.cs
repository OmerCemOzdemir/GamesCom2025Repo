using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject keyText;
    private Animator doorAnimator;
    private Animator targetAnimator;

    private bool isDoorOpen = false;
    private bool hasKey = false;
    private bool toggleDoor = true;
    public static event Action<Vector3, float> onDoorTravel;

    private Transform doorTravelPosition;
    [SerializeField] private bool requireKey = false; //default doesnt require key to open door;
    [SerializeField] private float delayOnTeleport;

    private void Awake()
    {
        doorAnimator = GetComponent<Animator>();
        targetAnimator = transform.GetChild(0).GetComponent<Animator>();

        keyText = transform.GetChild(1).GetChild(0).gameObject;
        doorTravelPosition = transform.GetChild(0).gameObject.transform;
    }

    private void OnEnable()
    {
        if (!requireKey)
        {
            PlatformerManager.onDoorEnter += DoorOpenAnimPlay;
            PlatformerManager.onDoorExit += DoorCloseAnimPlay;
        }
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
            targetAnimator.SetTrigger("DoorOpen");
            isDoorOpen = true;
        }
    }

    private void DoorCloseAnimPlay()
    {
        if (isDoorOpen)
        {
            doorAnimator.SetTrigger("DoorClose");
            targetAnimator.SetTrigger("DoorClose");
            isDoorOpen = false;
        }
    }

    public void OpenDoor()
    {
        if (toggleDoor)
        {
            onDoorTravel.Invoke(doorTravelPosition.position, delayOnTeleport);
            toggleDoor = false;
        }
        else
        {
            onDoorTravel.Invoke(transform.position, delayOnTeleport);
            toggleDoor = true;
        }

    }

    private void OpenDoorWithKey()
    {
        if (hasKey)
        {
            if (toggleDoor)
            {
                onDoorTravel.Invoke(doorTravelPosition.position, delayOnTeleport);
                toggleDoor = false;
            }
            else
            {
                onDoorTravel.Invoke(transform.position, delayOnTeleport);
                toggleDoor = true;
            }
        }
    }


    public void CheckKey(int key)
    {
        if (key == 0)
        {

        }
        else if (key >= 1)
        {
            PlatformerManager.onDoorEnter += DoorOpenAnimPlay;
            PlatformerManager.onDoorExit += DoorCloseAnimPlay;
            hasKey = true;
        }
    }

    public void CheckDoor()
    {
        if (requireKey)
        {
            OpenDoorWithKey();
            keyText.SetActive(true);
        }
        else
        {
            OpenDoor();
        }
    }


}
