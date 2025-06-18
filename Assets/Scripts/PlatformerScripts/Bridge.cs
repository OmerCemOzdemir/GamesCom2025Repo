using TMPro;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private BoxCollider2D bridgeBlock;
    private TextMeshProUGUI bridgeText_0;
    private TextMeshProUGUI bridgeText_1;
    private void Awake()
    {
        bridgeBlock = transform.GetChild(1).GetComponent<BoxCollider2D>();
        bridgeText_0 = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        bridgeText_1 = transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        PlayerControler.onPlayerPassBridge += unBlockBridge;
        PlatformerManager.onBridgeExit += BlockBridge;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerPassBridge -= unBlockBridge;
        PlatformerManager.onBridgeExit -= BlockBridge;
    }

    void unBlockBridge()
    {
        bridgeBlock.enabled = false;
        BridgeOpenPaid();
    }

    void BlockBridge()
    {
        bridgeBlock.enabled = true;

    }

    private void BridgeTextClose()
    {
        bridgeText_0.gameObject.SetActive(false);
        bridgeText_1.gameObject.SetActive(false);   

    }


    private void BridgeTextOpenToll()
    {
        bridgeText_0.gameObject.SetActive(true);
        bridgeText_1.gameObject.SetActive(true);
        //Money Required to Pass
        bridgeText_0.text = "Money Required to Pass";
        bridgeText_1.text = "Money Required to Pass";

    }

    private void BridgeOpenPaid()
    {
        bridgeText_0.gameObject.SetActive(true);
        bridgeText_1.gameObject.SetActive(true);
        bridgeText_0.text = "The bridge is paid.";
        bridgeText_1.text = "The bridge is paid.";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BridgeTextOpenToll();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BridgeTextClose();
        }
    }

}
