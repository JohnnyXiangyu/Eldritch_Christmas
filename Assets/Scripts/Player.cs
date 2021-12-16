using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputActions input;
    private Rigidbody2D rb;

    public Vector2 move;
    public float speedMult = 1f;

    void Awake()
    {
        input = new InputActions();

        input.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        input.Gameplay.Movement.canceled += ctx => move = Vector2.zero;

        input.Gameplay.Interact.performed += ctx => Interact();
        input.Gameplay.Peek.performed += ctx => Peek();

        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void Update()
    {
        rb.velocity = move * speedMult;
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
