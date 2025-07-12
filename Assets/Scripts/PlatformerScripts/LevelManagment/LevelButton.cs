using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void GetLevelIndex()
    {
        int index = int.Parse(transform.GetChild(0).name);
        Debug.Log("Level Button index: " + index);
        transform.root.GetComponent<TaxiUI>().LevelIndex = index;
        transform.root.GetComponent<TaxiUI>().UpdateCheckpoints();
        transform.root.GetComponent<TaxiUI>().OpenCheckpointsPanel();
    }

}
