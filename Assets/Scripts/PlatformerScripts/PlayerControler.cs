using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D playerRigid2D;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpHeight;
    [SerializeField] private Transform groundCheck;
    private float groundCheckRadius = 0.2f;

    private void Start()
    {
        playerRigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlatformerManager.moneyBelowZero)
        {
            Move();
        }
        else
        {
            playerRigid2D.linearVelocity = Vector3.zero;
        }
    }

    private void Move()
    {
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
 */