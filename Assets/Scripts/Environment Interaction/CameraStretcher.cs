using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraStretcher : MonoBehaviour
{
    [SerializeField] GameObject targetPoint;
    [SerializeField] float stretchSpeed;

    [SerializeField] UnityEvent beforeStretch;
    [SerializeField] UnityEvent afterStretch;

    [SerializeField] UnityEvent beforeRetract;
    [SerializeField] UnityEvent afterRetract;

    GameObject playerObj;
    GameObject mainCam;
    GameObject fov;

    Vector3 oldPos = Vector3.positiveInfinity;

    public bool continueStretching = true; // if turned off it'll immediately stop any movement
    private State state;

    // internal 
    Vector3 stretchDirection;
    Vector3 retractDirection;

    private enum State
    {
        IDLE,
        STETCHING,
        OUT,
        RETRACTING,
        LENGTH
    }

    private void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        mainCam = GameObject.FindWithTag("MainCamera");
        fov = playerObj.transform.parent.GetChild(1).gameObject;
    }

    private void Update()
    {
        switch (state)
        {
            case State.STETCHING:
                {
                    if (Get2DDistance(mainCam.transform.position, targetPoint.transform.position) > 0.01f)
                    {
                        if (!continueStretching)
                            break;

                        Vector3 change = stretchDirection.normalized * Mathf.Min(Time.deltaTime * stretchSpeed, (mainCam.transform.position - targetPoint.transform.position).magnitude);
                        mainCam.transform.position += change;
                        fov.transform.position += change;
                    }
                    else
                    {
                        state = State.OUT;
                        afterStretch.Invoke();
                    }

                    break;
                }
            case State.RETRACTING:
                {
                    if (Get2DDistance(mainCam.transform.position, oldPos) > 0.01f)
                    {
                        Vector3 change = retractDirection.normalized * Mathf.Min(Time.deltaTime * stretchSpeed, Get2DDistance(mainCam.transform.position, oldPos));
                        mainCam.transform.position += change;
                        fov.transform.position += change;
                    }
                    else
                    {
                        state = State.IDLE;
                        oldPos = Vector3.positiveInfinity;
                        afterStretch.Invoke();

                        if (playerObj)
                            playerObj.GetComponent<Player>().enabled = true;
                    }

                    break;
                }
        }
    }

    public void Stretch()
    {
        // StartCoroutine(StretchRoutine());
        Debug.Log("Stretch to " + targetPoint.transform.position);
        if (state == State.IDLE)
        {
            stretchDirection = targetPoint.transform.position - mainCam.transform.position;
            if (stretchDirection.magnitude <= 0)
                return;
            
            beforeStretch.Invoke();
            oldPos = mainCam.transform.position;

            if (playerObj)
                playerObj.GetComponent<Player>().enabled = false;

            stretchDirection = targetPoint.transform.position - mainCam.transform.position;
            stretchDirection.z = 0;

            state = State.STETCHING;
        }
        else if (state == State.RETRACTING) // player not unlocked yet
        {
            stretchDirection = targetPoint.transform.position - mainCam.transform.position;
            if (stretchDirection.magnitude <= 0)
                return;

            // beforeStretch.Invoke();
            // oldPos = mainCam.transform.position;

            if (playerObj)
                playerObj.GetComponent<Player>().enabled = false;

            stretchDirection = targetPoint.transform.position - mainCam.transform.position;
            stretchDirection.z = 0;

            state = State.STETCHING;
        }
    }

    public void Retract()
    {
        // StartCoroutine(RetractRoutine());

        if (state == State.STETCHING || state == State.OUT)
        {
            retractDirection = oldPos - mainCam.transform.position;
            retractDirection.z = 0;

            if (oldPos == Vector3.positiveInfinity || retractDirection.magnitude <= 0)
                return;

            beforeStretch.Invoke();

            state = State.RETRACTING;
        }
    }

    float Get2DDistance(Vector3 a, Vector3 b)
    {
        Vector2 a2 = a;
        Vector2 b2 = b;

        return (a2 - b2).magnitude;
    }
}
