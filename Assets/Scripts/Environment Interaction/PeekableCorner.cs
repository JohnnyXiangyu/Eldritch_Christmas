using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PeekableCorner : HoveringTooltip
{
    [SerializeField] CameraStretcher cameraControl;
    public InputActions input;

    private void Awake()
    {
        input = new InputActions();
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerFOV fov = player.GetComponent<PlayerFOV>();

        input.Gameplay.Peek.performed += ctx =>
        {
            if (tooltip.activeSelf)
            {
                fov.origin = transform.GetChild(1);
                cameraControl.Stretch();
            }
        };

        input.Gameplay.Peek.canceled += cts =>
        {
            if (tooltip.activeSelf)
            {
                fov.origin = player.transform;
                cameraControl.Retract();
            }
        };
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
