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

        input.Gameplay.Peek.performed += ctx =>
        {
            if (tooltip.activeSelf)
            {
                cameraControl.Stretch();
            }
        };

        input.Gameplay.Peek.canceled += cts =>
        {
            if (tooltip.activeSelf)
            {
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
