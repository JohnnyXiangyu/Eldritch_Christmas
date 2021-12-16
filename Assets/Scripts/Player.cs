using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputActions input;

    public Vector2 move;
    public float speedMult = 1f;

    void Awake()
    {
        input = new InputActions();

        input.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        input.Gameplay.Movement.canceled += ctx => move = Vector2.zero;

        input.Gameplay.Interact.performed += ctx => Interact();
        input.Gameplay.Peek.performed += ctx => Peek();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void Update()
    {
        transform.position += new Vector3(move.x, move.y, 0f) * speedMult;
    }

    void Interact()
    {
        
    }

    void Peek()
    {

    }

    void OnDisable()
    {
        input.Disable();
    }
}
