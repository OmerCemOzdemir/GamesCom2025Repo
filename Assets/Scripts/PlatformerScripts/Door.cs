using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject keyText;
    private Animator doorAnimator;
    private bool isDoorOpen = false;
    private bool hasKey = false;
    public static event Action<Vector3, float> onDoorTravel;

    private Transform doorTravelPosition;
    [SerializeField] private bool requireKey = false; //default doesnt require key to open door;
    [SerializeField] private float delayOnTeleport;

    private void Awake()
    {
        doorAnimator = GetComponent<Animator>();
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

        PlayerControler.onPlayerOpenDoor += CheckDoor;
        PlatformerManager.onDoorCheck += CheckKey;
    }

    private void OnDisable()
    {

        PlatformerManager.onDoorEnter -= DoorOpenAnimPlay;
        PlatformerManager.onDoorExit -= DoorCloseAnimPlay;

        PlayerControler.onPlayerOpenDoor -= CheckDoor;
        PlatformerManager.onDoorCheck -= CheckKey;
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

    private void OpenDoor()
    {
        onDoorTravel.Invoke(doorTravelPosition.position, delayOnTeleport);
    }

    private void OpenDoorWithKey()
    {
        if (hasKey)
        {
            onDoorTravel.Invoke(doorTravelPosition.position, delayOnTeleport);
        }
    }


    private void CheckKey(int key)
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

    private void CheckDoor()
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
