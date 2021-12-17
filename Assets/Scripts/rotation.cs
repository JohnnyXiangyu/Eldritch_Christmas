using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotaion : MonoBehaviour
{
    private Quaternion startRotation;

    [SerializeField]
    private float frequency = 5f;

    [SerializeField]
    private float angle = 180f;

    [SerializeField]
    private float offset = 0f; 

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = startRotation * Quaternion.AngleAxis(Mathf.Sin(Time.time * frequency + offset) * angle, transform.forward);
    }
}
