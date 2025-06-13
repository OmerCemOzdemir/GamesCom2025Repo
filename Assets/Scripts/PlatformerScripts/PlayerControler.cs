using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D playerRigid2D;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpHeight;
    [SerializeField] private Transform groundCheck;
    private float groundCheckRadius = 0.2f;
    private InputSystem playerInputAction;
    private Vector2 flipSpriteVector;
    private bool isGrounded;
    private bool enableMove = true;

    private void Awake()
    {
        playerRigid2D = GetComponent<Rigidbody2D>();
        playerInputAction = new InputSystem();
    }

    private void OnEnable()
    {
        playerInputAction.PlayerPlatform.Move.Enable();
        playerInputAction.PlayerPlatform.Jump.Enable();
        playerInputAction.PlayerPlatform.Move.started += FlipDeterminator;
        playerInputAction.PlayerPlatform.Move.canceled += FlipDeterminator;
        playerInputAction.PlayerPlatform.Move.performed += FlipSprite;
        playerInputAction.PlayerPlatform.Jump.performed += Jump;
        //-----------------------------------------------------
        PlatformerManager.onMoneyZero += DisableMovement;

    }

    private void OnDisable()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
        playerInputAction.PlayerPlatform.Move.started -= FlipDeterminator;
        playerInputAction.PlayerPlatform.Move.canceled -= FlipDeterminator;
        playerInputAction.PlayerPlatform.Move.performed -= FlipSprite;
        playerInputAction.PlayerPlatform.Jump.performed -= Jump;
        //-----------------------------------------------------
        PlatformerManager.onMoneyZero -= DisableMovement;
    }

    private void Move()
    {
        Vector2 _horizontalMovement = playerInputAction.PlayerPlatform.Move.ReadValue<Vector2>();
        playerRigid2D.linearVelocity = new Vector2(_horizontalMovement.x * playerSpeed, playerRigid2D.linearVelocity.y);
    }

    private void FlipDeterminator(InputAction.CallbackContext context)
    {
        flipSpriteVector = context.ReadValue<Vector2>();
    }

    private void FlipSprite(InputAction.CallbackContext context)
    {
        if (flipSpriteVector.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump Pressed");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
        if (isGrounded)
        {
            StopAllCoroutines();
            playerRigid2D.gravityScale = 1;
            playerRigid2D.AddForce(Vector2.up * playerJumpHeight, ForceMode2D.Impulse);
            StartCoroutine(DownForce());
        }

    }

    IEnumerator DownForce()
    {
        yield return new WaitForSeconds(0.7f);
        playerRigid2D.gravityScale = 4;
        Debug.Log("Gravity Scale: " + playerRigid2D.gravityScale);
    }

    private void DisableMovement()
    {
        enableMove = false;
        playerRigid2D.linearVelocity = Vector3.zero;
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
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

 */