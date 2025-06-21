using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private GameObject ladderModel;
    private Vector2 ladderSize = new Vector2(1, 2);
    private bool toggleLadder = true;
    private GameObject[] ladderModels;


    public void IncreaseLadderSize()
    {

        ladderSize += new Vector2(0, 1);
        ApplySizeChanges();
        CreateLadderModels((int)ladderSize.y);
    }

    public void DecreaseLadderSize()
    {
        if (ladderSize.y != 2)
        {
            ladderSize -= new Vector2(0, 1);
        }
        else
        {
            Debug.Log("Reached Minimum Size");
        }
        ApplySizeChanges();
        CreateLadderModels((int)ladderSize.y);
    }


    public void ApplySizeChanges()
    {
        GetComponent<BoxCollider2D>().size = ladderSize;
    }

    private void DestroyLadderModels()
    {
        if (transform.childCount != 0)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }

    private void CreateLadderModels(int num)
    {
        DestroyLadderModels();
        ladderModels = new GameObject[num];
        if (num == 2)
        {
            Instantiate(ladderModel, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity).transform.SetParent(transform);
            Instantiate(ladderModel, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity).transform.SetParent(transform);

        }
        else
        {
            int i = 0;
            while (i < num)
            {
                ladderModels[i] = Instantiate(ladderModel, transform.position + new Vector3(0, 0.5f + i, 0), Quaternion.identity);
                ladderModels[i].transform.SetParent(transform);
                ladderModels[i].transform.position -= new Vector3(0, 0.5f * num, 0);
                i++;
            }

     

            //transform.position -= new Vector()
        }

    }




    private int CalculateTotalLadders()
    {
        //1 - 2 - 4 .. (rest is dividers of 2)
        float sizeX = ladderSize.x;
        float sizeY = ladderSize.y;
        int numberOfLadders = 0;
        if (IsEven(sizeY))
        {
            numberOfLadders = (int)sizeY / 2;
        }
        else
        {
            Debug.LogError("the Y axis in box collider must be even number");
        }
        return numberOfLadders;
    }

    private bool IsEven(float num)
    {
        num = num / 2;
        if (num % 1 != 0)
        {
            //Debug.Log("false " + num % 1);
            return false;
        }
        else
        {
            //Debug.Log("true " + num % 1);
            return true;
        }

    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(Ladder))]
public class LadderCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Ladder lad = (Ladder)target;

        EditorGUILayout.LabelField("Adjust the Ladder Size: ");
        if (GUILayout.Button("Increase Ladder Size", GUILayout.Width(180f)))
        {
            lad.IncreaseLadderSize();
        }

        if (GUILayout.Button("Decrease Ladder Size", GUILayout.Width(180f)))
        {
            lad.DecreaseLadderSize();
        }

    }


}


#endif



/*
 
    private SerializedProperty ladderSize;

    private void OnEnable()
    {
        ladderSize = serializedObject.FindProperty("ladderSize");
    }
        Vector2 newLadderSize = ladderSize.vector2Value;


2/3 = 
 2/2 = 1
2/1 = 0.5

        float newNum;
        for (int i = 0; num < 0; i++)
        {
            newNum = num / 2;
        }
         num = num / 2;
        Debug.Log("Num: " + num);
        if (num == 1)
        {
            return true;
        }
        else
        {
            isEven(num);
        }
        return false;



        int i = 0;
        int j = 0;
        int k = 0;

        while (i < num)
        {

        }

    
            int childNumber = transform.childCount;
            GameObject[] ladderModels = new GameObject[childNumber];


            for (int i = 0; i < childNumber; i++)
            {
                ladderModels[i] = transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < childNumber; i++)
            {
                DestroyImmediate(ladderModels[i]);
                Debug.Log("Current GameObject: " + transform.GetChild(i).gameObject + " i : " + i + " childCount: " + transform.childCount);
            }


        for (int i = 0; i < num; i++)
        {
            if (toggleLadder)
            {
                //1 + 0.5 / -2 + -0.5 / 3 + 0.5 ... -->> Wrong
                //1 + 0.5 / -1 + -0.5 / 2 + 0.05 /-2 + -0.05


                Instantiate(ladderModel, transform.position + new Vector3(0, 0.5f + i, 0), Quaternion.identity).transform.SetParent(transform);
                toggleLadder = false;
            }
            else
            {
                if (j != 0)
                {
                    Instantiate(ladderModel, transform.position + new Vector3(0, -0.5f + -j, 0), Quaternion.identity).transform.SetParent(transform);
                }
                toggleLadder = true;

            }
            Debug.Log("i: " + i + "j: " + j);
        }




        int j = 0;
        int k = 0;
        for (int i = 0; i < num; i++)
        {
            if (toggleLadder)
            {
                //1 + 0.5 / -2 + -0.5 / 3 + 0.5 ... -->> Wrong
                //1 + 0.5 / -1 + -0.5 / 2 + 0.05 /-2 + -0.05
                Instantiate(ladderModel, transform.position + new Vector3(0, k + 1, 0), Quaternion.identity).transform.SetParent(transform);
                j = i;
                toggleLadder = false;
            }
            else
            {
                Instantiate(ladderModel, transform.position + new Vector3(0, -j - 1, 0), Quaternion.identity).transform.SetParent(transform);
                toggleLadder = true;
                k = j++;
            }
            Debug.Log("i: " + i + "j: " + j);
        }



 */