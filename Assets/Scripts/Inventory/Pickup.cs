using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : HoveringTooltip
{
    public string itemName;
    public Sprite thumbnail;

    InputActions input;

    private void Awake()
    {
        input = new InputActions();

        input.Gameplay.Interact.performed += context =>
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerInventory>().AddItem(this);
            PickedUp();
            Destroy(gameObject);
        };
    }

    protected abstract void PickedUp();

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
