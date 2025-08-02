using System;
using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public static event Action onElevatorDone;

    [SerializeField] private float elevatorCoolDown;
    private Animator elevatorAnimator;
    //private LerpObject lerpObject;
    private bool toggleElevator = true;
    private bool allowElevatorOp = true;
    private bool isPlayerOn = false;
    private GameObject player;

    [SerializeField] private Transform target;
    [SerializeField] private float time;
    private Vector3 endVector;
    private Vector3 startVector;

    private void Awake()
    {
        elevatorAnimator = GetComponent<Animator>();
        //lerpObject = GetComponent<LerpObject>();
        endVector = target.position;
        startVector = transform.position;

    }


    public void ToggleElevator()
    {
        if (allowElevatorOp)
        {
            if (player != null)
            {
                player.transform.SetParent(transform, true);
                isPlayerOn = true;
                player.GetComponent<PlayerControler>().DisableInput();
            }

            if (toggleElevator)
            {
                ElevatorUp();
                toggleElevator = false;
                //Debug.Log("Elevator Up");
            }
            else
            {

                ElevatorDown();
                toggleElevator = true;
            }
            allowElevatorOp = false;
            Debug.Log("Elevator toggleElevator: " + toggleElevator);
        }

    }

    public void ToggleElevatorWithOutPlayer()
    {
        if (allowElevatorOp)
        {
            if (toggleElevator)
            {
                ElevatorUp();
                toggleElevator = false;
                //Debug.Log("Elevator Up");
            }
            else
            {

                ElevatorDown();
                toggleElevator = true;
            }
            allowElevatorOp = false;
            Debug.Log("Elevator toggleElevator: " + toggleElevator);
        }
    }


    private void ElevatorUp()
    {

        //Debug.Log("Elevator Up ");

        //Debug.Log("Star Vector: " + startVector + " End Vector: " + endVector);
        StartCoroutine(LerpObjectKinematicCoroutine(startVector, endVector, time));

    }

    private void ElevatorDown()
    {

        //Debug.Log("Elevator Down ");
        //Debug.Log("Star Vector: " + endVector + " End Vector: " + startVector);
        StartCoroutine(LerpObjectKinematicCoroutine(endVector, startVector, time));
    }


    public void ElevatorStop()
    {
        //elevatorAnimator.SetTrigger("ElevatorStop");
        if (isPlayerOn)
        {
            player.transform.SetParent(null, true);
            isPlayerOn = false;
            player.GetComponent<PlayerControler>().EnableInput();
        }
        StartCoroutine(ElevatorCoolDown(elevatorCoolDown));
    }


    IEnumerator ElevatorCoolDown(float sec)
    {
        yield return new WaitForSeconds(sec);
        allowElevatorOp = !allowElevatorOp;
        //Player can use the interaction for elevator again
        onElevatorDone?.Invoke();
    }


    IEnumerator LerpObjectKinematicCoroutine(Vector3 start, Vector3 end, float overTime)
    {
        float startTime = Time.time;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (Time.time < startTime + overTime)
        {
            rb.MovePosition(Vector3.LerpUnclamped(start, end, (Time.time - startTime) / overTime));
            //transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        rb.MovePosition(end);
        ElevatorStop();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }

    }

    private void OnApplicationQuit()
    {
        if (player != null)
        {
            player.transform.SetParent(null, true);
        }
    }

}

/*
     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (toggleElevator)
            {
                ElevatorUp();
                toggleElevator = false;
                //Debug.Log("Elevator Up");
            }
            else
            {
                ElevatorDown();
                toggleElevator = true;
            }
        }
    }


        //Debug.Log("Elevator going");
        //elevatorAnimator.SetTrigger("ElevatorUp");
        //lerpObject.LerpObjectKinematicToPoint();

        //Debug.Log("Elevator going");
        //elevatorAnimator.SetTrigger("ElevatorUp");
        //lerpObject.LerpObjectKinematicBack();

        //player.transform.position = target;
        //player.GetComponent<Rigidbody2D>().MovePosition(target);
        //onlerpOpDone?.Invoke();
        //toggleElevator = true;
 */