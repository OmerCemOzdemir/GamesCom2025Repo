using System;
using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public static event Action onElevatorDone;

    [SerializeField] private float elevatorCoolDown;
    private Animator elevatorAnimator;
    private LerpObject lerpObject;
    private bool toggleElevator = true;
    private bool allowElevatorOp = true;
    private GameObject player;

    private void Awake()
    {
        elevatorAnimator = GetComponent<Animator>();
        lerpObject = GetComponent<LerpObject>();
    }

    public void ToggleElevator()
    {
        if (allowElevatorOp)
        {
            if (toggleElevator)
            {
                player.transform.SetParent(transform, true);
                ElevatorUp();
                toggleElevator = false;
                //Debug.Log("Elevator Up");
            }
            else
            {
                player.transform.SetParent(transform, true);
                ElevatorDown();
                toggleElevator = true;
            }
            allowElevatorOp = false;
        }
       
    }

    private void ElevatorUp()
    {
        //Debug.Log("Elevator going");
        elevatorAnimator.SetTrigger("ElevatorUp");
        lerpObject.LerpObjectKinematicToPoint();

    }

    private void ElevatorDown()
    {
        //Debug.Log("Elevator going");
        elevatorAnimator.SetTrigger("ElevatorUp");
        lerpObject.LerpObjectKinematicBack();
    }


    public void ElevatorStop()
    {
        elevatorAnimator.SetTrigger("ElevatorStop");
        player.transform.SetParent(null, true);
        StartCoroutine(ElevatorCoolDown(elevatorCoolDown));
    }


    IEnumerator ElevatorCoolDown(float sec)
    {
        yield return new WaitForSeconds(sec);
        allowElevatorOp = !allowElevatorOp;
        //Player can use the interaction for elevator again
        onElevatorDone?.Invoke();
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



 */