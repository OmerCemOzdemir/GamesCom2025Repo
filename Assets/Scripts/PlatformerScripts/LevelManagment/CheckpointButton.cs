using UnityEngine;

public class CheckpointButton : MonoBehaviour
{

    public void GetCheckpointIndex()
    {
        int index = int.Parse(transform.GetChild(0).name);
        Debug.Log("Checkpoint Button index: " + index);
        transform.root.GetComponent<TaxiUI>().CheckpointIndex = index;
    }

}

/*
     void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GetCheckpointIndex();
        });
    }

 
 */