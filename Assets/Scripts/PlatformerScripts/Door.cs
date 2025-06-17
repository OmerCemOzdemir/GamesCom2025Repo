using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private Animator doorAnimator;
    private bool isDoorOpen = false;
    private bool hasKey = false;
    [SerializeField] private bool requireKey = false; //default doesnt require key to open door;
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private GameObject keyText;

    private void Awake()
    {
        doorAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (!requireKey)
        {
            PlatformerManager.onDoorEnter += DoorOpenAnimPlay;
            PlatformerManager.onDoorExit += DoorCloseAnimPlay;
        }

        PlayerControler.onPlayerDoorOpen += CheckDoor;
        PlatformerManager.onDoorCheck += CheckKey;
    }

    private void OnDisable()
    {

        PlatformerManager.onDoorEnter -= DoorOpenAnimPlay;
        PlatformerManager.onDoorExit -= DoorCloseAnimPlay;

        PlayerControler.onPlayerDoorOpen -= CheckDoor;
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
        GameManager.Instance.NextLevel(levelIndex);
    }

    private void OpenDoorWithKey()
    {
        if (hasKey)
        {
            GameManager.Instance.NextLevel(levelIndex);
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
