using UnityEngine;

public class Wallet : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerControler.onPlayerPickUpWallet += PickUpWallet;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerPickUpWallet -= PickUpWallet;
    }

    private void PickUpWallet()
    {
        Destroy(gameObject);
    }

}
