using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public static event Action onPlayerJump;
    public static event Action onPlayerMove;
    public static event Action onPlayerClimb;
    public static event Action onSpacePressed;


    private Rigidbody2D playerRigid2D;
    [SerializeField] private float playerSpeed = 5; // default value is 5
    [SerializeField] private float playerJumpPower = 13; // default value is 13
    [SerializeField] private float playerGravityActivationTime = 0.6f; // default value is 0.6
    [SerializeField] private float playerDefaultGravityScale = 5; // default value is 5
    [SerializeField] private float playerMaxGravityMultiplier = 7;// default value is 7

    [SerializeField] private Transform playerModel;

    private float playerGravityActivationTimeTemp;
    protected InputSystem playerInputAction;

    [SerializeField] private Transform groundCheck; // assign groundcheck gameObject
    private Animator playerAnimator;

    private float groundCheckRadius = 0.2f;
    private Vector2 flipSpriteVector;
    private Vector3 currentLocalScale;
    private bool enableMove = true;
    private bool isJumping = false;
    private bool climb = false;
    private bool canClimb = false;
    private bool toggleClimb = false;

    private void Awake()
    {
        playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        playerRigid2D = GetComponent<Rigidbody2D>();
        playerGravityActivationTimeTemp = playerGravityActivationTime;
        playerRigid2D.gravityScale = playerDefaultGravityScale;
        playerInputAction = new InputSystem();
    }

    private void OnEnable()
    {
        playerInputAction.PlayerPlatform.Move.Enable();
        playerInputAction.PlayerPlatform.Jump.Enable();
        playerInputAction.PlayerPlatform.Interact.Enable();

        playerInputAction.PlayerPlatform.Interact.performed += MountLadder;

        playerInputAction.PlayerPlatform.Move.performed += FlipSprite;
        playerInputAction.PlayerPlatform.Move.started += FlipDeterminator;
        playerInputAction.PlayerPlatform.Move.canceled += AnimSetIdle;
        playerInputAction.PlayerPlatform.Move.performed += PlayerMoved;


        playerInputAction.PlayerPlatform.Jump.started += JumpStart;
        playerInputAction.PlayerPlatform.Jump.canceled += JumpEnd;
        //-----------------------------------------------------
        PlatformerManager.onMoneyZero += DisableMovement;
        PlatformerManager.onMoneyZero += DisableInput;
        PlatformerManager.onLadderDetected += EnableLadder;
        PlatformerManager.onLadderExit += DismountLadder;

    }

    private void OnDisable()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
        playerInputAction.PlayerPlatform.Interact.Disable();

        playerInputAction.PlayerPlatform.Interact.performed -= MountLadder;

        playerInputAction.PlayerPlatform.Move.performed -= FlipSprite;
        playerInputAction.PlayerPlatform.Move.started -= FlipDeterminator;

        playerInputAction.PlayerPlatform.Move.canceled -= AnimSetIdle;

        playerInputAction.PlayerPlatform.Jump.started -= JumpStart;
        playerInputAction.PlayerPlatform.Jump.canceled -= JumpEnd;
        //-----------------------------------------------------
        PlatformerManager.onMoneyZero -= DisableMovement;
        PlatformerManager.onMoneyZero -= DisableInput;
        PlatformerManager.onLadderDetected -= EnableLadder;
        PlatformerManager.onLadderExit -= DismountLadder;
    }


    private void Start()
    {
        playerAnimator.SetTrigger("Idle");
        currentLocalScale = transform.localScale;
    }


    private void Move()
    {
        Vector2 _horizontalMovement = playerInputAction.PlayerPlatform.Move.ReadValue<Vector2>();
        if (climb && canClimb)
        {
            playerRigid2D.gravityScale = 0;
            playerRigid2D.linearVelocity = new Vector2(playerRigid2D.linearVelocity.x, _horizontalMovement.y * playerSpeed);
        }
        else
        {
            playerRigid2D.linearVelocity = new Vector2(_horizontalMovement.x * playerSpeed, playerRigid2D.linearVelocity.y);
        }

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

    private void DismountLadder()
    {
        climb = false;
        canClimb = false;
        playerRigid2D.gravityScale = playerDefaultGravityScale;
    }

    private void EnableLadder(bool climbDetect)
    {
        canClimb = climbDetect;
    }

    private void OnLadder()
    {
        toggleClimb = true;
    }

    private void PlayerMoved(InputAction.CallbackContext context)
    {
        onPlayerMove?.Invoke();
    }

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

    private void AnimSetIdle(InputAction.CallbackContext context)
    {
        if (!isJumping)
        {
            playerAnimator.SetTrigger("Idle");
        }
    }

    private void AnimSetWalking()
    {
        if (!isJumping)
        {
            playerAnimator.SetTrigger("Walk");
        }
    }

    private void AnimSetJumping()
    {
        playerAnimator.SetTrigger("Jump");
    }

    //-----------------------------------------------------------------
    private void JumpStart(InputAction.CallbackContext context)
    {
        DismountLadder();
        onSpacePressed?.Invoke();
        if (isGround())
        {
            isJumping = true;
            Jumping();
            onPlayerJump?.Invoke();
            //Debug.Log("Jump Pressed");
        }

    }

    private void Jumping()
    {
        if (isJumping)
        {
            StopAllCoroutines();
            StartCoroutine(GravityMultiplier());
            playerRigid2D.linearVelocity = new Vector2(playerRigid2D.linearVelocity.x, playerJumpPower);
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
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }

    private void DisableMovement()
    {
        enableMove = false;
        playerRigid2D.linearVelocity = Vector3.zero;
    }

    protected void DisableInput()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
        playerInputAction.PlayerPlatform.Interact.Disable();
    }

    protected void EnableInput()
    {
        playerInputAction.PlayerPlatform.Move.Enable();
        playerInputAction.PlayerPlatform.Jump.Enable();
        playerInputAction.PlayerPlatform.Interact.Enable();
    }

    private void Update()
    {
        if (enableMove)
        {
            Move();
        }
    }

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





 */