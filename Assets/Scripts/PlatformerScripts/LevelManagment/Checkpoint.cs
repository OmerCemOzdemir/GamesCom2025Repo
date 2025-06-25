using System;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{

    public void GetCheckpointIndex()
    {
        int index = int.Parse(transform.GetChild(0).name);
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