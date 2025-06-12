using UnityEngine;
using System;

public class PlatformerManager : MonoBehaviour
{

    public static event Action onMoneyChange;
    public static bool moneyBelowZero = false;

    private void Update()
    {
        CameraFollow();
        if (!moneyBelowZero)
        {
            ReduceMoney();
        }
    }

    private void CameraFollow()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position, Time.deltaTime * 3f);

    }

    private void ReduceMoney()
    {
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

        if (GameManager.Instance.GetGameData().totalMoney < 0)
        {
            moneyBelowZero = true;
        }


    }


}
