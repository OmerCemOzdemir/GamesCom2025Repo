using System;
using TMPro;
using UnityEngine;

public class PlatformerManager : MonoBehaviour
{
    public static event Action<float> onMoneyChange;
    //Used for sending the game object of the interacted game object. Used for sending the game object of ladder.
    public static event Action<GameObject> onGameObjectInteract;
    public static event Action onMoneyZero;
    public static event Action onLadderExit;
    public static event Action onDoorEnter;
    public static event Action onDoorExit;
    public static event Action onKeyEnter;
    public static event Action<int> onDoorCheck;
    public static event Action onBridgeExit;

    private int keyNumber;
    private bool elevatorActive = false;
    private PlayerControler playerControler;

    public int KeyNumber { get => keyNumber; set => keyNumber = value; }

    public static event Action<Interaction> onInteract;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 1.5f, -10f); // default for Z is -10 to prevent 2D clipping issues
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject requiredMoneyText;

    [SerializeField] private float moneyRequiredMove = 10;
    [SerializeField] private float moneyRequiredJump = 100;
    [SerializeField] private float moneyRequiredClimb = 1000;
    [SerializeField] private float moneyRequiredPickUp = 2000;
    [SerializeField] private float moneyRequiredOpenDoor = 3000;
    [SerializeField] private float moneyRequiredUseElevator = 5000;
    [SerializeField] private float moneyRequiredPassBridge = 6000;



    private void OnEnable()
    {
        PlayerControler.onPlayerJump += ReduceMoneyJump;
        PlayerControler.onPlayerMove += ReduceMoneyMove;
        PlayerControler.onPlayerClimb += ReduceMoneyClimb;
        PlayerControler.onPlayerPickUp += ReduceMoneyPickUp;
        PlayerControler.onPlayerOpenDoor += ReduceMoneyDoorOpen;
        PlayerControler.onPlayerUseElevator += ReduceMoneyUseElevator;
        PlayerControler.onPlayerPassBridge += ReduceMoneyPassBridge;
        //------------------------------------------------------------------
        LerpObject.onlerpOpStart += DisableInteractText;
        LerpObject.onlerpOpDone += ToggleElevatorActive;
        LerpObject.onlerpOpStart += ToggleElevatorActive;
        //------------------------------------------------------------------
        UpgradeShop.onItemExchange += SaveGameData;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerJump -= ReduceMoneyJump;
        PlayerControler.onPlayerMove -= ReduceMoneyMove;
        PlayerControler.onPlayerClimb -= ReduceMoneyClimb;
        PlayerControler.onPlayerPickUp -= ReduceMoneyPickUp;
        PlayerControler.onPlayerOpenDoor -= ReduceMoneyDoorOpen;
        PlayerControler.onPlayerPassBridge -= ReduceMoneyPassBridge;
        //------------------------------------------------------------------
        LerpObject.onlerpOpStart -= DisableInteractText;
        LerpObject.onlerpOpDone -= ToggleElevatorActive;
        LerpObject.onlerpOpStart -= ToggleElevatorActive;
        //------------------------------------------------------------------
        UpgradeShop.onItemExchange -= SaveGameData;


    }

    private void Awake()
    {
        playerControler = GetComponent<PlayerControler>();
    }

    private void Update()
    {
        CameraFollow();
    }


    //THis Functions makes the main camera in the Scene to follow the player.
    private void CameraFollow()
    {
        Vector3 targetPosition = transform.position + cameraOffset; // apply offset to camera position
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * 3f);
    }

    private void SaveGameData(bool[] items)
    {
        //This Array holds the items of game: JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
        GameManager.Instance.GetGameData().platformItems = items;
        GameManager.Instance.SaveGame();
    }

    //This function is tied to player movement. When player presses a single movement key this function deducts a certain amount of money.
    //After it reduces the money if the current money is below 0 a game will trigger the onMoneyZero event which disables the player controls
    private void ReduceMoneyMove()
    {
        //Vector2 horizontalMovement = context.ReadValue<Vector2>();
        GameManager.Instance.GetGameData().totalMoney -= moneyRequiredMove;
        onMoneyChange?.Invoke(moneyRequiredMove);
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    //This function is tied to player jump ability. When player succesfully initiates a jump action, this function deducts a certain amount of money.
    //After it reduces the money if the current money is below 0 a game will trigger the onMoneyZero event which disables the player controls
    private void ReduceMoneyJump()
    {
        GameManager.Instance.GetGameData().totalMoney -= moneyRequiredJump;
        onMoneyChange?.Invoke(moneyRequiredJump);
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    //This function is tied to player climb ability. When player succesfully mounts on a ladder, this function deducts a certain amount of money.
    //After it reduces the money if the current money is below 0 a game will trigger the onMoneyZero event which disables the player controls
    private void ReduceMoneyClimb()
    {
        GameManager.Instance.GetGameData().totalMoney -= moneyRequiredClimb;
        onMoneyChange?.Invoke(moneyRequiredClimb);
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    private void ReduceMoneyPickUp()
    {
        GameManager.Instance.GetGameData().totalMoney -= moneyRequiredPickUp;
        onMoneyChange?.Invoke(moneyRequiredPickUp);
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    private void ReduceMoneyDoorOpen()
    {
        GameManager.Instance.GetGameData().totalMoney -= moneyRequiredOpenDoor;
        onMoneyChange?.Invoke(moneyRequiredOpenDoor);
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    private void ReduceMoneyUseElevator()
    {
        GameManager.Instance.GetGameData().totalMoney -= moneyRequiredUseElevator;
        onMoneyChange?.Invoke(moneyRequiredUseElevator);
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    private void ReduceMoneyPassBridge()
    {
        GameManager.Instance.GetGameData().totalMoney -= moneyRequiredPassBridge;
        onMoneyChange?.Invoke(moneyRequiredPassBridge);
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }


    private void EnableInteractText(string txt)
    {
        interactText.SetActive(true);
        requiredMoneyText.SetActive(true);
        requiredMoneyText.GetComponent<TextMeshProUGUI>().text = txt;
    }

    private void DisableInteractText()
    {
        interactText.SetActive(false);
        requiredMoneyText.SetActive(false);
    }

    private void ToggleElevatorActive()
    {
        elevatorActive = !elevatorActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("This object : " + collision.gameObject);
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Ladder);
            if (playerControler.LocalItems != null)
            {
                // Climb Gloves: 3 = Climbing free
                if (playerControler.LocalItems[3])
                {
                    EnableInteractText("FREE");
                }
                else
                {
                    EnableInteractText("$" + moneyRequiredClimb);
                }
            }

        }

        if (collision.CompareTag("Door"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Door);
            onDoorEnter?.Invoke();
            onDoorCheck?.Invoke(keyNumber);
            EnableInteractText("$" + moneyRequiredOpenDoor);

        }

        if (collision.CompareTag("Key"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Key);
            onKeyEnter?.Invoke();
            EnableInteractText("$" + moneyRequiredPickUp);

        }

        if (collision.CompareTag("Elevator"))
        {
            if (!elevatorActive)
            {
                //Debug.Log("Ladder can NOT be used");
                onInteract?.Invoke(Interaction.Elevator);
                EnableInteractText("$" + moneyRequiredUseElevator);
            }
        }

        if (collision.CompareTag("Bridge"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Bridge);
            EnableInteractText("$" + moneyRequiredPassBridge);

        }

        if (collision.CompareTag("Shop"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Shop);
            EnableInteractText("Upgrade Shop");

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("This object : " + collision.gameObject);
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder can be used");
            onInteract?.Invoke(Interaction.Ladder);
            onGameObjectInteract?.Invoke(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("This object : " + collision.gameObject);
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder can NOT be used");
            onLadderExit?.Invoke();
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();
        }

        if (collision.CompareTag("Door"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            onDoorExit?.Invoke();
            DisableInteractText();
        }

        if (collision.CompareTag("Key"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();
        }


        if (collision.CompareTag("Elevator"))
        {
            if (!elevatorActive)
            {
                //Debug.Log("Ladder can NOT be used");
                onInteract?.Invoke(Interaction.Empty);
                DisableInteractText();
            }
        }

        if (collision.CompareTag("Bridge"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            onBridgeExit?.Invoke();
            DisableInteractText();

        }

        if (collision.CompareTag("Shop"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();

        }

    }

}

/*
   if (Input.GetKey(KeyCode.RightArrow))
        {
            onMoneyChange?.Invoke();
            GameManager.Instance.GetGameData().totalMoney -= 10;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            onMoneyChange?.Invoke();
            GameManager.Instance.GetGameData().totalMoney -= 10;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onMoneyChange?.Invoke();
            GameManager.Instance.GetGameData().totalMoney -= 100;
        }

         if (!moneyBelowZero)
        {
            ReduceMoney();
        }
 

    private void ReduceMoney()
    {

        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            moneyBelowZero = true;
        }


    }

    private void DisableInput()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
    }

    private void DebugInputSystem()
    {
        Debug.Log("Disable Input : " + playerInputAction.PlayerPlatform.Move.enabled);
    }
 */