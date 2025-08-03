using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerManager : MonoBehaviour
{
    public static event Action<float, string> onMoneyChange;
    public static event Action onMoneyCheck;

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
        PlayerControler.onMoneySpent += SpentMoney;
        //------------------------------------------------------------------
        TestScript.onDataChange += LoadGameData;
    }

    private void OnDisable()
    {
        PlayerControler.onMoneySpent -= SpentMoney;
        //------------------------------------------------------------------
        TestScript.onDataChange -= LoadGameData;

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

    private void LoadGameData()
    {
        //This Array holds the items of game: JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
        //GameManager.Instance.GetGameData().platformItems;
        GameManager.Instance.SaveGame();
        GameManager.Instance.LoadGame();
        onMoneyChange?.Invoke(0, "");

    }

    public bool CheckMoney(MoneySpent moneySpent)
    {
        switch (moneySpent)
        {

            case MoneySpent.moneySpentJump:
                return CheckMoneyReduction(moneyRequiredJump);
            case MoneySpent.moneySpentClimb:
                return CheckMoneyReduction(moneyRequiredClimb);
            case MoneySpent.moneySpentPickUp:
                return CheckMoneyReduction(moneyRequiredPickUp);
            case MoneySpent.moneySpentOpenDoor:
                return CheckMoneyReduction(moneyRequiredOpenDoor);
            case MoneySpent.moneySpentPassBridge:
                return CheckMoneyReduction(moneyRequiredPassBridge);
            case MoneySpent.moneySpentUseElevator:
                return CheckMoneyReduction(moneyRequiredUseElevator);
        }
        return false;

    }

    private bool CheckMoneyReduction(float moneyReq)
    {
        double totalMoney = GameManager.Instance.GetGameData().totalMoney;
        double calcMoney = totalMoney - moneyReq;
        if (calcMoney <= 0)
        {
            onMoneyCheck?.Invoke();
        }

        return calcMoney > 0;
    }


    //These Functions calculate the money spent and reduce the money.
    private void SpentMoney(MoneySpent moneySpent)
    {
        switch (moneySpent)
        {
            case MoneySpent.moneySpentMove:
                RecudeMoney(moneyRequiredMove, "Move");
                break;
            case MoneySpent.moneySpentJump:
                RecudeMoney(moneyRequiredJump, "Jump");

                break;
            case MoneySpent.moneySpentClimb:
                RecudeMoney(moneyRequiredClimb, "Climb");

                break;
            case MoneySpent.moneySpentPickUp:
                RecudeMoney(moneyRequiredPickUp, "Pick Up");

                break;
            case MoneySpent.moneySpentOpenDoor:
                RecudeMoney(moneyRequiredOpenDoor, "Door");

                break;
            case MoneySpent.moneySpentPassBridge:
                RecudeMoney(moneyRequiredPassBridge, "Bridge");

                break;
            case MoneySpent.moneySpentUseElevator:
                RecudeMoney(moneyRequiredUseElevator, "Elevator");

                break;
        }
    }

    private void RecudeMoney(float moneyReq, string text)
    {
        double money = GameManager.Instance.GetGameData().totalMoney;
        double calcMoney = money - moneyReq;
        if (calcMoney <= 0)
        {
            if (SceneManager.GetActiveScene().name != "MainHubScene")
            {
                GameManager.Instance.GetGameData().totalMoney = 0;
                onMoneyChange?.Invoke(moneyReq, text);
                onMoneyZero?.Invoke();
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "MainHubScene")
            {
                //Debug.Log("Reduced Money: " + text + " " + moneyReq);
                GameManager.Instance.GetGameData().totalMoney = calcMoney;
                onMoneyChange?.Invoke(moneyReq, text);
            }

        }

    }

    public void UpdateItemMoneyEffects(int index, MoneySpent moneySpent)
    {
        float value = playerControler.UpgradeItems[index].itemEffectOnMoney.value;
        Operations op = playerControler.UpgradeItems[index].itemEffectOnMoney.operations;
        Debug.Log("the Item: " + playerControler.UpgradeItems[index].itemName + " : " + value + " , " + op);
        switch (moneySpent)
        {
            case MoneySpent.moneySpentMove:
                moneyRequiredMove = playerControler.ImplementOperations(moneyRequiredMove, value, op);
                break;
            case MoneySpent.moneySpentJump:
                moneyRequiredJump = playerControler.ImplementOperations(moneyRequiredJump, value, op);

                break;
            case MoneySpent.moneySpentClimb:
                moneyRequiredClimb = playerControler.ImplementOperations(moneyRequiredClimb, value, op);

                break;
            case MoneySpent.moneySpentPickUp:
                moneyRequiredPickUp = playerControler.ImplementOperations(moneyRequiredPickUp, value, op);

                break;
            case MoneySpent.moneySpentOpenDoor:
                moneyRequiredOpenDoor = playerControler.ImplementOperations(moneyRequiredOpenDoor, value, op);

                break;
            case MoneySpent.moneySpentPassBridge:
                moneyRequiredPassBridge = playerControler.ImplementOperations(moneyRequiredPassBridge, value, op);

                break;
            case MoneySpent.moneySpentUseElevator:
                moneyRequiredUseElevator = playerControler.ImplementOperations(moneyRequiredUseElevator, value, op);

                break;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("This object : " + collision.gameObject);
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Ladder);
            EnableInteractText("$" + moneyRequiredClimb);
        }

        if (collision.CompareTag("Door"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Door);
            onDoorEnter?.Invoke();
            onDoorCheck?.Invoke(keyNumber);
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("$" + moneyRequiredOpenDoor);

        }

        if (collision.CompareTag("Key"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Key);
            onKeyEnter?.Invoke();
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("$" + moneyRequiredPickUp);

        }

        if (collision.CompareTag("Elevator"))
        {

            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Elevator);
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("$" + moneyRequiredUseElevator);

        }

        if (collision.CompareTag("Bridge"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Bridge);
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("$" + moneyRequiredPassBridge);

        }

        if (collision.CompareTag("Shop"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Shop);
            EnableInteractText("Upgrade Shop");

        }

        if (collision.CompareTag("Taxi"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Taxi);
            EnableInteractText("Bus");

        }

        if (collision.CompareTag("Sign"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Sign);
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("");
        }

        if (collision.CompareTag("Wallet"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Wallet);
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("");
        }

        if (collision.CompareTag("Item"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Item);
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("");
        }

        if (collision.CompareTag("ElevatorControl"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.ElevatorControl);
            onGameObjectInteract?.Invoke(collision.gameObject);
            EnableInteractText("");
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

            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();

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

        if (collision.CompareTag("Taxi"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();

        }

        if (collision.CompareTag("Sign"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();
        }

        if (collision.CompareTag("Wallet"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();
        }

        if (collision.CompareTag("Item"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            DisableInteractText();
        }

        if (collision.CompareTag("ElevatorControl"))
        {
            //Debug.Log("Ladder can NOT be used");
            onInteract?.Invoke(Interaction.Empty);
            EnableInteractText("");
        }

    }

}

public enum MoneySpent
{
    moneySpentMove,
    moneySpentJump,
    moneySpentClimb,
    moneySpentPickUp,
    moneySpentOpenDoor,
    moneySpentUseElevator,
    moneySpentPassBridge
}


/*
 * 
    private void SaveGameData(bool[] items)
    {
        //This Array holds the items of game: JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
        GameManager.Instance.GetGameData()._platformItems = items;
        GameManager.Instance.SaveGame();
    }

 * 
 * 
 * 
 *     [SerializeField] private float moneyRequiredMove = 10;
    [SerializeField] private float moneyRequiredJump = 100;
    [SerializeField] private float moneyRequiredClimb = 1000;
    [SerializeField] private float moneyRequiredPickUp = 2000;
    [SerializeField] private float moneyRequiredOpenDoor = 3000;
    [SerializeField] private float moneyRequiredUseElevator = 5000;
    [SerializeField] private float moneyRequiredPassBridge = 6000;

 * 
 * 
 * 
 * 
 * 
 * 
 *     //This function is tied to player movement. When player presses a single movement key this function deducts a certain amount of money.
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


 * 
 * 
 * 
 *             if (playerControler.LocalItems != null)
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
 * 
 * 
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

        PlayerControler.onPlayerJump += ReduceMoneyJump;
        PlayerControler.onPlayerMove += ReduceMoneyMove;
        PlayerControler.onPlayerClimb += ReduceMoneyClimb;
        PlayerControler.onPlayerPickUpKey += ReduceMoneyPickUp;
        PlayerControler.onPlayerOpenDoor += ReduceMoneyDoorOpen;
        PlayerControler.onPlayerUseElevator += ReduceMoneyUseElevator;
        PlayerControler.onPlayerPassBridge += ReduceMoneyPassBridge;

        PlayerControler.onPlayerJump -= ReduceMoneyJump;
        PlayerControler.onPlayerMove -= ReduceMoneyMove;
        PlayerControler.onPlayerClimb -= ReduceMoneyClimb;
        PlayerControler.onPlayerPickUpKey -= ReduceMoneyPickUp;
        PlayerControler.onPlayerOpenDoor -= ReduceMoneyDoorOpen;
        PlayerControler.onPlayerPassBridge -= ReduceMoneyPassBridge;
 */