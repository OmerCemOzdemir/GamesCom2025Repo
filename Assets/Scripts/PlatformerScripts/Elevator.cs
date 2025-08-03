using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Elevator : MonoBehaviour
{
    public static event Action onElevatorDone;

    [Header("Elevator Variables")]
    [SerializeField] private float elevatorCoolDown;
    [SerializeField] private float elevatorSpeed;
    [Space(10)]
    //private LerpObject lerpObject;
    private bool toggleElevator = true;
    private bool allowElevatorOp = true;
    private bool isPlayerOn = false;
    private GameObject player;
    Rigidbody2D rb2D;


    [Header("Do not touch")]
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem cooldownEffect;
    [SerializeField] private ParticleSystem burstEffect;


    private Vector3 endVector;
    private Vector3 startVector;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
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
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerManager>().DisableInteraction();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerManager>().DisableInteractText();


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
        StartCoroutine(LerpObjectKinematicCoroutine(endVector, elevatorSpeed));

    }

    private void ElevatorDown()
    {

        //Debug.Log("Elevator Down ");
        //Debug.Log("Star Vector: " + endVector + " End Vector: " + startVector);
        StartCoroutine(LerpObjectKinematicCoroutine(startVector, elevatorSpeed));
    }


    public void ElevatorStop()
    {
        //elevatorAnimator.SetTrigger("ElevatorStop");
        cooldownEffect.Play();
        burstEffect.Play();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerManager>().EnableInteraction();
        if (isPlayerOn)
        {
            player.transform.SetParent(null, true);
            isPlayerOn = false;
            player.GetComponent<PlayerControler>().EnableInput();
        }
        Debug.Log("Elevator Stoped");
        StartCoroutine(ElevatorCoolDown(elevatorCoolDown));
    }


    IEnumerator ElevatorCoolDown(float sec)
    {
        yield return new WaitForSeconds(sec);
        cooldownEffect.Stop();
        burstEffect.Stop();
        allowElevatorOp = !allowElevatorOp;
        //Player can use the interaction for elevator again
        onElevatorDone?.Invoke();
    }


    IEnumerator LerpObjectKinematicCoroutine(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) >= 0.01f)
        {
            // Move a constant amount per frame based on speed and time
            rb2D.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
            yield return null; // Wait for the next frame
        }
        Debug.Log("Elevator Corotine is done");
        rb2D.MovePosition(target);
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
 * 
 *  float startTime = Time.time;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        while (rb.position.y >= end.y)
        {
            rb.MovePosition(Vector3.MoveTowards(start, end, speed));
            //transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            // (Time.time - startTime) / overTime)
            //Time.time < startTime + overTime
            yield return null;
        }
        rb.MovePosition(end);
 * 
 * 
 *         while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            // Move a constant amount per frame based on speed and time
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Optional: Snap to exact destination at the end
        transform.position = destination;
 * 
 * 
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