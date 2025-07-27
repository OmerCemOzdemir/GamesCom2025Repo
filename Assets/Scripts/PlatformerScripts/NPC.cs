using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Information: ")]
    [SerializeField] private string nameNPC;
    [TextArea]
    [SerializeField] private string dialogNPC;
    [Space(10)]

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;


    private void Start()
    {
        nameText.text = nameNPC;
        dialogText.text = dialogNPC;
    }

    private void EnableText()
    {
        dialogBox.SetActive(true);
    }

    private void DisableText()
    {
        dialogBox.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { EnableText(); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { DisableText(); }

    }


}
