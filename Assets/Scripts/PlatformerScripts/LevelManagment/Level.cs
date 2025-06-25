using UnityEngine;

public class Level : MonoBehaviour
{
    public void GetLevelIndex()
    {
        int index = int.Parse(transform.GetChild(0).name);
        transform.root.GetComponent<TaxiUI>().LevelIndex = index;
    }

}
