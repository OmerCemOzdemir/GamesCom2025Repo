using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerControler.onPlayerPickUp += PickUpKey;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerPickUp -= PickUpKey;
    }

    private void PickUpKey()
    {
        Destroy(gameObject);
    }

}
