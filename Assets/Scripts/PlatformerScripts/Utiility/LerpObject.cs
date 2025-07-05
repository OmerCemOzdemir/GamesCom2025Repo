using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LerpObject : MonoBehaviour
{
    [SerializeField] public float lerpSpeed;
    private Rigidbody2D rb;
    private Vector3 endVector;
    private Vector3 startVector;
    private Vector3 newStartVector;
    private Vector3 newEndVector;
    public static event Action onlerpOpDone;  //Triggered when the lerp operation is done
    public static event Action onlerpOpStart;  //Triggered when the lerp operation is done
    private bool toggleElevator = true;


    private void Start()
    {
        startVector = transform.position;
        endVector = transform.GetChild(0).position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void LerpObjectToPoint()
    {
        newStartVector = transform.position;
        newEndVector = transform.GetChild(0).position;
        StartCoroutine(LerpObjectCoroutine(newStartVector, newEndVector, lerpSpeed));
        //Debug.Log("LerpStarted");
    }

    public void LerpObjectToPoint2()
    {
        StartCoroutine(LerpObjectCoroutine(startVector, endVector, lerpSpeed));
        //Debug.Log("LerpStarted");
    }


    public void LerpObjectBack()
    {
        StartCoroutine(LerpObjectCoroutine(endVector, startVector, lerpSpeed));

    }

    IEnumerator LerpObjectCoroutine(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;

    }

    public void LerpObjectKinematicToPoint()
    {
        if (toggleElevator)
        {
            //StopAllCoroutines();
            onlerpOpStart?.Invoke();
            newStartVector = transform.position;
            newEndVector = transform.GetChild(0).position;
            toggleElevator = false;
            StartCoroutine(LerpObjectKinematicCoroutine(newStartVector, newEndVector, lerpSpeed));
            Debug.Log("LerpStarted");
        }
      
    }

    public void LerpObjectKinematicBack()
    {
        if (toggleElevator)
        {
            //StopAllCoroutines();
            onlerpOpStart?.Invoke();
            toggleElevator = false;
            StartCoroutine(LerpObjectKinematicCoroutine(endVector, startVector, lerpSpeed));
        }
      

    }

    IEnumerator LerpObjectKinematicCoroutine(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            rb.MovePosition(Vector3.Lerp(source, target, (Time.time - startTime) / overTime));
            //transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        rb.MovePosition(target);
        onlerpOpDone?.Invoke();
        toggleElevator = true;
    }


}
