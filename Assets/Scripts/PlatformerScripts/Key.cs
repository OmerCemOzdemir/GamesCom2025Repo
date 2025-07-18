using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerControler.onPlayerPickUpKey += PickUpKey;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerPickUpKey -= PickUpKey;
    }

    private void PickUpKey()
    {
        Destroy(gameObject);
    }

}
