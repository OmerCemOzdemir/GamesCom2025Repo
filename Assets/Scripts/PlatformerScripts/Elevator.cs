using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
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

    private void OnEnable()
    {
        PlayerControler.onPlayerUseElevator += ToggleElevator;
        LerpObject.onlerpOpDone += ElevatorStop;
    }

    private void OnDisable()
    {
        PlayerControler.onPlayerUseElevator -= ToggleElevator;
        LerpObject.onlerpOpDone -= ElevatorStop;
    }


    private void ToggleElevator()
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


    private void ElevatorStop()
    {
        elevatorAnimator.SetTrigger("ElevatorStop");
        player.transform.SetParent(null, true);
        StartCoroutine(ElevatorCoolDown(elevatorCoolDown));
    }


    IEnumerator ElevatorCoolDown(float sec)
    {
        yield return new WaitForSeconds(sec);
        allowElevatorOp = !allowElevatorOp;
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