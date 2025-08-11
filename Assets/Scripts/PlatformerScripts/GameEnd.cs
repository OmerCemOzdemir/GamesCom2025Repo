using UnityEngine;

public class GameEnd : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.GoToLevel("EndScene");
        }
    }

}
