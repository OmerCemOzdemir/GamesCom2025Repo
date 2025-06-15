using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerManager : MonoBehaviour
{

    public static event Action onMoneyChange;
    public static event Action onMoneyZero;

    protected InputSystem playerInputAction;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 1.5f, -10f); // default for Z is -10 to prevent 2D clipping issues

    protected virtual void Awake()
    {
        playerInputAction = new InputSystem();
    }

    protected virtual void OnEnable()
    {
        playerInputAction.PlayerPlatform.Move.Enable();
        playerInputAction.PlayerPlatform.Jump.Enable();

        playerInputAction.PlayerPlatform.Move.started += ReduceMoneyMove;
        playerInputAction.PlayerPlatform.Jump.performed += ReduceMoneyJump;
        //------------------------------------------------------------------
        onMoneyZero += DisableInput;
    }

    protected virtual void OnDisable()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();

        playerInputAction.PlayerPlatform.Move.started -= ReduceMoneyMove;
        playerInputAction.PlayerPlatform.Jump.performed -= ReduceMoneyJump;
        //------------------------------------------------------------------
        onMoneyZero += DisableInput;
    }

    protected void DisableInput()
    {
        playerInputAction.PlayerPlatform.Move.Disable();
        playerInputAction.PlayerPlatform.Jump.Disable();
    }

    protected void EnableInput()
    {
        playerInputAction.PlayerPlatform.Move.Enable();
        playerInputAction.PlayerPlatform.Jump.Enable();
    }

    private void Update()
    {
        CameraFollow();
    }

    private void CameraFollow()
    {
        Vector3 targetPosition = transform.position + cameraOffset; // apply offset to camera position
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * 3f);

    }

    private void ReduceMoneyMove(InputAction.CallbackContext context)
    {
        //Vector2 horizontalMovement = context.ReadValue<Vector2>();
        GameManager.Instance.GetGameData().totalMoney -= 10;
        onMoneyChange?.Invoke();
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();

        }
    }

    private void ReduceMoneyJump(InputAction.CallbackContext context)
    {
        GameManager.Instance.GetGameData().totalMoney -= 100;
        onMoneyChange?.Invoke();
        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            onMoneyZero?.Invoke();
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