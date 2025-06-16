using System;
using UnityEngine;

public class PlatformerManager : MonoBehaviour
{
    public static event Action onMoneyChange;
    public static event Action onMoneyZero;
    public static event Action<bool> onLadderDetected;
    public static event Action onLadderExit;


    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 1.5f, -10f); // default for Z is -10 to prevent 2D clipping issues
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject stopInteractText;

    
    private void OnEnable()
    {
        PlayerControler.onPlayerJump += ReduceMoneyJump;
        PlayerControler.onSpacePressed += DisableStopInteractText;
        PlayerControler.onPlayerMove += ReduceMoneyMove;
        PlayerControler.onPlayerClimb += ReduceMoneyClimb;
        PlayerControler.onPlayerClimb += EnableStopInteractText;
        //------------------------------------------------------------------
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerJump -= ReduceMoneyJump;
        PlayerControler.onSpacePressed -= DisableStopInteractText;
        PlayerControler.onPlayerMove -= ReduceMoneyMove;
        PlayerControler.onPlayerClimb -= ReduceMoneyClimb;
        PlayerControler.onPlayerClimb -= EnableStopInteractText;
        //------------------------------------------------------------------
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


    //This function is tied to player movement. When player presses a single movement key this function deducts a certain amount of money.
    //After it reduces the money if the current money is below 0 a game will trigger the onMoneyZero event which disables the player controls
    private void ReduceMoneyMove()
    {
        //Vector2 horizontalMovement = context.ReadValue<Vector2>();
        GameManager.Instance.GetGameData().totalMoney -= 10;
        onMoneyChange?.Invoke();
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    //This function is tied to player jump ability. When player succesfully initiates a jump action, this function deducts a certain amount of money.
    //After it reduces the money if the current money is below 0 a game will trigger the onMoneyZero event which disables the player controls
    private void ReduceMoneyJump()
    {
        GameManager.Instance.GetGameData().totalMoney -= 100;
        onMoneyChange?.Invoke();
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }

    //This function is tied to player climb ability. When player succesfully mounts on a ladder, this function deducts a certain amount of money.
    //After it reduces the money if the current money is below 0 a game will trigger the onMoneyZero event which disables the player controls
    private void ReduceMoneyClimb()
    {
        GameManager.Instance.GetGameData().totalMoney -= 1000;
        onMoneyChange?.Invoke();
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
        }
    }


    private void EnableStopInteractText()
    {
        interactText.SetActive(false);
        stopInteractText.SetActive(true); 
    }

    private void DisableStopInteractText()
    {
        stopInteractText.SetActive(false);
        interactText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("This object : " + collision.gameObject);
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder can NOT be used");
            interactText.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("This object : " + collision.gameObject);
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder can be used");
            onLadderDetected?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("This object : " + collision.gameObject);
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder can NOT be used");
            onLadderExit?.Invoke();
            DisableStopInteractText();
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