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

    Vector3 oldPos = Vector3.positiveInfinity;

    public bool continueStretching = true; // if turned off it'll immediately stop any movement

    private void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        mainCam = GameObject.FindWithTag("MainCamera");
    }

    public void Stretch()
    {
        StartCoroutine(StretchRoutine());
    }

    public void Retract()
    {
        StartCoroutine(RetractRoutine());
    }

    private IEnumerator StretchRoutine()
    {
        if (playerObj)
            playerObj.GetComponent<Player>().enabled = false;

        beforeStretch.Invoke();

        oldPos = mainCam.transform.position;

        Vector3 direction = targetPoint.transform.position - mainCam.transform.position;
        direction.z = 0;

        if (direction.magnitude > 0)
        {
            while (Get2DDistance(mainCam.transform.position, targetPoint.transform.position) > 0.01f)
            {
                if (!continueStretching)
                    break;

                mainCam.transform.position += direction.normalized * Mathf.Min(Time.deltaTime * stretchSpeed, (mainCam.transform.position - targetPoint.transform.position).magnitude);
                yield return null;
            }
        }

        afterStretch.Invoke();
    }

    private IEnumerator RetractRoutine()
    {
        continueStretching = false;

        beforeStretch.Invoke();

        Vector3 direction = oldPos - mainCam.transform.position;
        direction.z = 0;

        if (oldPos != Vector3.positiveInfinity && direction.magnitude > 0)
        {
            while (Get2DDistance(mainCam.transform.position, oldPos) > 0.01f)
            {
                mainCam.transform.position += direction.normalized * Mathf.Min(Time.deltaTime * stretchSpeed, Get2DDistance(mainCam.transform.position, oldPos));
                yield return null;
            }

            oldPos = Vector3.positiveInfinity;
        }

        afterStretch.Invoke();

        if (playerObj)
            playerObj.GetComponent<Player>().enabled = true;

        continueStretching = true;
    }

    float Get2DDistance(Vector3 a, Vector3 b)
    {
        Vector2 a2 = a;
        Vector2 b2 = b;

        return (a2 - b2).magnitude;
    }
}
