using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    public static event Action onPlayerJump;
    public static event Action onPlayerMove;
    public static event Action onPlayerClimb;

    public static event Action onPlayerPickUpKey;
    public static event Action onPlayerPickUpWallet;
    public static event Action onPlayerPickUpItem;

    public static event Action onPlayerOpenDoor;
    public static event Action onPlayerUseElevator;
    public static event Action onPlayerPassBridge;
    public static event Action onPlayerOpenShop;
    public static event Action onPlayerGetInTaxi;



    [SerializeField] private float playerSpeed = 5; // default value is 5
    [SerializeField] private float playerSprintMultiplier = 1.5f; // default value is 1.5f
    [SerializeField] private float playerJumpPower = 13; // default value is 13
    [SerializeField] private float playerJumpPowerMultiplier = 1.5f; // default value is 1.5f
    [SerializeField] private float playerGravityActivationTime = 0.6f; // default value is 0.6
    [SerializeField] private float playerDefaultGravityScale = 5; // default value is 5
    [SerializeField] private float playerMaxGravityMultiplier = 7; // default value is 7
    private float playerGravityActivationTimeTemp;
    //private float groundCheckRadius = 0.2f;

    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform groundCheck; // assign groundcheck gameObject

    private Rigidbody2D playerRigid2D;
    private InputSystem playerInputAction;
    private Animator playerAnimator;
    private Interaction interaction = Interaction.Empty; //Default is Ladder
    private PlatformerManager platformerManager;


    private Vector2 flipSpriteVector;
    //private Vector3 currentLocalScale;

    private GameObject currentInteractedGameObject;

    //This Array holds the items of game temp: JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
    private bool[] localItems;
    private bool enableMove = true;
    private bool isJumping = false;
    private bool climb = false;
    private bool toggleClimb = true;
    private bool interactionToggle = true;
    public InputSystem PlayerInputAction { get => playerInputAction; set => playerInputAction = value; }
    public bool[] LocalItems { get => localItems; set => localItems = value; }

    private void Awake()
    {
        playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        playerRigid2D = GetComponent<Rigidbody2D>();
        platformerManager = GetComponent<PlatformerManager>();
        playerGravityActivationTimeTemp = playerGravityActivationTime;
        playerRigid2D.gravityScale = playerDefaultGravityScale;
        playerInputAction = new InputSystem();
        localItems = new bool[4];
        for (int i = 0; i < localItems.Length; i++)
        {
            localItems[i] = false;
        }
    }

    private void OnEnable()
    {
        playerInputAction.PlayerPlatform.Move.Enable();
        playerInputAction.PlayerPlatform.Jump.Enable();
        playerInputAction.PlayerPlatform.Interact.Enable();

        playerInputAction.PlayerPlatform.Interact.performed += Interact;

        playerInputAction.PlayerPlatform.Move.performed += FlipSprite;
        playerInputAction.PlayerPlatform.Move.started += FlipDeterminator;
        playerInputAction.PlayerPlatform.Move.canceled += AnimSetIdle;
        playerInputAction.PlayerPlatform.Move.performed += PlayerMoved;


        playerInputAction.PlayerPlatform.Jump.started += JumpStart;
        playerInputAction.PlayerPlatform.Jump.canceled += JumpEnd;

        playerInputAction.PlayerPlatform.Sprint.started += SprintStart;
        playerInputAction.PlayerPlatform.Sprint.canceled += SprintEnd;

        //-----------------------------------------------------
        PlatformerManager.onMoneyZero += DisableMovement;
        PlatformerManager.onMoneyZero += DisableInput;

        PlatformerManager.onLadderExit += DismountLadder;
        PlatformerManager.onInteract += SetUpInteraction;
        UpgradeShop.onItemExchange += UpdateLocalItems;
        //-----------------------------------------------------
        //Elevator Operation is Done
        Elevator.onElevatorDone += ToggleInteraction;
        //Bridge is Passed 
        Bridge.onBridgeDone += ToggleInteraction;
        //
        PlatformerManager.onGameObjectInteract += SetUpGameObjectInteraction;
        Door.onDoorTravel += TeleportPlayer;
        TaxiUI.onPlayerTravel += TeleportPlayer;
    }

    private void OnDisable()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
        playerInputAction.PlayerPlatform.Interact.Disable();
        playerInputAction.PlayerPlatform.Sprint.Disable();

        playerInputAction.PlayerPlatform.Interact.performed -= Interact;

        playerInputAction.PlayerPlatform.Move.performed -= FlipSprite;
        playerInputAction.PlayerPlatform.Move.started -= FlipDeterminator;
        playerInputAction.PlayerPlatform.Move.canceled -= AnimSetIdle;
        playerInputAction.PlayerPlatform.Move.performed -= PlayerMoved;

        playerInputAction.PlayerPlatform.Jump.started -= JumpStart;
        playerInputAction.PlayerPlatform.Jump.canceled -= JumpEnd;

        playerInputAction.PlayerPlatform.Sprint.started -= SprintStart;
        playerInputAction.PlayerPlatform.Sprint.canceled -= SprintEnd;
        //-----------------------------------------------------
        PlatformerManager.onMoneyZero -= DisableMovement;
        PlatformerManager.onMoneyZero -= DisableInput;

        PlatformerManager.onLadderExit -= DismountLadder;
        PlatformerManager.onInteract -= SetUpInteraction;
        UpgradeShop.onItemExchange -= UpdateLocalItems;
        //-----------------------------------------------------
        //Elevator Operation is Done
        Elevator.onElevatorDone -= ToggleInteraction;
        //Bridge is Passed 
        Bridge.onBridgeDone -= ToggleInteraction;
        PlatformerManager.onGameObjectInteract -= SetUpGameObjectInteraction;
        Door.onDoorTravel -= TeleportPlayer;
        TaxiUI.onPlayerTravel -= TeleportPlayer;

    }

    private void Start()
    {
        playerAnimator.SetTrigger("Idle");
        //currentLocalScale = transform.localScale;
    }
    private void Update()
    {

        Move();
        DebugFunc();
    }

    //This Array holds the items of game: JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
    //This Functions also enables sprinting when localItem[1] = true
    private void UpdateLocalItems(bool[] items)
    {
        localItems = items;
        //SprintBoots: 1 = Enables sprinting
        if (localItems[1])
        {
            playerInputAction.PlayerPlatform.Sprint.Enable();
        }
    }

    private void TeleportPlayer(Vector3 pos, float sec)
    {
        //Debug.Log("Teleport to ")
        DisableInput();
        //playerRigid2D.MovePosition(pos);
        //StartCoroutine(DelayOnTeleport(sec));
        transform.position = pos;
        EnableInput();
    }

    IEnumerator DelayOnTeleport(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

    //These Functions handle basic movement and sprint
    #region HorizontalMovement

    //This function runs on update and it handles player movement for the left and right directions.
    //If the player climb ability is active then this functions moves player up and down.
    private void Move()
    {
        if (enableMove)
        {
            Vector2 _horizontalMovement = playerInputAction.PlayerPlatform.Move.ReadValue<Vector2>();
            if (climb)
            {
                playerRigid2D.gravityScale = 0;
                playerRigid2D.linearVelocity = new Vector2(playerRigid2D.linearVelocity.x, _horizontalMovement.y * playerSpeed);
            }
            else
            {
                playerRigid2D.linearVelocity = new Vector2(_horizontalMovement.x * playerSpeed, playerRigid2D.linearVelocity.y);
            }
        }
    }
    //This Functions just sends a event trigger to PlatformManager to deduct money.
    private void PlayerMoved(InputAction.CallbackContext context)
    {
        onPlayerMove?.Invoke();
    }

    private void SprintStart(InputAction.CallbackContext context)
    {
        playerSpeed = playerSpeed * playerSprintMultiplier;
    }

    private void SprintEnd(InputAction.CallbackContext context)
    {
        playerSpeed = playerSpeed / playerSprintMultiplier;
    }
    #endregion


    //SetUpInteraction function is trigger when player changes a trigger area and from the Platform Manager a signal is send to SetUpInteraction() to change the interaction enum
    //to the correct object: Example if player is in Ladder the interaction enum is set to Interactio.Ladder, 
    //If player presses the interact button which byt default is "X", then the interact function determines what to do with the object:
    //Example: if player presses X while in a ladder, MountLadder Function runs and player mounts the ladder.
    #region Interaction

    //Take the interacted gameobject and assigns it to  currentInteractedGameObject to use it in ladder positioning.s
    private void SetUpGameObjectInteraction(GameObject obj)
    {
        currentInteractedGameObject = obj;
    }

    private void SetUpInteraction(Interaction tempInteraction = Interaction.Ladder)
    {
        interaction = tempInteraction;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        switch (interaction)
        {
            case Interaction.Empty:
                //Debug.Log("No Interaction");
                break;
            case Interaction.Ladder:
                //Interaction Toggle Not needed
                Debug.Log("Mount Ladder");
                if (toggleClimb)
                {
                    //Debug.Log("Player ON Ladder");
                    MountLadder();
                }
                else
                {
                    //Debug.Log("Player OFF Ladder");
                    DismountLadder();
                }
                break;
            case Interaction.Door:
                //Interaction Toggle Not needed
                Debug.Log("Open Door");
                onPlayerOpenDoor?.Invoke();
                break;
            case Interaction.Key:
                Debug.Log("PickUp Key");
                //Interaction Toggle Not needed
                onPlayerPickUpKey?.Invoke();
                platformerManager.KeyNumber++;
                interaction = Interaction.Empty;
                break;
            case Interaction.Elevator:
                if (interactionToggle)
                {
                    Debug.Log("Use Elevator");
                    onPlayerUseElevator?.Invoke();
                    interactionToggle = false;
                }
                break;
            case Interaction.Bridge:
                if (interactionToggle)
                {
                    Debug.Log("PassBridge");
                    onPlayerPassBridge?.Invoke();
                    interactionToggle = false;
                }
                break;
            case Interaction.Shop:
                //Interaction Toggle Not needed
                Debug.Log("Open Shop");
                onPlayerOpenShop?.Invoke();
                break;
            case Interaction.Taxi:
                //Interaction Toggle Not needed
                Debug.Log("Get in Taxi");
                onPlayerGetInTaxi?.Invoke();
                break;
            case Interaction.Sign:
                //Interaction Toggle Not needed
                Debug.Log("Travel");
                if (currentInteractedGameObject.GetComponent<Sign>().nextLevel)
                {
                    NextLevel(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    NextLevel(1);
                }
                break;
            case Interaction.Wallet:
                //Interaction Toggle Not needed
                Debug.Log("Get Wallet");
                GameManager.Instance.GetGameData().walletLevel++;
                Debug.Log("Wallet Level: " + GameManager.Instance.GetGameData().walletLevel);
                onPlayerPickUpWallet?.Invoke();
                break;
            case Interaction.Item:
                //Interaction Toggle Not needed
                Debug.Log("Get Item");
                ItemPickUp();
                break;
            default:
                break;
        }

    }


    private void ItemPickUp()
    {
        ClickerItemSaveData[] currentItemData = GameManager.Instance.GetGameData().clickerItems;

        for (int i = 0; i < currentItemData.Length; i++)
        {
            if (currentInteractedGameObject.GetComponent<TempPlaformItem>().Item.name == currentItemData[i].ID)
            {
                GameManager.Instance.GetGameData().clickerItems[i].unlock = true;
            }
        }
        onPlayerPickUpItem?.Invoke();
    }

    private void NextLevel(int index)
    {
        GameManager.Instance.SaveGame();
        GameManager.Instance.NextLevel(index);
    }

    //When called player dismounts the ladder by enabling the climb value to false.
    private void DismountLadder()
    {
        toggleClimb = true;
        climb = false;
        playerRigid2D.gravityScale = playerDefaultGravityScale;

    }

    //When called player mounts the ladder by enabling the climb value to true.
    private void MountLadder()
    {
        toggleClimb = false;
        DisableMovement();
        if (currentInteractedGameObject != null)
        {
            transform.position = new Vector2(currentInteractedGameObject.transform.position.x, transform.position.y);
        }
        EnableMovement();
        if (localItems != null)
        {
            // ClimbGloves: 3 = Climbing ladders becomes free
            if (!localItems[3])
            {
                //Reduce Money
                onPlayerClimb?.Invoke();
                Debug.Log("Reduce Monay For Climbing");
            }
        }
        climb = true;
        AnimSetClimbing();
    }

    private void ToggleInteraction()
    {
        interactionToggle = true;
    }

    #endregion

    //the FlipDeterminatior and FlipSprite are used to determine which direction the player is going and then flip the sprite according to that direction.
    #region FlipSprite
    private void FlipDeterminator(InputAction.CallbackContext context)
    {
        flipSpriteVector = context.ReadValue<Vector2>();
    }

    //Right is positive; Left is negative
    //flipSpriteVector.x < 0 --> Negative
    //flipSpriteVector.x > 0 --> Positive
    private void FlipSprite(InputAction.CallbackContext context)
    {

        if (flipSpriteVector.x < 0)
        {
            playerModel.localScale = new Vector3(-1, 1, 1);
            //transform.localScale = new Vector3(-currentLocalScale.x, currentLocalScale.y, currentLocalScale.z);
        }
        else
        {
            playerModel.localScale = new Vector3(1, 1, 1);
            //transform.localScale = new Vector3(currentLocalScale.x, currentLocalScale.y, currentLocalScale.z);
        }
        AnimSetWalking();
    }


    #endregion

    //Animation Functions Triggers the animations based on player inputs
    //Example: If player presses Jump(Space) then an AnimSetJumping() will initilize the jumping animation.
    #region Animation
    private void AnimSetIdle(InputAction.CallbackContext context)
    {
        if (!isJumping && !climb)
        {
            playerAnimator.SetTrigger("Idle");
        }
    }

    private void AnimSetWalking()
    {
        if (!isJumping && !climb)
        {
            playerAnimator.SetTrigger("Walk");
        }
    }

    private void AnimSetJumping()
    {
        playerAnimator.SetTrigger("Jump");
    }

    private void AnimSetClimbing()
    {
        playerAnimator.SetTrigger("Climb");
    }

    #endregion

    //The Jump ability works in 3 functions and one coroutine. When player press jump button it triggers the jumpStart function. This functions checks if if the player 
    //grounded using the isGrounded function. After if the player is grounded then it calls the jumping function which is where the actual vertical movement happens.
    //Jumping Function also starts the corotinue for gravity accelaration(GravityMultiplier()). This corotine uses gravity scale gradually increase the gravity until 
    //player hits the ground. And finally if player leaves their hand of the space bar all of these functions gets canceled (used to be used to dynamic jumping but later removed.)
    #region JumpAbility
    private void JumpStart(InputAction.CallbackContext context)
    {
        //DismountLadder();
        if (isGround() && !climb)
        {
            isJumping = true;
            Jumping();
            if (localItems != null)
            {
                // JumpBoots: 0 = Jumping becomes free
                if (!localItems[0])
                {
                    //Reduce Money
                    onPlayerJump?.Invoke();
                    Debug.Log("Reduce Monay For Climbing");
                }
            }
            //Debug.Log("Jump Pressed");
        }
    }

    private void Jumping()
    {
        if (isJumping)
        {
            StopAllCoroutines();
            StartCoroutine(GravityMultiplier());
            //SpringSoles: 2 = Double the Jump Power // If localItems[2] = true: double the jump power
            float trueJumpPower = localItems[2] ? playerJumpPower * playerJumpPowerMultiplier : playerJumpPower;
            playerRigid2D.linearVelocity = new Vector2(playerRigid2D.linearVelocity.x, trueJumpPower);
        }
        AnimSetJumping();
    }

    IEnumerator GravityMultiplier()
    {
        playerGravityActivationTime = playerGravityActivationTimeTemp;
        while (playerRigid2D.gravityScale < playerMaxGravityMultiplier)
        {
            playerRigid2D.gravityScale++;
            yield return new WaitForSeconds(playerGravityActivationTime);
            playerGravityActivationTime--;
        }
    }

    private void JumpEnd(InputAction.CallbackContext context)
    {
        if (isJumping)
        {
            StopAllCoroutines();
            //Debug.Log("Jump Stoped");
            isJumping = false;
            playerRigid2D.linearVelocity = new Vector2(playerRigid2D.linearVelocity.x, playerRigid2D.linearVelocity.y);
        }
    }

    private bool isGround()
    {
        Vector2 size = groundCheck.GetComponent<CapsuleCollider2D>().size;
        Vector2 point = groundCheck.position;
        CapsuleDirection2D direction = groundCheck.GetComponent<CapsuleCollider2D>().direction;
        return Physics2D.OverlapCapsule(point, size, direction, 0, LayerMask.GetMask("Ground"));
        //return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }

    #endregion

    //This function disables the movement of the player when called. It is triggered by platform manager when money is below or equal to 0.
    private void DisableMovement()
    {
        enableMove = false;
        playerRigid2D.linearVelocity = Vector3.zero;
    }

    private void EnableMovement()
    {
        enableMove = true;
    }


    //This function disables the player inputs. It is triggered by platform manager when money is below or equal to 0.
    protected void DisableInput()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
        playerInputAction.PlayerPlatform.Interact.Disable();
    }

    //This function is purely for debug purposes. Is not yet used.
    protected void EnableInput()
    {
        playerInputAction.PlayerPlatform.Move.Enable();
        playerInputAction.PlayerPlatform.Jump.Enable();
        playerInputAction.PlayerPlatform.Interact.Enable();
    }


    //Function used for debbuging
    #region DebugFunc

    private void PrintArray()
    {
        int i = 0;
        foreach (var item in localItems)
        {
            Debug.Log("Item_" + i++ + ": " + item);
        }
    }

    private void DebugFunc()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PrintArray();
        }
    }

    #endregion



}

public enum Interaction
{
    Empty,
    Door,
    Ladder,
    Key,
    Elevator,
    Bridge,
    Shop,
    Taxi,
    Sign,
    Wallet,
    Item

}


/*
 
    if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRigid2D.MovePosition(Vector2.right * speedPlayer);
            //playerRigid2D.AddForce(Vector2.right * speedPlayer, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRigid2D.MovePosition(Vector2.left * speedPlayer);
            //playerRigid2D.AddForce(Vector2.left * speedPlayer, ForceMode2D.Impulse);
        }

  float moveInput = Input.GetAxisRaw("Horizontal");
        playerRigid2D.linearVelocity = new Vector2(moveInput * playerSpeed, playerRigid2D.linearVelocity.y);
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRigid2D.AddForce(Vector2.up * playerJumpHeight, ForceMode2D.Impulse);

        }

        if (!PlatformerManager.moneyBelowZero)
        {
            //Move();
        }
        else
        {
            playerRigid2D.linearVelocity = Vector3.zero;
        }


    private void JumpPressStart(InputAction.CallbackContext context)
    {
        jumpPress = true;
       // Debug.Log(jumpPress);
    }

    private void JumpPressStop(InputAction.CallbackContext context)
    {
        jumpPress = false;

    }

    private void ExtraGravity()
    {
        if (!isGrounded && !jumpPress)
        {
            playerRigid2D.AddForce(Vector2.down * downForce, ForceMode2D.Impulse);
           //Debug.Log("Gravity incresed");
        }
    }

    private void Update()
    {
        ExtraGravity();
        Debug.Log(!isGrounded && !jumpPress);

    }

            //playerRigid2D.AddForce(Vector2.up * playerJumpHeight, ForceMode2D.Impulse);
            //Debug.Log("Context.duration: " + (float)context.duration);



        if (flipSpriteVector.x < 0)
        {
            Debug.Log("flipSpriteVector.x: " + flipSpriteVector.x);
            transform.localScale = new Vector3(flipSpriteVector.x * currentLocalScale.x, currentLocalScale.y, currentLocalScale.z);
            //playerAnimator.SetTrigger("RunLeft");
        }
        else
        {
            Debug.Log("flipSpriteVector.x: " + flipSpriteVector.x);
            //playerAnimator.SetTrigger("RunRight");
            transform.localScale = new Vector3(currentLocalScale.x, currentLocalScale.y, currentLocalScale.z);
        }


    IEnumerator JumpHeightModifier()
    {
        float currentJumpHeight = 0;
        while (currentJumpHeight < playerMaxJumpHeight)
        {
            playerRigid2D.linearVelocity = new Vector2(playerRigid2D.linearVelocity.x, playerJumpPower);
            currentJumpHeight++;


            yield return new WaitForSeconds(0.1f);
        }
        //playerRigid2D.gravityScale = 3;
        //Debug.Log("Gravity Scale: " + playerRigid2D.gravityScale);
    }


    IEnumerator GravityMultiplier()
    {
        while (playerRigid2D.gravityScale < 4)
        {
            playerRigid2D.gravityScale++;
            yield return new WaitForSeconds(0.1f);
        }
        //playerRigid2D.gravityScale = 3;
        //Debug.Log("Gravity Scale: " + playerRigid2D.gravityScale);
    }



    private void EnableLadder(bool climbDetect)
    {
        canClimb = climbDetect;
    }

    private void OnLadder()
    {
        toggleClimb = true;
    }


    private void MountLadder(InputAction.CallbackContext context)
    {
        //Debug.Log("climb: " + climb);
        //Debug.Log("canClimb: " + canClimb);
        climb = canClimb;
        OnLadder();
        if (toggleClimb)
        {
            onPlayerClimb?.Invoke();
            toggleClimb = false;
        }

    }


Old Interaction Logic: 
//The way climb ability works is first we have 3 different onTrigger2D functions in PlatformManager. When onTriggerStay2D sends a event trigger that player is in a ladder
    //When player is in a ladder if they press interact function then climbing mod initiates which at this point there is only 2 ways to get of:
    //1) Player presses jump button to dismount the ladder or the onTrigger2DExit function detects that player is out of the ladder and dismounts the player.
    //Note that current pressing interact again doesnt dismount ladder. (TO DO)



            Vector2 verticalVelocity = new Vector2(playerRigid2D.linearVelocity.x, playerJumpPower);
            Vector2 verticalDoubleVelocity = new Vector2(playerRigid2D.linearVelocity.x, playerJumpPower * 2);

 */