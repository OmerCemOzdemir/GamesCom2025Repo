using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void GetLevelIndex()
    {
        int index = int.Parse(transform.GetChild(0).name);
        //Debug.Log("Level Button index: " + index);

        //Debug.Log("Level firstLevel: " + transform.root.GetComponent<BusUI>().firstLevel);
        if (transform.root.GetComponent<BusUI>().firstLevel)
        {
            transform.root.GetComponent<BusUI>().OpenCheckpointsPanel();
            transform.root.GetComponent<BusUI>().FirstLevel();
        }
        else
        {
            transform.root.GetComponent<BusUI>().LevelIndex = index;
            transform.root.GetComponent<BusUI>().UpdateCheckpoints();
            transform.root.GetComponent<BusUI>().OpenCheckpointsPanel();
        }

    }

}
