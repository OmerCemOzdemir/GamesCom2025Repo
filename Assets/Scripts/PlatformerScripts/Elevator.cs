using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Elevator : MonoBehaviour
{
    //UNDER CONSTRUCTION
    private Animator elevatorAnimator;
    private LerpObject lerpObject;

    private void Awake()
    {
        elevatorAnimator = GetComponent<Animator>();
        lerpObject = GetComponent<LerpObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ElevatorUp();
            Debug.Log("Elevator Up");
        }

    }

    private void ElevatorUp()
    {
        Debug.Log("Elevator going");
        elevatorAnimator.SetTrigger("ElevatorUp");
        //elevatorRigid2D.MovePosition(Vector2.up * 0.0001f);
        lerpObject.LerpObjectKinematicToPoint();

    }

    //DOES NOT WORK YET!!!!!
    IEnumerator LerpObjectCoroutine(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
        //elevatorRigid2D.MovePosition(target);
    }



}
