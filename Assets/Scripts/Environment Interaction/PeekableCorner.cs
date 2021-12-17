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
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerFOV fov = player.GetComponent<PlayerFOV>();

        input.Gameplay.Peek.performed += ctx =>
        {
            if (tooltip.activeSelf)
            {
                player.move = Vector2.zero;
                player.anim.SetBool("moving", false);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.transform.GetChild(0).position = (Vector2)transform.GetChild(1).position;

                fov.origin = transform.GetChild(1);
                cameraControl.Stretch();
            }
        };

        input.Gameplay.Peek.canceled += cts =>
        {
            if (tooltip.activeSelf)
            {
                player.transform.GetChild(0).position = player.transform.position;

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
