using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ArrayDropdown : MonoBehaviour
{
    [SerializeField] private GameObject arrayItemInstance;
    [SerializeField] private Transform parentInputField;
    private GameObject[] arrayInstance;
    private GameObject buttonDropDropdown;
    private GameObject viewPort;


    private bool toggle = true;
    private bool[] itemArray;
    public bool[] ItemArray { get => itemArray; set => itemArray = value; }

    private void Awake()
    {
        buttonDropDropdown = transform.GetChild(1).gameObject;
        viewPort = transform.GetChild(2).gameObject;
        buttonDropDropdown.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Open";
    }

    private void Start()
    {

        arrayInstance = new GameObject[itemArray.Length];
        for (int i = 0; i < itemArray.Length; i++)
        {
            arrayInstance[i] = Instantiate(arrayItemInstance, parentInputField.position, Quaternion.identity);
            arrayInstance[i].transform.SetParent(parentInputField);
            arrayInstance[i].transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Item: " + i;
            arrayInstance[i].transform.GetChild(0).GetComponent<Toggle>().onValueChanged.AddListener((value) =>
            {
                ChangeArray();
            });



        }
    }

    private void ChangeArray()
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            itemArray[i] = arrayInstance[i].transform.GetChild(0).GetComponent<Toggle>().isOn;
        }
        //printArray();
    }

    private void printArray()
    {
        int i = 0;
        foreach (var item in itemArray)
        {
            Debug.Log("Item: " + i++ + " " + item);
        }
    }

    public void ToggleArrayDropdown()
    {
        if (toggle)
        {
            viewPort.SetActive(true);
            toggle = false;
            viewPort.transform.SetParent(transform.root.transform);
            buttonDropDropdown.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Close";
        }
        else
        {
            viewPort.SetActive(false);
            toggle = true;
            viewPort.transform.SetParent(transform);
            buttonDropDropdown.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Open";
        }

        Debug.Log("Item Array Size: " + itemArray.Length);
    }


}
