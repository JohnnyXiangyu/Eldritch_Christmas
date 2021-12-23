using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

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

    public void Stretch()
    {
        // StartCoroutine(StretchRoutine());
        // Debug.Log("Stretch to " + targetPoint.transform.position);
        if (state == State.IDLE || state == State.RETRACTING)
        {
            beforeStretch.Invoke();
            if (playerObj)
                playerObj.GetComponent<Player>().enabled = false;
            state = State.STETCHING;
            DOTween.Kill(mainCam.transform);
            mainCam.transform.DOMove(new Vector3(targetPoint.transform.position.x, targetPoint.transform.position.y, mainCam.transform.position.z), 0.25f).OnComplete(() => 
            {
                state = State.OUT;
                afterStretch.Invoke();
            });
        }
    }

    public void Retract()
    {
        // StartCoroutine(RetractRoutine());
        if (state == State.STETCHING || state == State.OUT)
        {
            beforeStretch.Invoke();
            state = State.RETRACTING;
            mainCam.transform.DOLocalMove(new Vector3(0f, 0f, mainCam.transform.localPosition.z), 0.25f).OnComplete(() => 
            {
                state = State.IDLE;
                if (playerObj)
                    playerObj.GetComponent<Player>().enabled = true;
                afterStretch.Invoke();
            });
        }
    }
}
